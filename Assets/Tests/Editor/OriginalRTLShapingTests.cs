using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using RTLTMPro;

namespace ProjectLucid.Tests
{
    // Original shared shaping buffers are not thread-safe. Each stateful case begins
    // with the genuine public operation that establishes its own algorithm state.
    public sealed class OriginalRTLShapingTests
    {
        private const BindingFlags Declared = BindingFlags.DeclaredOnly | BindingFlags.Public
            | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        [Test]
        public void CompleteHandwrittenDeclarationsAndOriginalOptionalDefaultsArePreserved()
        {
            Assembly assembly = typeof(FastStringBuilder).Assembly;
            Assert.That(assembly.GetName().Name, Is.EqualTo("HLRTLTextMeshPro.Runtime"));
            foreach (string expected in TypeDeclarations)
            {
                string[] cells = expected.Split('|');
                Type type = assembly.GetType(cells[0], true, false);
                Assert.That((int)type.Attributes, Is.EqualTo(int.Parse(cells[1])), cells[0]);
                Assert.That(type.BaseType == null ? "" : TypeName(type.BaseType), Is.EqualTo(cells[2]), cells[0]);
                RequireGenuineScope(type);
                if (type.BaseType != null) RequireGenuineScope(type.BaseType);
                foreach (FieldInfo field in type.GetFields(Declared)) RequireGenuineScope(field.FieldType);
                string[] fields = type.GetFields(Declared).OrderBy(f => f.MetadataToken)
                    .Select(f => type.FullName + "|" + f.Name + "|" + TypeName(f.FieldType)
                        + "|" + (int)f.Attributes + "|" + (f.IsLiteral ? Convert.ToString(f.GetRawConstantValue(), System.Globalization.CultureInfo.InvariantCulture) : ""))
                    .ToArray();
                CollectionAssert.AreEqual(FieldDeclarations.Where(row => row.StartsWith(type.FullName + "|", StringComparison.Ordinal)).ToArray(), fields);
                MethodBase[] methods = type.GetMethods(Declared).Cast<MethodBase>()
                    .Concat(type.GetConstructors(Declared)).Distinct().OrderBy(m => m.MetadataToken).ToArray();
                CollectionAssert.AreEqual(MethodDeclarations.Where(row => row.StartsWith(type.FullName + "|", StringComparison.Ordinal)).ToArray(),
                    methods.Select(m => type.FullName + "|" + Signature(m) + "|" + (int)m.Attributes + "|" + (int)m.GetMethodImplementationFlags()).ToArray());
            }
            foreach (string expected in TypeDeclarations)
            {
                Type type = assembly.GetType(expected.Split('|')[0], true, false);
                var actual = CustomAttributeData.GetCustomAttributes(type).Select(c => AttributeLine(type.FullName, c))
                    .Concat(type.GetFields(Declared).OrderBy(f => f.MetadataToken)
                        .SelectMany(f => CustomAttributeData.GetCustomAttributes(f).Select(c => AttributeLine(type.FullName + "." + f.Name, c)))).ToArray();
                CollectionAssert.AreEqual(AttributeDeclarations.Where(row => row.StartsWith(type.FullName + "|", StringComparison.Ordinal)
                    || row.StartsWith(type.FullName + ".", StringComparison.Ordinal)).ToArray(), actual);
            }
            ParameterInfo[] parameters = typeof(RTLSupport).GetMethod("FixRTL").GetParameters();
            CollectionAssert.AreEqual(new[] { "input", "output", "farsi", "fixTextTags", "preserveNumbers" }, parameters.Select(p => p.Name));
            CollectionAssert.AreEqual(new[] { false, false, true, true, true }, parameters.Select(p => p.IsOptional));
            CollectionAssert.AreEqual(new object[] { true, true, false }, parameters.Skip(2).Select(p => p.DefaultValue));
            Assert.That(typeof(RichTextFixer.Tag).GetFields(Declared).Select(f => f.Name), Is.EquivalentTo(new[] { "Start", "End", "HashCode", "Type" }));
        }

        [Test]
        public void SignedChar32GateAndNumberSelectionRetainTheirBoundaries()
        {
            Assert.That(Char32Utils.IsUnicode16Char(-1), Is.True);
            Assert.That(Char32Utils.IsUnicode16Char(65534), Is.True);
            Assert.That(Char32Utils.IsUnicode16Char(65535), Is.False);
            Assert.That(Char32Utils.IsRTLCharacter(unchecked(0x0628 - 65536)), Is.True);
            Assert.That(Char32Utils.IsRTLCharacter(0x10000), Is.False);
            Assert.That(Char32Utils.IsEnglishLetter(unchecked('A' - 65536)), Is.True);
            Assert.That(Char32Utils.IsEnglishLetter('['), Is.False);
            Assert.That(Char32Utils.IsNumber('5', true, false), Is.True);
            Assert.That(Char32Utils.IsNumber('\u06f5', false, true), Is.True);
            Assert.That(Char32Utils.IsNumber('\u0665', false, false), Is.True);
            Assert.That(Char32Utils.IsNumber('\u06f5', false, false), Is.False);
        }

        [Test]
        public void FirstVisibleLetterAndTagClassificationPreserveTheOriginalThrow()
        {
            Assert.That(TextUtils.IsRTLInput("<b>\u0628</b> A"), Is.True);
            Assert.That(TextUtils.IsRTLInput("A \u0628"), Is.False);
            Assert.That(TextUtils.IsRTLInput("12 <b>\u0628</b>"), Is.True);
            Assert.That(TextUtils.IsRTLInput("<unfinished \u0628"), Is.False);
            Assert.That(TextUtils.IsRTLInput("123!?"), Is.False);
            Assert.That(TextUtils.IsArabicCharacter('\u0870'), Is.True);
            Assert.That(TextUtils.IsArabicCharacter('\u08a0'), Is.True);
            Assert.That(TextUtils.IsArabicCharacter('\u0900'), Is.False);
            Assert.Throws<NotImplementedException>(() => TextUtils.IsPunctuation('!'));
        }

        [Test]
        public void GenuineGlyphTableAndAllFourLamJoinsRemainLiteral()
        {
            Assert.That(GlyphTable.Convert('\u0628'), Is.EqualTo('\ufe8f'));
            Assert.That(GlyphTable.Convert('A'), Is.EqualTo('A'));
            int[] alefs = { 0x0622, 0x0623, 0x0625, 0x0627 };
            int[] joined = { 0xfef5, 0xfef7, 0xfef9, 0xfefb };
            for (int i = 0; i < alefs.Length; i++)
            {
                // Native Lam replacements FEF3/5/7/9 subsequently gain +2 from
                // the genuine leading-letter branch. The consumed cell stays FFFF.
                var input = new FastStringBuilder("\u0644" + (char)alefs[i], 16);
                var output = new FastStringBuilder(16);
                GlyphFixer.Fix(input, output, true, false, false);
                Assert.That(output.Length, Is.EqualTo(2));
                Assert.That(output.Get(0), Is.EqualTo(joined[i]));
                Assert.That(output.Get(1), Is.EqualTo(0xffff));
            }
            var pairInput = new FastStringBuilder("\u0628\u0628", 16);
            var pairOutput = new FastStringBuilder(16);
            GlyphFixer.Fix(pairInput, pairOutput, true, false, false);
            Assert.That(pairOutput.ToString(), Is.EqualTo("\ufe91\ufe90"));
        }

