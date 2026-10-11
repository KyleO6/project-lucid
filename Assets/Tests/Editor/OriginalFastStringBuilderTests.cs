using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using RTLTMPro;

namespace ProjectLucid.Tests
{
    // Ordinary managed instances only. Current managed observations do not establish
    // shipping BCL/native exception identity, allocation faults or overflow parity.
    public sealed class OriginalFastStringBuilderTests
    {
        private const BindingFlags Declared = BindingFlags.Instance | BindingFlags.Static |
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

        [Test]
        public void CompleteOriginalDeclarationsRetainVisibilityAndImplementationFlags()
        {
            Type type = typeof(FastStringBuilder);
            Assert.That(type.Assembly.GetName().Name, Is.EqualTo("HLRTLTextMeshPro.Runtime"));
            Assert.That(type.FullName, Is.EqualTo("RTLTMPro.FastStringBuilder"));
            Assert.That((int)type.Attributes, Is.EqualTo(1048577));
            Assert.That(type.BaseType, Is.EqualTo(typeof(object)));
            Assert.That(type.GetInterfaces(), Is.Empty);
            Assert.That(type.GetNestedTypes(Declared), Is.Empty);
            Assert.That(type.GetCustomAttributesData(), Is.Empty);
            Assert.That(type.TypeInitializer, Is.Null);
            FieldInfo[] fields = type.GetFields(Declared).OrderBy(field => field.MetadataToken).ToArray();
            Assert.That(fields.Select(field => field.Name + "|" + field.FieldType.FullName + "|" +
                (int)field.Attributes).ToArray(), Is.EqualTo(new[]
                { "length|System.Int32|1", "array|System.Int32[]|1", "capacity|System.Int32|1" }));
            Assert.That(fields.All(field => field.GetCustomAttributesData().Count == 0), Is.True);
            MethodBase[] methods = type.GetMethods(Declared).Cast<MethodBase>()
                .Concat(type.GetConstructors(BindingFlags.Instance | BindingFlags.Public |
                    BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
                .OrderBy(method => method.MetadataToken).ToArray();
            string[] expected =
            {
                "System.Int32 get_Length()|2182|0",
                "System.Void set_Length(System.Int32 value)|2182|0",
                "System.Void .ctor(System.Int32 capacity)|6278|0",
                "System.Void .ctor(System.String text)|6278|0",
                "System.Void .ctor(System.String text, System.Int32 capacity)|6278|0",
                "System.String op_Implicit(RTLTMPro.FastStringBuilder x)|2198|0",
                "RTLTMPro.FastStringBuilder op_Implicit(System.String x)|2198|0",
                "System.Int32 Get(System.Int32 index)|134|256",
                "System.Void Set(System.Int32 index, System.Int32 ch)|134|256",
                "System.Void SetValue(System.String text)|134|0",
                "System.Void SetValue(RTLTMPro.FastStringBuilder other)|134|0",
                "System.Void Append(System.Int32 ch)|134|0",
                "System.Void Append(System.Char ch)|134|0",
                "System.Void Insert(System.Int32 pos, RTLTMPro.FastStringBuilder str, System.Int32 offset, System.Int32 count)|134|0",
                "System.Void Insert(System.Int32 pos, RTLTMPro.FastStringBuilder str)|134|0",
                "System.Void Insert(System.Int32 pos, System.Int32 ch)|134|0",
                "System.Void RemoveAll(System.Int32 character)|134|0",
                "System.Void Remove(System.Int32 start, System.Int32 length)|134|0",
                "System.Void Reverse(System.Int32 startIndex, System.Int32 length)|134|0",
                "System.Void Reverse()|134|0",
                "System.Void Substring(RTLTMPro.FastStringBuilder output, System.Int32 start, System.Int32 length)|134|0",
                "System.String ToString()|198|0",
                "System.String ToDebugString()|134|0",
                "System.Void Replace(System.Int32 oldChar, System.Int32 newChar)|134|0",
                "System.Void Replace(RTLTMPro.FastStringBuilder oldStr, RTLTMPro.FastStringBuilder newStr)|134|0",
                "System.Void Clear()|134|0",
                "System.Void EnsureCapacity(System.Int32 cap, System.Boolean keepValues)|129|0",
                "System.Void Copy(System.Int32[] src, System.Int32[] dst)|145|0",
            };
            Assert.That(methods.Select(Declaration).ToArray(), Is.EqualTo(expected));
            Assert.That(methods.All(method => method.GetCustomAttributesData().Count == 0), Is.True);
            Assert.That(methods.SelectMany(method => method.GetParameters()).All(parameter =>
                !parameter.HasDefaultValue && !parameter.IsOptional &&
                parameter.GetCustomAttributesData().Count == 0), Is.True);
            PropertyInfo[] properties = type.GetProperties(Declared);
            Assert.That(properties.Length, Is.EqualTo(1));
            PropertyInfo length = properties.Single();
            Assert.That(length.Name, Is.EqualTo("Length"));
            Assert.That(length.PropertyType, Is.EqualTo(typeof(int)));
            Assert.That(length.GetIndexParameters(), Is.Empty);
            Assert.That(length.GetMethod.IsPublic && length.SetMethod.IsPublic, Is.True);
            Assert.That(length.GetCustomAttributesData(), Is.Empty);
            Assert.That(type.GetEvents(Declared), Is.Empty);
        }

        [Test]
        public void OrdinaryConstructorsAndConversionsPreserveUnicodeScalars()
        {
            const string text = "A\uD83D\uDE00\u0628";
            var exact = new FastStringBuilder(text);
            var spare = new FastStringBuilder(text, 8);
            FastStringBuilder implicitBuilder = text;
            string implicitText = implicitBuilder;
            Assert.That(exact.Length, Is.EqualTo(3));
            Assert.That(new[] { exact.Get(0), exact.Get(1), exact.Get(2) },
                Is.EqualTo(new[] { 0x41, 0x1F600, 0x628 }));
            Assert.That(exact.ToString(), Is.EqualTo(text));
            Assert.That(exact.ToDebugString(), Is.EqualTo("\\41\\1F600\\628"));
            Assert.That(spare.ToString(), Is.EqualTo(text));
            Assert.That(implicitText, Is.EqualTo(text));
            var empty = new FastStringBuilder(0);
            empty.Append('Q');
            Assert.That(empty.ToString(), Is.EqualTo("Q"));
        }

        [Test]
        public void RawCapacityAccessAndNegativeLengthKeepShrinkOnlyPublication()
        {
            var builder = new FastStringBuilder("abc", 5);
            builder.Length = 8;
            Assert.That(builder.Length, Is.EqualTo(3));
            builder.Set(3, 0x7F);
            Assert.That(builder.Get(3), Is.EqualTo(0x7F));
            Assert.That(builder.ToString(), Is.EqualTo("abc"));
            builder.Length = 1;
            Assert.That(builder.Get(2), Is.EqualTo((int)'c'));
            builder.Length = -2;
            builder.Length = 0;
            Assert.That(builder.Length, Is.EqualTo(-2));
            Assert.That(builder.ToString(), Is.Empty);
            Assert.That(ObserveFailure(() => builder.Append('x')), Is.Not.Null);
            Assert.That(builder.Length, Is.EqualTo(-1));
            builder.Clear();
            builder.Append('q');
            Assert.That(builder.ToString(), Is.EqualTo("q"));
        }

        [Test]
        public void FullCapacityCopyWritesForwardAndPublishesLengthOnlyOnSuccess()
        {
            var source = new FastStringBuilder("AB", 4);
            var shortDestination = new FastStringBuilder("Z", 2);
            Assert.That(ObserveFailure(() => shortDestination.SetValue(source)), Is.Not.Null);
            Assert.That(shortDestination.Length, Is.EqualTo(1));
            Assert.That(new[] { shortDestination.Get(0), shortDestination.Get(1) },
                Is.EqualTo(new[] { (int)'A', (int)'B' }));
            var roomyDestination = new FastStringBuilder("zzzzzz", 8);
            roomyDestination.SetValue(source);
            Assert.That(roomyDestination.ToString(), Is.EqualTo("AB"));
            Assert.That(new[] { roomyDestination.Get(2), roomyDestination.Get(3), roomyDestination.Get(4) },
                Is.EqualTo(new[] { 0, 0, (int)'z' }));
            source.Set(0, 'X');
            Assert.That(roomyDestination.Get(0), Is.EqualTo((int)'A'));
        }

        [Test]
        public void SelfInsertRejectsBeforeZeroCountAndSelfSubstringReadsAfterClear()
        {
            var builder = new FastStringBuilder("abc", 8);
            Assert.Throws<InvalidOperationException>(() => builder.Insert(0, builder, 0, 0));
            Assert.That(builder.ToString(), Is.EqualTo("abc"));
            builder.Substring(builder, 1, 2);
            Assert.That(builder.ToString(), Is.EqualTo("bc"));
            builder.SetValue(builder);
            Assert.That(builder.ToString(), Is.EqualTo("bc"));
        }

        [Test]
        public void NominalMutationsComposeAndSubstringOwnsIndependentState()
        {
            var builder = new FastStringBuilder("abcdef");
            builder.Reverse(1, 4);
            builder.Remove(2, 2);
            builder.RemoveAll('e');
            builder.Insert(1, new FastStringBuilder("CD"), 0, 2);
            builder.Reverse();
            Assert.That(builder.ToString(), Is.EqualTo("fbDCa"));
            var output = new FastStringBuilder(0);
            builder.Substring(output, 1, 3);
            output.Replace('D', 'x');
            Assert.That(output.ToString(), Is.EqualTo("bxC"));
            Assert.That(builder.ToString(), Is.EqualTo("fbDCa"));
            output.Insert(1, new FastStringBuilder("!"));
            output.Insert(0, (int)'?');
            output.Append(0x1F600);
            output.Append('.');
            Assert.That(output.ToString(), Is.EqualTo("?b!xC\uD83D\uDE00."));
        }

        [Test]
        public void BuilderReplacePreservesSkippedMatchAndAsymmetricShrinkQuirks()
        {
            var sameSize = new FastStringBuilder("aaaa", 8);
            sameSize.Replace(new FastStringBuilder("aa"), new FastStringBuilder("bb"));
            Assert.That(sameSize.ToString(), Is.EqualTo("bbaa"));
            var shrinking = new FastStringBuilder("abcabc", 8);
            shrinking.Replace(new FastStringBuilder("abc"), new FastStringBuilder("x"));
            Assert.That(shrinking.ToString(), Is.EqualTo("xbbc"));
            var suffix = new FastStringBuilder("abc", 8);
            suffix.Length = 2;
            suffix.Replace(new FastStringBuilder("bc"), new FastStringBuilder("YZ"));
            Assert.That(suffix.ToString(), Is.EqualTo("aY"));
            Assert.That(suffix.Get(2), Is.EqualTo((int)'Z'));
            var edits = new FastStringBuilder("abc", 8);
            edits.Remove(5, 1);
            edits.Reverse(100, 1);
            Assert.That(edits.ToString(), Is.EqualTo("ab"));
        }

        [Test]
        public void ConversionFailuresRetainPublishedLengthAndRawScalarStorage()
        {
            var builder = new FastStringBuilder("old", 4);
            Assert.That(ObserveFailure(() => builder.SetValue("A\uD800")), Is.Not.Null);
            Assert.That(builder.Length, Is.EqualTo(2));
            Assert.That(builder.Get(0), Is.EqualTo((int)'A'));
            builder.Clear();
            builder.SetValue("ok");
            Assert.That(builder.ToString(), Is.EqualTo("ok"));
            var invalidScalar = new FastStringBuilder(1);
            invalidScalar.Append(0x110000);
            Assert.That(invalidScalar.Length, Is.EqualTo(1));
            Assert.That(invalidScalar.Get(0), Is.EqualTo(0x110000));
            Assert.That(invalidScalar.ToDebugString(), Is.EqualTo("\\110000"));
            Assert.That(ObserveFailure(() => invalidScalar.ToString()), Is.Not.Null);
            Assert.That(invalidScalar.Length, Is.EqualTo(1));
        }

        private static string Declaration(MethodBase method)
        {
            var ordinary = method as MethodInfo;
            string result = ordinary == null ? "System.Void" : ordinary.ReturnType.FullName;
            return result + " " + method.Name + "(" + string.Join(", ", method.GetParameters()
                .Select(parameter => parameter.ParameterType.FullName + " " + parameter.Name)) + ")|" +
                (int)method.Attributes + "|" + (int)method.GetMethodImplementationFlags();
        }

        private static Exception ObserveFailure(Action action)
        {
            try { action(); }
            catch (Exception failure) { return failure; }
            return null;
        }
    }
}