        [Test]
        public void YahAndNumberTagProtectionUseGenuinePublicTransformations()
        {
            var yah = new FastStringBuilder("\u064a\u06cc", 32);
            GlyphFixer.FixYah(yah, true);
            Assert.That(yah.ToString(), Is.EqualTo("\u06cc\u06cc"));
            GlyphFixer.FixYah(yah, false);
            Assert.That(yah.ToString(), Is.EqualTo("\u064a\u064a"));
            var tags = new FastStringBuilder("<size=2>3</size>", 32);
            GlyphFixer.FixNumbersOutsideOfTags(tags, true);
            Assert.That(tags.ToString(), Is.EqualTo("<size=2>\u06f3</size>"));
            var malformed = new FastStringBuilder("< 2>3", 32);
            GlyphFixer.FixNumbersOutsideOfTags(malformed, false);
            Assert.That(malformed.ToString(), Is.EqualTo("< \u0662>\u0663"));
            var digits = new FastStringBuilder("09x", 32);
            GlyphFixer.FixNumbers(digits, false);
            Assert.That(digits.ToString(), Is.EqualTo("\u0660\u0669x"));
        }

        [Test]
        public void RichTextTagFieldsAndWholeTagReversalKeepOriginalOrder()
        {
            var constructed = new RichTextFixer.Tag(-2, 9, RichTextFixer.TagType.Closing, 37);
            Assert.That(constructed.Start, Is.EqualTo(-2));
            Assert.That(constructed.End, Is.EqualTo(9));
            Assert.That(constructed.Type, Is.EqualTo(RichTextFixer.TagType.Closing));
            Assert.That(constructed.HashCode, Is.EqualTo(37));
            var text = new FastStringBuilder("x<b>y</b><br/>", 32);
            RichTextFixer.FindTag(text, 0, out RichTextFixer.Tag first);
            Assert.That(first.Start, Is.EqualTo(1));
            Assert.That(first.End, Is.EqualTo(3));
            Assert.That(first.Type, Is.EqualTo(RichTextFixer.TagType.Opening));
            Assert.That(first.HashCode, Is.EqualTo((int)'b'));
            RichTextFixer.FindTag(text, 5, out RichTextFixer.Tag closing);
            Assert.That(closing.Type, Is.EqualTo(RichTextFixer.TagType.Closing));
            RichTextFixer.FindTag(text, 9, out RichTextFixer.Tag self);
            Assert.That(self.Type, Is.EqualTo(RichTextFixer.TagType.SelfContained));
            RichTextFixer.Fix(text);
            Assert.That(text.ToString(), Is.EqualTo("x>b<y>b/<>/rb<"));
            RichTextFixer.FindTag(new FastStringBuilder("< b>"), 0, out RichTextFixer.Tag none);
            Assert.That(none.Type, Is.EqualTo(RichTextFixer.TagType.None));
            Assert.That(none.Start, Is.Zero);
        }

        [Test]
        public void TashkeelPositionsAndRepeatedRestorePreserveSharedStateSemantics()
        {
            var text = new FastStringBuilder("A\u064eB", 32);
            TashkeelFixer.RemoveTashkeel(text);
            Assert.That(text.ToString(), Is.EqualTo("AB"));
            TashkeelFixer.RestoreTashkeel(text);
            Assert.That(text.ToString(), Is.EqualTo("A\u064eB"));
            TashkeelFixer.RestoreTashkeel(text);
            Assert.That(text.ToString(), Is.EqualTo("A\u064e\u064eB"));
            // A later ordinary removal establishes the next operation's locations.
            var next = new FastStringBuilder("XY", 32);
            TashkeelFixer.RemoveTashkeel(next);
            TashkeelFixer.RestoreTashkeel(next);
            Assert.That(next.ToString(), Is.EqualTo("XY"));
            var location = new TashkeelLocation((TashkeelCharacters)0x1234, -3);
            Assert.That(location.Tashkeel, Is.EqualTo('\u1234'));
            Assert.That(location.Position, Is.EqualTo(-3));
        }

        [Test]
        public void ShaddaCombinesOnlyItsOriginalFollowingMarks()
        {
            var marks = new FastStringBuilder("\u0651\u064c\u0651\u064d\u0651\u064e\u0651\u064f\u0651\u0650\u0651\u0670", 32);
            TashkeelFixer.FixShaddaCombinations(marks);
            Assert.That(marks.ToString(), Is.EqualTo("\ufc5e\ufc5f\ufc60\ufc61\ufc62\ufc63"));
            var reversed = new FastStringBuilder("\u064e\u0651", 32);
            TashkeelFixer.FixShaddaCombinations(reversed);
            Assert.That(reversed.ToString(), Is.EqualTo("\u064e\u0651"));
        }

        [Test]
        public void LigatureReversalAppendsAndPreservesTheOriginalScalarQuirk()
        {
            var output = new FastStringBuilder("!", 64);
            LigatureFixer.Fix(new FastStringBuilder("AB12", 64), output, false, true, true);
            Assert.That(output.ToString(), Is.EqualTo("!AB12"));
            output.Clear();
            LigatureFixer.Fix(new FastStringBuilder("\u05d0(\u05d1", 64), output, false, false, true);
            Assert.That(output.ToString(), Is.EqualTo("\u05d1)\u05d0"));
            output.Clear();
            LigatureFixer.Fix(new FastStringBuilder("A\u200c\uffffB", 64), output, false, false, true);
            Assert.That(output.ToString(), Is.EqualTo("BA"));
            output.Clear();
            LigatureFixer.Fix(new FastStringBuilder("A\U0001f600B", 64), output, false, false, true);
            Assert.That(output.ToString(), Is.EqualTo("B\U0001f600A"));
        }

        [Test]
        public void WholePipelineDefaultsAndSuccessiveCallsUseGenuineSharedBuffers()
        {
            var output = new FastStringBuilder(2048);
            RTLSupport.FixRTL("12", output);
            Assert.That(output.ToString(), Is.EqualTo("\u06f1\u06f2"));
            output.Clear();
            RTLSupport.FixRTL("12", output, false, false, false);
            Assert.That(output.ToString(), Is.EqualTo("\u0661\u0662"));
            output.Clear();
            RTLSupport.FixRTL("\u0644\u0627", output, false, false, true);
            Assert.That(output.ToString(), Is.EqualTo("\ufefb"));
            output.Clear();
            RTLSupport.FixRTL("\u05d0\u05d1", output, false, false, true);
            Assert.That(output.ToString(), Is.EqualTo("\u05d1\u05d0"));
            output.Clear();
            RTLSupport.FixRTL("AB12", output, false, false, true);
            Assert.That(output.ToString(), Is.EqualTo("AB12"));
            output.Clear();
            RTLSupport.FixRTL("", output, false, false, true);
            Assert.That(output.Length, Is.Zero);
        }

        private static void RequireGenuineScope(Type type)
        {
            if (type.IsByRef || type.IsArray) { RequireGenuineScope(type.GetElementType()); return; }
            string name = type.IsGenericType ? type.GetGenericTypeDefinition().FullName : type.FullName;
            string expected = name.StartsWith("RTLTMPro.", StringComparison.Ordinal) ? "HLRTLTextMeshPro.Runtime"
                : name.StartsWith("TMPro.", StringComparison.Ordinal) ? "Unity.TextMeshPro"
                : name.StartsWith("UnityEngine.", StringComparison.Ordinal) ? "UnityEngine.CoreModule"
                : name.StartsWith("Unity.IL2CPP.", StringComparison.Ordinal) ? "HLUnityCore.Runtime"
                : name == "System.Collections.Generic.HashSet`1" ? "System.Core"
                : name.StartsWith("System.", StringComparison.Ordinal) ? "mscorlib" : null;
            Assert.That(expected, Is.Not.Null, name);
            Assert.That(type.Assembly.GetName().Name, Is.EqualTo(expected), name);
            if (type.IsGenericType) foreach (Type argument in type.GetGenericArguments()) RequireGenuineScope(argument);
        }

        private static string AttributeLine(string scope, CustomAttributeData attribute)
        {
            Assert.That(attribute.NamedArguments.Count, Is.Zero);
            return scope + "|" + attribute.AttributeType.Assembly.GetName().Name + ":" + attribute.AttributeType.FullName + "("
                + string.Join(",", attribute.ConstructorArguments.Select(arg => TypeName(arg.ArgumentType) + "="
                    + Convert.ToString(arg.Value, System.Globalization.CultureInfo.InvariantCulture))) + ")";
        }

        private static string TypeName(Type type)
        {
            if (type.IsByRef) return TypeName(type.GetElementType());
            if (type.IsArray) return TypeName(type.GetElementType()) + "[]";
            if (!type.IsGenericType) return type.FullName;
            return type.GetGenericTypeDefinition().FullName + "<" + string.Join(",", type.GetGenericArguments().Select(TypeName)) + ">";
        }

        private static string Signature(MethodBase method)
        {
            if (method is MethodInfo declaredMethod) RequireGenuineScope(declaredMethod.ReturnType);
            foreach (ParameterInfo parameter in method.GetParameters()) RequireGenuineScope(parameter.ParameterType);
            string result = method is MethodInfo info ? TypeName(info.ReturnType) : "System.Void";
            return result + " " + method.Name + "(" + string.Join(", ", method.GetParameters().Select(p =>
                (p.ParameterType.IsByRef ? (p.IsOut ? "out " : "ref ") : "") + TypeName(p.ParameterType) + " " + p.Name
                    + (p.IsOptional ? " = " + Convert.ToString(p.DefaultValue, System.Globalization.CultureInfo.InvariantCulture) : ""))) + ")";
        }

        // Original physical declaration rows, never fixed current tokens or image hashes.
        private static readonly string[] TypeDeclarations =
        {
            "RTLTMPro.Char32Utils|1048961|System.Object",
            "RTLTMPro.FastStringBuilder|1048577|System.Object",
            "RTLTMPro.GlyphFixer|1048961|System.Object",
            "RTLTMPro.GlyphTable|385|System.Object",
            "RTLTMPro.LigatureFixer|1048961|System.Object",
            "RTLTMPro.RichTextFixer+TagType|258|System.Enum",
            "RTLTMPro.RichTextFixer+Tag|1048842|System.ValueType",
            "RTLTMPro.RichTextFixer|1048961|System.Object",
            "RTLTMPro.RTLSupport|385|System.Object",
            "RTLTMPro.RTLTextMeshPro|1048833|TMPro.TextMeshProUGUI",
            "RTLTMPro.RTLTextMeshPro3D|1048577|TMPro.TextMeshPro",
            "RTLTMPro.TashkeelFixer|1048961|System.Object",
            "RTLTMPro.TashkeelLocation|1048841|System.ValueType",
            "RTLTMPro.TextUtils|1048961|System.Object",
            "RTLTMPro.ArabicGeneralLetters|257|System.Enum",
            "RTLTMPro.ArabicIsolatedLetters|256|System.Enum",
            "RTLTMPro.EnglishNumbers|257|System.Enum",
            "RTLTMPro.FarsiNumbers|257|System.Enum",
            "RTLTMPro.HinduNumbers|257|System.Enum",
            "RTLTMPro.SpecialCharacters|257|System.Enum",
            "RTLTMPro.TashkeelCharacters|257|System.Enum",
        };
        private static readonly string[] FieldDeclarations =
        {
            "RTLTMPro.FastStringBuilder|length|System.Int32|1|",
            "RTLTMPro.FastStringBuilder|array|System.Int32[]|1|",
            "RTLTMPro.FastStringBuilder|capacity|System.Int32|1|",
            "RTLTMPro.GlyphFixer|EnglishToFarsiNumberMap|System.Collections.Generic.Dictionary`2<System.Char,System.Char>|22|",
            "RTLTMPro.GlyphFixer|EnglishToHinduNumberMap|System.Collections.Generic.Dictionary`2<System.Char,System.Char>|22|",
            "RTLTMPro.GlyphTable|MapList|System.Collections.Generic.Dictionary`2<System.Char,System.Char>|49|",
            "RTLTMPro.LigatureFixer|LtrTextHolder|System.Collections.Generic.List`1<System.Int32>|49|",
            "RTLTMPro.LigatureFixer|TagTextHolder|System.Collections.Generic.List`1<System.Int32>|49|",
            "RTLTMPro.LigatureFixer|MirroredCharsMap|System.Collections.Generic.Dictionary`2<System.Char,System.Char>|49|",
            "RTLTMPro.LigatureFixer|MirroredCharsSet|System.Collections.Generic.HashSet`1<System.Char>|49|",
            "RTLTMPro.RichTextFixer+TagType|value__|System.Int32|1542|",
            "RTLTMPro.RichTextFixer+TagType|None|RTLTMPro.RichTextFixer+TagType|32854|0",
            "RTLTMPro.RichTextFixer+TagType|Opening|RTLTMPro.RichTextFixer+TagType|32854|1",
            "RTLTMPro.RichTextFixer+TagType|Closing|RTLTMPro.RichTextFixer+TagType|32854|2",
            "RTLTMPro.RichTextFixer+TagType|SelfContained|RTLTMPro.RichTextFixer+TagType|32854|3",
            "RTLTMPro.RichTextFixer+Tag|Start|System.Int32|6|",
            "RTLTMPro.RichTextFixer+Tag|End|System.Int32|6|",
            "RTLTMPro.RichTextFixer+Tag|HashCode|System.Int32|6|",
            "RTLTMPro.RichTextFixer+Tag|Type|RTLTMPro.RichTextFixer+TagType|6|",
            "RTLTMPro.RTLSupport|DefaultBufferSize|System.Int32|32854|2048",
            "RTLTMPro.RTLSupport|inputBuilder|RTLTMPro.FastStringBuilder|17|",
            "RTLTMPro.RTLSupport|glyphFixerOutput|RTLTMPro.FastStringBuilder|17|",
            "RTLTMPro.RTLTextMeshPro|preserveNumbers|System.Boolean|1|",
            "RTLTMPro.RTLTextMeshPro|farsi|System.Boolean|1|",
            "RTLTMPro.RTLTextMeshPro|originalText|System.String|1|",
            "RTLTMPro.RTLTextMeshPro|fixTags|System.Boolean|1|",
            "RTLTMPro.RTLTextMeshPro|forceFix|System.Boolean|1|",
            "RTLTMPro.RTLTextMeshPro|m_flipLeftAlignmentToRight|System.Boolean|1|",
            "RTLTMPro.RTLTextMeshPro|m_inverseGeometrySortingOrder|System.Boolean|1|",
            "RTLTMPro.RTLTextMeshPro|m_serialisedAlignment|System.Int32|1|",
            "RTLTMPro.RTLTextMeshPro|finalText|RTLTMPro.FastStringBuilder|33|",
            "RTLTMPro.RTLTextMeshPro3D|preserveNumbers|System.Boolean|4|",
            "RTLTMPro.RTLTextMeshPro3D|farsi|System.Boolean|4|",
            "RTLTMPro.RTLTextMeshPro3D|originalText|System.String|4|",
            "RTLTMPro.RTLTextMeshPro3D|fixTags|System.Boolean|4|",
            "RTLTMPro.RTLTextMeshPro3D|forceFix|System.Boolean|4|",
            "RTLTMPro.RTLTextMeshPro3D|finalText|RTLTMPro.FastStringBuilder|36|",
            "RTLTMPro.TashkeelFixer|TashkeelLocations|System.Collections.Generic.List`1<RTLTMPro.TashkeelLocation>|49|",
            "RTLTMPro.TashkeelFixer|ShaddaDammatan|System.String|49|",
            "RTLTMPro.TashkeelFixer|ShaddaKasratan|System.String|49|",
            "RTLTMPro.TashkeelFixer|ShaddaSuperscriptAlef|System.String|49|",
            "RTLTMPro.TashkeelFixer|ShaddaFatha|System.String|49|",
            "RTLTMPro.TashkeelFixer|ShaddaDamma|System.String|49|",
            "RTLTMPro.TashkeelFixer|ShaddaKasra|System.String|49|",
            "RTLTMPro.TashkeelFixer|ShaddaWithFathaIsolatedForm|System.String|49|",
            "RTLTMPro.TashkeelFixer|ShaddaWithDammaIsolatedForm|System.String|49|",
            "RTLTMPro.TashkeelFixer|ShaddaWithKasraIsolatedForm|System.String|49|",
            "RTLTMPro.TashkeelFixer|ShaddaWithDammatanIsolatedForm|System.String|49|",
            "RTLTMPro.TashkeelFixer|ShaddaWithKasratanIsolatedForm|System.String|49|",
            "RTLTMPro.TashkeelFixer|ShaddaWithSuperscriptAlefIsolatedForm|System.String|49|",
            "RTLTMPro.TashkeelFixer|TashkeelCharactersSet|System.Collections.Generic.HashSet`1<System.Char>|49|",
            "RTLTMPro.TashkeelFixer|ShaddaCombinationMap|System.Collections.Generic.Dictionary`2<System.Char,System.Char>|49|",
            "RTLTMPro.TashkeelLocation|<Tashkeel>k__BackingField|System.Char|1|",
            "RTLTMPro.TashkeelLocation|<Position>k__BackingField|System.Int32|1|",
            "RTLTMPro.TextUtils|LowerCaseA|System.Char|32849|a",
            "RTLTMPro.TextUtils|UpperCaseA|System.Char|32849|A",
            "RTLTMPro.TextUtils|LowerCaseZ|System.Char|32849|z",
            "RTLTMPro.TextUtils|UpperCaseZ|System.Char|32849|Z",
            "RTLTMPro.TextUtils|HebrewLow|System.Char|32849|\u0591",
            "RTLTMPro.TextUtils|HebrewHigh|System.Char|32849|\u05f4",
            "RTLTMPro.TextUtils|ArabicBaseBlockLow|System.Char|32849|\u0600",
            "RTLTMPro.TextUtils|ArabicBaseBlockHigh|System.Char|32849|\u06ff",
            "RTLTMPro.TextUtils|ArabicExtendedABlockLow|System.Char|32849|\u08a0",
            "RTLTMPro.TextUtils|ArabicExtendedABlockHigh|System.Char|32849|\u08ff",
            "RTLTMPro.TextUtils|ArabicExtendedBBlockLow|System.Char|32849|\u0870",
            "RTLTMPro.TextUtils|ArabicExtendedBBlockHigh|System.Char|32849|\u089f",
            "RTLTMPro.TextUtils|ArabicPresentationFormsABlockLow|System.Char|32849|\ufb50",
            "RTLTMPro.TextUtils|ArabicPresentationFormsABlockHigh|System.Char|32849|\ufdff",
            "RTLTMPro.TextUtils|ArabicPresentationFormsBBlockLow|System.Char|32849|\ufe70",
            "RTLTMPro.TextUtils|ArabicPresentationFormsBBlockHigh|System.Char|32849|\ufeff",
            "RTLTMPro.ArabicGeneralLetters|value__|System.Int32|1542|",
            "RTLTMPro.ArabicGeneralLetters|Hamza|RTLTMPro.ArabicGeneralLetters|32854|1569",
            "RTLTMPro.ArabicGeneralLetters|AlefMaddaAbove|RTLTMPro.ArabicGeneralLetters|32854|1570",
            "RTLTMPro.ArabicGeneralLetters|AlefHamzaAbove|RTLTMPro.ArabicGeneralLetters|32854|1571",
            "RTLTMPro.ArabicGeneralLetters|WawHamzaAbove|RTLTMPro.ArabicGeneralLetters|32854|1572",
            "RTLTMPro.ArabicGeneralLetters|AlefHamzaBelow|RTLTMPro.ArabicGeneralLetters|32854|1573",
            "RTLTMPro.ArabicGeneralLetters|YehHamzaAbove|RTLTMPro.ArabicGeneralLetters|32854|1574",
            "RTLTMPro.ArabicGeneralLetters|Alef|RTLTMPro.ArabicGeneralLetters|32854|1575",
            "RTLTMPro.ArabicGeneralLetters|Beh|RTLTMPro.ArabicGeneralLetters|32854|1576",
            "RTLTMPro.ArabicGeneralLetters|TehMarbuta|RTLTMPro.ArabicGeneralLetters|32854|1577",
            "RTLTMPro.ArabicGeneralLetters|Teh|RTLTMPro.ArabicGeneralLetters|32854|1578",
            "RTLTMPro.ArabicGeneralLetters|Theh|RTLTMPro.ArabicGeneralLetters|32854|1579",
            "RTLTMPro.ArabicGeneralLetters|Jeem|RTLTMPro.ArabicGeneralLetters|32854|1580",
            "RTLTMPro.ArabicGeneralLetters|Hah|RTLTMPro.ArabicGeneralLetters|32854|1581",
            "RTLTMPro.ArabicGeneralLetters|Khah|RTLTMPro.ArabicGeneralLetters|32854|1582",
            "RTLTMPro.ArabicGeneralLetters|Dal|RTLTMPro.ArabicGeneralLetters|32854|1583",
            "RTLTMPro.ArabicGeneralLetters|Thal|RTLTMPro.ArabicGeneralLetters|32854|1584",
            "RTLTMPro.ArabicGeneralLetters|Reh|RTLTMPro.ArabicGeneralLetters|32854|1585",
            "RTLTMPro.ArabicGeneralLetters|Zain|RTLTMPro.ArabicGeneralLetters|32854|1586",
            "RTLTMPro.ArabicGeneralLetters|Seen|RTLTMPro.ArabicGeneralLetters|32854|1587",
            "RTLTMPro.ArabicGeneralLetters|Sheen|RTLTMPro.ArabicGeneralLetters|32854|1588",
            "RTLTMPro.ArabicGeneralLetters|Sad|RTLTMPro.ArabicGeneralLetters|32854|1589",
            "RTLTMPro.ArabicGeneralLetters|Dad|RTLTMPro.ArabicGeneralLetters|32854|1590",
            "RTLTMPro.ArabicGeneralLetters|Tah|RTLTMPro.ArabicGeneralLetters|32854|1591",
            "RTLTMPro.ArabicGeneralLetters|Zah|RTLTMPro.ArabicGeneralLetters|32854|1592",
            "RTLTMPro.ArabicGeneralLetters|Ain|RTLTMPro.ArabicGeneralLetters|32854|1593",
            "RTLTMPro.ArabicGeneralLetters|Ghain|RTLTMPro.ArabicGeneralLetters|32854|1594",
            "RTLTMPro.ArabicGeneralLetters|Feh|RTLTMPro.ArabicGeneralLetters|32854|1601",
            "RTLTMPro.ArabicGeneralLetters|Qaf|RTLTMPro.ArabicGeneralLetters|32854|1602",
            "RTLTMPro.ArabicGeneralLetters|Kaf|RTLTMPro.ArabicGeneralLetters|32854|1603",
            "RTLTMPro.ArabicGeneralLetters|Lam|RTLTMPro.ArabicGeneralLetters|32854|1604",
            "RTLTMPro.ArabicGeneralLetters|Meem|RTLTMPro.ArabicGeneralLetters|32854|1605",
            "RTLTMPro.ArabicGeneralLetters|Noon|RTLTMPro.ArabicGeneralLetters|32854|1606",
            "RTLTMPro.ArabicGeneralLetters|Heh|RTLTMPro.ArabicGeneralLetters|32854|1607",
            "RTLTMPro.ArabicGeneralLetters|Waw|RTLTMPro.ArabicGeneralLetters|32854|1608",
            "RTLTMPro.ArabicGeneralLetters|AlefMaksura|RTLTMPro.ArabicGeneralLetters|32854|1609",
            "RTLTMPro.ArabicGeneralLetters|Yeh|RTLTMPro.ArabicGeneralLetters|32854|1610",
            "RTLTMPro.ArabicGeneralLetters|FarsiYeh|RTLTMPro.ArabicGeneralLetters|32854|1740",
            "RTLTMPro.ArabicGeneralLetters|Peh|RTLTMPro.ArabicGeneralLetters|32854|1662",
            "RTLTMPro.ArabicGeneralLetters|TCheh|RTLTMPro.ArabicGeneralLetters|32854|1670",
            "RTLTMPro.ArabicGeneralLetters|Jeh|RTLTMPro.ArabicGeneralLetters|32854|1688",
            "RTLTMPro.ArabicGeneralLetters|Keheh|RTLTMPro.ArabicGeneralLetters|32854|1705",
            "RTLTMPro.ArabicGeneralLetters|Gaf|RTLTMPro.ArabicGeneralLetters|32854|1711",
            "RTLTMPro.ArabicGeneralLetters|Tatweel|RTLTMPro.ArabicGeneralLetters|32854|1600",
            "RTLTMPro.ArabicIsolatedLetters|value__|System.Int32|1542|",
            "RTLTMPro.ArabicIsolatedLetters|Hamza|RTLTMPro.ArabicIsolatedLetters|32854|65152",
            "RTLTMPro.ArabicIsolatedLetters|AlefMaddaAbove|RTLTMPro.ArabicIsolatedLetters|32854|65153",
            "RTLTMPro.ArabicIsolatedLetters|AlefHamzaAbove|RTLTMPro.ArabicIsolatedLetters|32854|65155",
            "RTLTMPro.ArabicIsolatedLetters|WawHamzaAbove|RTLTMPro.ArabicIsolatedLetters|32854|65157",
            "RTLTMPro.ArabicIsolatedLetters|AlefHamzaBelow|RTLTMPro.ArabicIsolatedLetters|32854|65159",
            "RTLTMPro.ArabicIsolatedLetters|YehHamzaAbove|RTLTMPro.ArabicIsolatedLetters|32854|65161",
            "RTLTMPro.ArabicIsolatedLetters|Alef|RTLTMPro.ArabicIsolatedLetters|32854|65165",
            "RTLTMPro.ArabicIsolatedLetters|Beh|RTLTMPro.ArabicIsolatedLetters|32854|65167",
            "RTLTMPro.ArabicIsolatedLetters|TehMarbuta|RTLTMPro.ArabicIsolatedLetters|32854|65171",
            "RTLTMPro.ArabicIsolatedLetters|Teh|RTLTMPro.ArabicIsolatedLetters|32854|65173",
            "RTLTMPro.ArabicIsolatedLetters|Theh|RTLTMPro.ArabicIsolatedLetters|32854|65177",
            "RTLTMPro.ArabicIsolatedLetters|Jeem|RTLTMPro.ArabicIsolatedLetters|32854|65181",
            "RTLTMPro.ArabicIsolatedLetters|Hah|RTLTMPro.ArabicIsolatedLetters|32854|65185",
            "RTLTMPro.ArabicIsolatedLetters|Khah|RTLTMPro.ArabicIsolatedLetters|32854|65189",
            "RTLTMPro.ArabicIsolatedLetters|Dal|RTLTMPro.ArabicIsolatedLetters|32854|65193",
            "RTLTMPro.ArabicIsolatedLetters|Thal|RTLTMPro.ArabicIsolatedLetters|32854|65195",
            "RTLTMPro.ArabicIsolatedLetters|Reh|RTLTMPro.ArabicIsolatedLetters|32854|65197",
            "RTLTMPro.ArabicIsolatedLetters|Zain|RTLTMPro.ArabicIsolatedLetters|32854|65199",
            "RTLTMPro.ArabicIsolatedLetters|Seen|RTLTMPro.ArabicIsolatedLetters|32854|65201",
            "RTLTMPro.ArabicIsolatedLetters|Sheen|RTLTMPro.ArabicIsolatedLetters|32854|65205",
            "RTLTMPro.ArabicIsolatedLetters|Sad|RTLTMPro.ArabicIsolatedLetters|32854|65209",
            "RTLTMPro.ArabicIsolatedLetters|Dad|RTLTMPro.ArabicIsolatedLetters|32854|65213",
            "RTLTMPro.ArabicIsolatedLetters|Tah|RTLTMPro.ArabicIsolatedLetters|32854|65217",
            "RTLTMPro.ArabicIsolatedLetters|Zah|RTLTMPro.ArabicIsolatedLetters|32854|65221",
            "RTLTMPro.ArabicIsolatedLetters|Ain|RTLTMPro.ArabicIsolatedLetters|32854|65225",
            "RTLTMPro.ArabicIsolatedLetters|Ghain|RTLTMPro.ArabicIsolatedLetters|32854|65229",
            "RTLTMPro.ArabicIsolatedLetters|Feh|RTLTMPro.ArabicIsolatedLetters|32854|65233",
            "RTLTMPro.ArabicIsolatedLetters|Qaf|RTLTMPro.ArabicIsolatedLetters|32854|65237",
            "RTLTMPro.ArabicIsolatedLetters|Kaf|RTLTMPro.ArabicIsolatedLetters|32854|65241",
            "RTLTMPro.ArabicIsolatedLetters|Lam|RTLTMPro.ArabicIsolatedLetters|32854|65245",
            "RTLTMPro.ArabicIsolatedLetters|Meem|RTLTMPro.ArabicIsolatedLetters|32854|65249",
            "RTLTMPro.ArabicIsolatedLetters|Noon|RTLTMPro.ArabicIsolatedLetters|32854|65253",
            "RTLTMPro.ArabicIsolatedLetters|Heh|RTLTMPro.ArabicIsolatedLetters|32854|65257",
            "RTLTMPro.ArabicIsolatedLetters|Waw|RTLTMPro.ArabicIsolatedLetters|32854|65261",
            "RTLTMPro.ArabicIsolatedLetters|AlefMaksura|RTLTMPro.ArabicIsolatedLetters|32854|65263",
            "RTLTMPro.ArabicIsolatedLetters|Yeh|RTLTMPro.ArabicIsolatedLetters|32854|65265",
            "RTLTMPro.ArabicIsolatedLetters|FarsiYeh|RTLTMPro.ArabicIsolatedLetters|32854|64508",
            "RTLTMPro.ArabicIsolatedLetters|Peh|RTLTMPro.ArabicIsolatedLetters|32854|64342",
            "RTLTMPro.ArabicIsolatedLetters|TCheh|RTLTMPro.ArabicIsolatedLetters|32854|64378",
            "RTLTMPro.ArabicIsolatedLetters|Jeh|RTLTMPro.ArabicIsolatedLetters|32854|64394",
            "RTLTMPro.ArabicIsolatedLetters|Keheh|RTLTMPro.ArabicIsolatedLetters|32854|64398",
            "RTLTMPro.ArabicIsolatedLetters|Gaf|RTLTMPro.ArabicIsolatedLetters|32854|64402",
            "RTLTMPro.EnglishNumbers|value__|System.Int32|1542|",
            "RTLTMPro.EnglishNumbers|Zero|RTLTMPro.EnglishNumbers|32854|48",
            "RTLTMPro.EnglishNumbers|One|RTLTMPro.EnglishNumbers|32854|49",
            "RTLTMPro.EnglishNumbers|Two|RTLTMPro.EnglishNumbers|32854|50",
            "RTLTMPro.EnglishNumbers|Three|RTLTMPro.EnglishNumbers|32854|51",
            "RTLTMPro.EnglishNumbers|Four|RTLTMPro.EnglishNumbers|32854|52",
            "RTLTMPro.EnglishNumbers|Five|RTLTMPro.EnglishNumbers|32854|53",
            "RTLTMPro.EnglishNumbers|Six|RTLTMPro.EnglishNumbers|32854|54",
            "RTLTMPro.EnglishNumbers|Seven|RTLTMPro.EnglishNumbers|32854|55",
            "RTLTMPro.EnglishNumbers|Eight|RTLTMPro.EnglishNumbers|32854|56",
            "RTLTMPro.EnglishNumbers|Nine|RTLTMPro.EnglishNumbers|32854|57",
            "RTLTMPro.FarsiNumbers|value__|System.Int32|1542|",
            "RTLTMPro.FarsiNumbers|Zero|RTLTMPro.FarsiNumbers|32854|1776",
            "RTLTMPro.FarsiNumbers|One|RTLTMPro.FarsiNumbers|32854|1777",
            "RTLTMPro.FarsiNumbers|Two|RTLTMPro.FarsiNumbers|32854|1778",
            "RTLTMPro.FarsiNumbers|Three|RTLTMPro.FarsiNumbers|32854|1779",
            "RTLTMPro.FarsiNumbers|Four|RTLTMPro.FarsiNumbers|32854|1780",
            "RTLTMPro.FarsiNumbers|Five|RTLTMPro.FarsiNumbers|32854|1781",
            "RTLTMPro.FarsiNumbers|Six|RTLTMPro.FarsiNumbers|32854|1782",
            "RTLTMPro.FarsiNumbers|Seven|RTLTMPro.FarsiNumbers|32854|1783",
            "RTLTMPro.FarsiNumbers|Eight|RTLTMPro.FarsiNumbers|32854|1784",
            "RTLTMPro.FarsiNumbers|Nine|RTLTMPro.FarsiNumbers|32854|1785",
            "RTLTMPro.HinduNumbers|value__|System.Int32|1542|",
            "RTLTMPro.HinduNumbers|Zero|RTLTMPro.HinduNumbers|32854|1632",
            "RTLTMPro.HinduNumbers|One|RTLTMPro.HinduNumbers|32854|1633",
            "RTLTMPro.HinduNumbers|Two|RTLTMPro.HinduNumbers|32854|1634",
            "RTLTMPro.HinduNumbers|Three|RTLTMPro.HinduNumbers|32854|1635",
            "RTLTMPro.HinduNumbers|Four|RTLTMPro.HinduNumbers|32854|1636",
            "RTLTMPro.HinduNumbers|Five|RTLTMPro.HinduNumbers|32854|1637",
            "RTLTMPro.HinduNumbers|Six|RTLTMPro.HinduNumbers|32854|1638",
            "RTLTMPro.HinduNumbers|Seven|RTLTMPro.HinduNumbers|32854|1639",
            "RTLTMPro.HinduNumbers|Eight|RTLTMPro.HinduNumbers|32854|1640",
            "RTLTMPro.HinduNumbers|Nine|RTLTMPro.HinduNumbers|32854|1641",
            "RTLTMPro.SpecialCharacters|value__|System.Int32|1542|",
            "RTLTMPro.SpecialCharacters|ZeroWidthNoJoiner|RTLTMPro.SpecialCharacters|32854|8204",
            "RTLTMPro.TashkeelCharacters|value__|System.Int32|1542|",
            "RTLTMPro.TashkeelCharacters|Fathan|RTLTMPro.TashkeelCharacters|32854|1611",
            "RTLTMPro.TashkeelCharacters|Dammatan|RTLTMPro.TashkeelCharacters|32854|1612",
            "RTLTMPro.TashkeelCharacters|Kasratan|RTLTMPro.TashkeelCharacters|32854|1613",
            "RTLTMPro.TashkeelCharacters|Fatha|RTLTMPro.TashkeelCharacters|32854|1614",
            "RTLTMPro.TashkeelCharacters|Damma|RTLTMPro.TashkeelCharacters|32854|1615",
            "RTLTMPro.TashkeelCharacters|Kasra|RTLTMPro.TashkeelCharacters|32854|1616",
            "RTLTMPro.TashkeelCharacters|Shadda|RTLTMPro.TashkeelCharacters|32854|1617",
            "RTLTMPro.TashkeelCharacters|Sukun|RTLTMPro.TashkeelCharacters|32854|1618",
            "RTLTMPro.TashkeelCharacters|MaddahAbove|RTLTMPro.TashkeelCharacters|32854|1619",
            "RTLTMPro.TashkeelCharacters|SuperscriptAlef|RTLTMPro.TashkeelCharacters|32854|1648",
            "RTLTMPro.TashkeelCharacters|ShaddaWithDammatanIsolatedForm|RTLTMPro.TashkeelCharacters|32854|64606",
            "RTLTMPro.TashkeelCharacters|ShaddaWithKasratanIsolatedForm|RTLTMPro.TashkeelCharacters|32854|64607",
            "RTLTMPro.TashkeelCharacters|ShaddaWithFathaIsolatedForm|RTLTMPro.TashkeelCharacters|32854|64608",
            "RTLTMPro.TashkeelCharacters|ShaddaWithDammaIsolatedForm|RTLTMPro.TashkeelCharacters|32854|64609",
            "RTLTMPro.TashkeelCharacters|ShaddaWithKasraIsolatedForm|RTLTMPro.TashkeelCharacters|32854|64610",
            "RTLTMPro.TashkeelCharacters|ShaddaWithSuperscriptAlefIsolatedForm|RTLTMPro.TashkeelCharacters|32854|64611",
        };
        private static readonly string[] MethodDeclarations =
        {
            "RTLTMPro.Char32Utils|System.Boolean IsUnicode16Char(System.Int32 ch)|150|0",
            "RTLTMPro.Char32Utils|System.Boolean IsRTLCharacter(System.Int32 ch)|150|0",
            "RTLTMPro.Char32Utils|System.Boolean IsEnglishLetter(System.Int32 ch)|150|0",
            "RTLTMPro.Char32Utils|System.Boolean IsNumber(System.Int32 ch, System.Boolean preserveNumbers, System.Boolean farsi)|150|0",
            "RTLTMPro.Char32Utils|System.Boolean IsSymbol(System.Int32 ch)|150|0",
            "RTLTMPro.Char32Utils|System.Boolean IsLetter(System.Int32 ch)|150|0",
            "RTLTMPro.Char32Utils|System.Boolean IsPunctuation(System.Int32 ch)|150|0",
            "RTLTMPro.Char32Utils|System.Boolean IsWhiteSpace(System.Int32 ch)|150|0",
            "RTLTMPro.FastStringBuilder|System.Int32 get_Length()|2182|0",
            "RTLTMPro.FastStringBuilder|System.Void set_Length(System.Int32 value)|2182|0",
            "RTLTMPro.FastStringBuilder|System.Void .ctor(System.Int32 capacity)|6278|0",
            "RTLTMPro.FastStringBuilder|System.Void .ctor(System.String text)|6278|0",
            "RTLTMPro.FastStringBuilder|System.Void .ctor(System.String text, System.Int32 capacity)|6278|0",
            "RTLTMPro.FastStringBuilder|System.String op_Implicit(RTLTMPro.FastStringBuilder x)|2198|0",
            "RTLTMPro.FastStringBuilder|RTLTMPro.FastStringBuilder op_Implicit(System.String x)|2198|0",
            "RTLTMPro.FastStringBuilder|System.Int32 Get(System.Int32 index)|134|256",
            "RTLTMPro.FastStringBuilder|System.Void Set(System.Int32 index, System.Int32 ch)|134|256",
            "RTLTMPro.FastStringBuilder|System.Void SetValue(System.String text)|134|0",
            "RTLTMPro.FastStringBuilder|System.Void SetValue(RTLTMPro.FastStringBuilder other)|134|0",
            "RTLTMPro.FastStringBuilder|System.Void Append(System.Int32 ch)|134|0",
            "RTLTMPro.FastStringBuilder|System.Void Append(System.Char ch)|134|0",
            "RTLTMPro.FastStringBuilder|System.Void Insert(System.Int32 pos, RTLTMPro.FastStringBuilder str, System.Int32 offset, System.Int32 count)|134|0",
            "RTLTMPro.FastStringBuilder|System.Void Insert(System.Int32 pos, RTLTMPro.FastStringBuilder str)|134|0",
            "RTLTMPro.FastStringBuilder|System.Void Insert(System.Int32 pos, System.Int32 ch)|134|0",
            "RTLTMPro.FastStringBuilder|System.Void RemoveAll(System.Int32 character)|134|0",
            "RTLTMPro.FastStringBuilder|System.Void Remove(System.Int32 start, System.Int32 length)|134|0",
            "RTLTMPro.FastStringBuilder|System.Void Reverse(System.Int32 startIndex, System.Int32 length)|134|0",
            "RTLTMPro.FastStringBuilder|System.Void Reverse()|134|0",
            "RTLTMPro.FastStringBuilder|System.Void Substring(RTLTMPro.FastStringBuilder output, System.Int32 start, System.Int32 length)|134|0",
            "RTLTMPro.FastStringBuilder|System.String ToString()|198|0",
            "RTLTMPro.FastStringBuilder|System.String ToDebugString()|134|0",
            "RTLTMPro.FastStringBuilder|System.Void Replace(System.Int32 oldChar, System.Int32 newChar)|134|0",
            "RTLTMPro.FastStringBuilder|System.Void Replace(RTLTMPro.FastStringBuilder oldStr, RTLTMPro.FastStringBuilder newStr)|134|0",
            "RTLTMPro.FastStringBuilder|System.Void Clear()|134|0",
            "RTLTMPro.FastStringBuilder|System.Void EnsureCapacity(System.Int32 cap, System.Boolean keepValues)|129|0",
            "RTLTMPro.FastStringBuilder|System.Void Copy(System.Int32[] src, System.Int32[] dst)|145|0",
            "RTLTMPro.GlyphFixer|System.Void Fix(RTLTMPro.FastStringBuilder input, RTLTMPro.FastStringBuilder output, System.Boolean preserveNumbers, System.Boolean farsi, System.Boolean fixTextTags)|150|0",
            "RTLTMPro.GlyphFixer|System.Void FixYah(RTLTMPro.FastStringBuilder text, System.Boolean farsi)|150|0",
            "RTLTMPro.GlyphFixer|System.Boolean HandleSpecialLam(RTLTMPro.FastStringBuilder input, RTLTMPro.FastStringBuilder output, System.Int32 i)|145|0",
            "RTLTMPro.GlyphFixer|System.Void FixNumbers(RTLTMPro.FastStringBuilder text, System.Boolean farsi)|150|0",
            "RTLTMPro.GlyphFixer|System.Void FixNumbersOutsideOfTags(RTLTMPro.FastStringBuilder text, System.Boolean farsi)|150|0",
            "RTLTMPro.GlyphFixer|System.Boolean IsLeadingLetter(RTLTMPro.FastStringBuilder letters, System.Int32 index)|145|0",
            "RTLTMPro.GlyphFixer|System.Boolean IsFinishingLetter(RTLTMPro.FastStringBuilder letters, System.Int32 index)|145|0",
            "RTLTMPro.GlyphFixer|System.Boolean IsMiddleLetter(RTLTMPro.FastStringBuilder letters, System.Int32 index)|145|0",
            "RTLTMPro.GlyphFixer|System.Void .cctor()|6289|0",
            "RTLTMPro.GlyphTable|System.Void .cctor()|6289|0",
            "RTLTMPro.GlyphTable|System.Char Convert(System.Char toBeConverted)|150|0",
            "RTLTMPro.LigatureFixer|System.Void FlushBufferToOutput(System.Collections.Generic.List`1<System.Int32> buffer, RTLTMPro.FastStringBuilder output)|145|0",
            "RTLTMPro.LigatureFixer|System.Void Fix(RTLTMPro.FastStringBuilder input, RTLTMPro.FastStringBuilder output, System.Boolean farsi, System.Boolean fixTextTags, System.Boolean preserveNumbers)|150|0",
            "RTLTMPro.LigatureFixer|System.Void .cctor()|6289|0",
            "RTLTMPro.RichTextFixer+Tag|System.Void .ctor(System.Int32 start, System.Int32 end, RTLTMPro.RichTextFixer+TagType type, System.Int32 hashCode)|6278|0",
            "RTLTMPro.RichTextFixer|System.Void Fix(RTLTMPro.FastStringBuilder text)|150|0",
            "RTLTMPro.RichTextFixer|System.Void FindTag(RTLTMPro.FastStringBuilder str, System.Int32 start, out RTLTMPro.RichTextFixer+Tag tag)|150|0",
            "RTLTMPro.RTLSupport|System.Void .cctor()|6289|0",
            "RTLTMPro.RTLSupport|System.Void FixRTL(System.String input, RTLTMPro.FastStringBuilder output, System.Boolean farsi = True, System.Boolean fixTextTags = True, System.Boolean preserveNumbers = False)|150|0",
            "RTLTMPro.RTLTextMeshPro|System.String get_text()|2246|0",
            "RTLTMPro.RTLTextMeshPro|System.Void set_text(System.String value)|2246|0",
            "RTLTMPro.RTLTextMeshPro|System.String get_OriginalText()|2182|0",
            "RTLTMPro.RTLTextMeshPro|System.Boolean get_PreserveNumbers()|2182|0",
            "RTLTMPro.RTLTextMeshPro|System.Void set_PreserveNumbers(System.Boolean value)|2182|0",
            "RTLTMPro.RTLTextMeshPro|System.Boolean get_Farsi()|2182|0",
            "RTLTMPro.RTLTextMeshPro|System.Void set_Farsi(System.Boolean value)|2182|0",
            "RTLTMPro.RTLTextMeshPro|System.Boolean get_FixTags()|2182|0",
            "RTLTMPro.RTLTextMeshPro|System.Void set_FixTags(System.Boolean value)|2182|0",
            "RTLTMPro.RTLTextMeshPro|System.Boolean get_ForceFix()|2182|0",
            "RTLTMPro.RTLTextMeshPro|System.Void set_ForceFix(System.Boolean value)|2182|0",
            "RTLTMPro.RTLTextMeshPro|System.Void Awake()|196|0",
            "RTLTMPro.RTLTextMeshPro|System.Void Update()|129|0",
            "RTLTMPro.RTLTextMeshPro|System.Void UpdateText()|134|0",
            "RTLTMPro.RTLTextMeshPro|System.Void FlipAlignment(System.Boolean isRTL)|129|0",
            "RTLTMPro.RTLTextMeshPro|System.String GetFixedText(System.String input)|129|0",
            "RTLTMPro.RTLTextMeshPro|System.Void .ctor()|6278|0",
            "RTLTMPro.RTLTextMeshPro3D|System.String get_text()|2246|0",
            "RTLTMPro.RTLTextMeshPro3D|System.Void set_text(System.String value)|2246|0",
            "RTLTMPro.RTLTextMeshPro3D|System.String get_OriginalText()|2182|0",
            "RTLTMPro.RTLTextMeshPro3D|System.Boolean get_PreserveNumbers()|2182|0",
            "RTLTMPro.RTLTextMeshPro3D|System.Void set_PreserveNumbers(System.Boolean value)|2182|0",
            "RTLTMPro.RTLTextMeshPro3D|System.Boolean get_Farsi()|2182|0",
            "RTLTMPro.RTLTextMeshPro3D|System.Void set_Farsi(System.Boolean value)|2182|0",
            "RTLTMPro.RTLTextMeshPro3D|System.Boolean get_FixTags()|2182|0",
            "RTLTMPro.RTLTextMeshPro3D|System.Void set_FixTags(System.Boolean value)|2182|0",
            "RTLTMPro.RTLTextMeshPro3D|System.Boolean get_ForceFix()|2180|0",
            "RTLTMPro.RTLTextMeshPro3D|System.Void set_ForceFix(System.Boolean value)|2180|0",
            "RTLTMPro.RTLTextMeshPro3D|System.Void Update()|132|0",
            "RTLTMPro.RTLTextMeshPro3D|System.Void UpdateText()|134|0",
            "RTLTMPro.RTLTextMeshPro3D|System.String GetFixedText(System.String input)|129|0",
            "RTLTMPro.RTLTextMeshPro3D|System.Void .ctor()|6278|0",
            "RTLTMPro.TashkeelFixer|System.Void RemoveTashkeel(RTLTMPro.FastStringBuilder input)|150|0",
            "RTLTMPro.TashkeelFixer|System.Void RestoreTashkeel(RTLTMPro.FastStringBuilder letters)|150|0",
            "RTLTMPro.TashkeelFixer|System.Void FixShaddaCombinations(RTLTMPro.FastStringBuilder input)|150|0",
            "RTLTMPro.TashkeelFixer|System.Void .cctor()|6289|0",
            "RTLTMPro.TashkeelLocation|System.Char get_Tashkeel()|2182|0",
            "RTLTMPro.TashkeelLocation|System.Void set_Tashkeel(System.Char value)|2182|0",
            "RTLTMPro.TashkeelLocation|System.Int32 get_Position()|2182|0",
            "RTLTMPro.TashkeelLocation|System.Void set_Position(System.Int32 value)|2182|0",
            "RTLTMPro.TashkeelLocation|System.Void .ctor(RTLTMPro.TashkeelCharacters tashkeel, System.Int32 position)|6278|0",
            "RTLTMPro.TextUtils|System.Boolean IsPunctuation(System.Char ch)|150|0",
            "RTLTMPro.TextUtils|System.Boolean IsNumber(System.Char ch, System.Boolean preserveNumbers, System.Boolean farsi)|150|0",
            "RTLTMPro.TextUtils|System.Boolean IsEnglishNumber(System.Char ch)|150|0",
            "RTLTMPro.TextUtils|System.Boolean IsFarsiNumber(System.Char ch)|150|0",
            "RTLTMPro.TextUtils|System.Boolean IsHinduNumber(System.Char ch)|150|0",
            "RTLTMPro.TextUtils|System.Boolean IsEnglishLetter(System.Char ch)|150|0",
            "RTLTMPro.TextUtils|System.Boolean IsHebrewCharacter(System.Char ch)|150|0",
            "RTLTMPro.TextUtils|System.Boolean IsArabicCharacter(System.Char ch)|150|0",
            "RTLTMPro.TextUtils|System.Boolean IsRTLCharacter(System.Char ch)|150|0",
            "RTLTMPro.TextUtils|System.Boolean IsGlyphFixedArabicCharacter(System.Char ch)|150|0",
            "RTLTMPro.TextUtils|System.Boolean IsRTLInput(System.String input)|150|0",
        };
        private static readonly string[] AttributeDeclarations =
        {
            "RTLTMPro.RTLTextMeshPro|HLUnityCore.Runtime:Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option=1,System.Boolean=False)",
            "RTLTMPro.RTLTextMeshPro|HLUnityCore.Runtime:Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option=2,System.Boolean=False)",
            "RTLTMPro.RTLTextMeshPro|UnityEngine.CoreModule:UnityEngine.ExecuteInEditMode()",
            "RTLTMPro.RTLTextMeshPro.preserveNumbers|UnityEngine.CoreModule:UnityEngine.SerializeField()",
            "RTLTMPro.RTLTextMeshPro.farsi|UnityEngine.CoreModule:UnityEngine.SerializeField()",
            "RTLTMPro.RTLTextMeshPro.originalText|UnityEngine.CoreModule:UnityEngine.TextAreaAttribute(System.Int32=3,System.Int32=10)",
            "RTLTMPro.RTLTextMeshPro.originalText|UnityEngine.CoreModule:UnityEngine.SerializeField()",
            "RTLTMPro.RTLTextMeshPro.fixTags|UnityEngine.CoreModule:UnityEngine.SerializeField()",
            "RTLTMPro.RTLTextMeshPro.forceFix|UnityEngine.CoreModule:UnityEngine.SerializeField()",
            "RTLTMPro.RTLTextMeshPro.m_flipLeftAlignmentToRight|UnityEngine.CoreModule:UnityEngine.SerializeField()",
            "RTLTMPro.RTLTextMeshPro.m_inverseGeometrySortingOrder|UnityEngine.CoreModule:UnityEngine.SerializeField()",
            "RTLTMPro.RTLTextMeshPro3D|UnityEngine.CoreModule:UnityEngine.ExecuteInEditMode()",
            "RTLTMPro.RTLTextMeshPro3D.preserveNumbers|UnityEngine.CoreModule:UnityEngine.SerializeField()",
            "RTLTMPro.RTLTextMeshPro3D.farsi|UnityEngine.CoreModule:UnityEngine.SerializeField()",
            "RTLTMPro.RTLTextMeshPro3D.originalText|UnityEngine.CoreModule:UnityEngine.SerializeField()",
            "RTLTMPro.RTLTextMeshPro3D.originalText|UnityEngine.CoreModule:UnityEngine.TextAreaAttribute(System.Int32=3,System.Int32=10)",
            "RTLTMPro.RTLTextMeshPro3D.fixTags|UnityEngine.CoreModule:UnityEngine.SerializeField()",
            "RTLTMPro.RTLTextMeshPro3D.forceFix|UnityEngine.CoreModule:UnityEngine.SerializeField()",
            "RTLTMPro.TashkeelLocation.<Tashkeel>k__BackingField|mscorlib:System.Runtime.CompilerServices.CompilerGeneratedAttribute()",
            "RTLTMPro.TashkeelLocation.<Position>k__BackingField|mscorlib:System.Runtime.CompilerServices.CompilerGeneratedAttribute()",
        };
    }
}
