using System;
using System.Linq;
using System.Reflection;
using Hardlight;
using NUnit.Framework;
using Unity.IL2CPP.CompilerServices;

namespace ProjectLucid.Editor
{
    // These cases exercise the rebuilt managed implementation and genuine Unity math.
    // Original native floating-point, library, fault and gameplay parity remain separate.
    public sealed class OriginalEaseUtilitiesTests
    {
        private static readonly string[] Names =
        {
            "InQuad", "OutQuad", "InOutQuad", "InCubic", "OutCubic", "InOutCubic",
            "InQuartic", "OutQuartic", "InOutQuartic", "InQuintic", "OutQuintic", "InOutQuintic",
            "InSinusoidal", "OutSinusoidal", "InOutSinusoidal", "InElastic", "OutElastic", "InOutElastic",
            "InBounce", "OutBounce", "InOutBounce", "InSmallBounce", "OutSmallBounce", "InOutSmallBounce",
            "InBack", "OutBack", "InOutBack", "NONE"
        };

        private static readonly Func<float, float>[] Curves =
        {
            EaseUtilities.InQuad, EaseUtilities.OutQuad, EaseUtilities.InOutQuad,
            EaseUtilities.InCubic, EaseUtilities.OutCubic, EaseUtilities.InOutCubic,
            EaseUtilities.InQuartic, EaseUtilities.OutQuartic, EaseUtilities.InOutQuartic,
            EaseUtilities.InQuintic, EaseUtilities.OutQuintic, EaseUtilities.InOutQuintic,
            EaseUtilities.InSinusoidal, EaseUtilities.OutSinusoidal, EaseUtilities.InOutSinusoidal,
            EaseUtilities.InElastic, EaseUtilities.OutElastic, EaseUtilities.InOutElastic,
            EaseUtilities.InBounce, EaseUtilities.OutBounce, EaseUtilities.InOutBounce,
            EaseUtilities.InSmallBounce, EaseUtilities.OutSmallBounce, EaseUtilities.InOutSmallBounce,
            EaseUtilities.InBack, EaseUtilities.OutBack, EaseUtilities.InOutBack
        };

        private static readonly Func<float, float>[] ClampedCurves =
        {
            EaseUtilities.InSinusoidal, EaseUtilities.OutSinusoidal, EaseUtilities.InOutSinusoidal,
            EaseUtilities.InExponential, EaseUtilities.OutExponential, EaseUtilities.InOutExponential,
            EaseUtilities.InElastic, EaseUtilities.OutElastic, EaseUtilities.InOutElastic
        };

        private const BindingFlags Declared = BindingFlags.Public | BindingFlags.NonPublic |
            BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        private static void Near(float actual, float expected)
        {
            Assert.That(float.IsNaN(actual) || float.IsInfinity(actual), Is.False);
            Assert.That(actual, Is.EqualTo(expected).Within(0.000003f));
        }

        private static void SameBits(float actual, float expected)
        {
            Assert.That(BitConverter.ToInt32(BitConverter.GetBytes(actual), 0),
                Is.EqualTo(BitConverter.ToInt32(BitConverter.GetBytes(expected), 0)));
        }

        [Test]
        public void OriginalDeclarationsAndOrderedOptionMetadata()
        {
            Type type = typeof(EaseUtilities);
            Assert.That(type.FullName, Is.EqualTo("Hardlight.EaseUtilities"));
            Assert.That(type.Assembly.GetName().Name, Is.EqualTo("HLUnityCore.Runtime"));
            Assert.That(type.BaseType, Is.EqualTo(typeof(object)));
            Assert.That((int)type.Attributes, Is.EqualTo(1048577));
            Assert.That(type.IsAbstract || type.IsSealed || type.IsGenericType, Is.False);
            Assert.That(type.TypeInitializer, Is.Null);
            Assert.That(type.GetFields(Declared), Is.Empty);
            Assert.That(type.GetProperties(Declared), Is.Empty);
            Assert.That(type.GetEvents(Declared), Is.Empty);
            Assert.That(type.GetInterfaces(), Is.Empty);
            Assert.That(type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic),
                Is.EqualTo(new[] { typeof(EaseUtilities.EaseType) }));

            var options = type.GetCustomAttributesData();
            Assert.That(options.Count, Is.EqualTo(2));
            for (int i = 0; i < options.Count; i++)
            {
                var option = options[i];
                Assert.That(option.AttributeType, Is.EqualTo(typeof(Il2CppSetOptionAttribute)));
                Assert.That(option.AttributeType.Assembly, Is.EqualTo(type.Assembly));
                Assert.That(option.Constructor.DeclaringType, Is.EqualTo(typeof(Il2CppSetOptionAttribute)));
                Assert.That(option.Constructor.GetParameters().Select(p => p.ParameterType).ToArray(),
                    Is.EqualTo(new[] { typeof(Option), typeof(object) }));
                Assert.That(option.ConstructorArguments.Count, Is.EqualTo(2));
                Assert.That(option.ConstructorArguments[0].ArgumentType, Is.EqualTo(typeof(Option)));
                Assert.That(Convert.ToInt32(option.ConstructorArguments[0].Value), Is.EqualTo(i + 1));
                // CAD retains the concrete boxed Boolean value of the genuine object parameter.
                Assert.That(option.ConstructorArguments[1].ArgumentType, Is.EqualTo(typeof(bool)));
                Assert.That(option.ConstructorArguments[1].Value, Is.EqualTo(false));
                Assert.That(option.NamedArguments, Is.Empty);
            }

            Type enumType = typeof(EaseUtilities.EaseType);
            Assert.That(enumType.FullName, Is.EqualTo("Hardlight.EaseUtilities+EaseType"));
            Assert.That(enumType.DeclaringType, Is.EqualTo(type));
            Assert.That((int)enumType.Attributes, Is.EqualTo(258));
            Assert.That(enumType.BaseType, Is.EqualTo(typeof(Enum)));
            Assert.That(enumType.IsEnum && enumType.IsNestedPublic && enumType.IsSealed, Is.True);
            Assert.That(Enum.GetUnderlyingType(enumType), Is.EqualTo(typeof(int)));
            Assert.That(enumType.GetCustomAttributesData(), Is.Empty);
            FieldInfo[] fields = enumType.GetFields(Declared);
            Assert.That(fields.Length, Is.EqualTo(29));
            FieldInfo storage = fields.Single(f => f.Name == "value__");
            Assert.That(storage.FieldType, Is.EqualTo(typeof(int)));
            Assert.That((int)storage.Attributes, Is.EqualTo(1542));
            Assert.That(storage.GetCustomAttributesData(), Is.Empty);
            for (int i = 0; i < Names.Length; i++)
            {
                FieldInfo field = fields.Single(f => f.Name == Names[i]);
                Assert.That(field.FieldType, Is.EqualTo(enumType));
                Assert.That((int)field.Attributes, Is.EqualTo(32854));
                Assert.That(field.GetRawConstantValue(), Is.EqualTo(i));
                Assert.That(field.GetCustomAttributesData(), Is.Empty);
            }

            MethodInfo[] methods = type.GetMethods(Declared);
            string[] unary = Names.Take(27).Concat(new[]
            {
                "InQuadIntegral", "OutQuadIntergral", "InOutQuadIntergral",
                "InExponential", "OutExponential", "InOutExponential"
            }).ToArray();
            Assert.That(methods.Length, Is.EqualTo(34));
            Assert.That(methods.Select(m => m.Name), Is.EquivalentTo(unary.Concat(new[] { "Ease" })));
            foreach (MethodInfo method in methods)
            {
                Assert.That((int)method.Attributes, Is.EqualTo(150));
                Assert.That(method.ReturnType, Is.EqualTo(typeof(float)));
                Assert.That(method.CallingConvention, Is.EqualTo(CallingConventions.Standard));
                Assert.That(method.IsGenericMethod || method.ContainsGenericParameters, Is.False);
                Assert.That(method.GetCustomAttributesData(), Is.Empty);
                Assert.That(method.ReturnParameter.GetCustomAttributesData(), Is.Empty);
                ParameterInfo[] parameters = method.GetParameters();
                Assert.That(parameters.Select(p => p.ParameterType), Is.EqualTo(method.Name == "Ease"
                    ? new[] { enumType, typeof(float) } : new[] { typeof(float) }));
                Assert.That(parameters.Select(p => p.Name), Is.EqualTo(method.Name == "Ease"
                    ? new[] { "type", "t" } : new[] { "t" }));
                foreach (ParameterInfo parameter in parameters)
                {
                    Assert.That((int)parameter.Attributes, Is.EqualTo(0));
                    Assert.That(parameter.HasDefaultValue || parameter.IsOptional, Is.False);
                    Assert.That(parameter.GetCustomAttributesData(), Is.Empty);
                }
            }
            ConstructorInfo[] constructors = type.GetConstructors(Declared);
            Assert.That(constructors.Length, Is.EqualTo(1));
            Assert.That((int)constructors[0].Attributes, Is.EqualTo(6278));
            Assert.That(constructors[0].GetParameters(), Is.Empty);
            Assert.That(constructors[0].GetCustomAttributesData(), Is.Empty);
            EaseUtilities first = new EaseUtilities();
            EaseUtilities second = new EaseUtilities();
            Assert.That(first.GetType(), Is.EqualTo(type));
            Assert.That(ReferenceEquals(first, second), Is.False);
        }

        [Test]
        public void PolynomialNominalValuesAndRawInputs()
        {
            // Rational samples are independent of the reconstructed expression spelling.
            float[] atQuarter = { 0.0625f, 0.4375f, 0.125f, 0.015625f, 0.578125f, 0.0625f,
                0.00390625f, 0.68359375f, 0.03125f, 0.0009765625f, 0.7626953125f, 0.015625f };
            float[] atThreeQuarters = { 0.5625f, 0.9375f, 0.875f, 0.421875f, 0.984375f, 0.9375f,
                0.31640625f, 0.99609375f, 0.96875f, 0.2373046875f, 0.9990234375f, 0.984375f };
            for (int i = 0; i < 12; i++)
            {
                Near(Curves[i](0f), 0f);
                Near(Curves[i](1f), 1f);
                Near(Curves[i](0.25f), atQuarter[i]);
                Near(Curves[i](0.75f), atThreeQuarters[i]);
            }
            Near(EaseUtilities.InQuad(-1f), 1f);
            Near(EaseUtilities.OutQuad(-1f), -3f);
            Near(EaseUtilities.InOutQuad(-1f), 2f);
            Near(EaseUtilities.InCubic(-1f), -1f);
            Near(EaseUtilities.InQuartic(2f), 16f);
            Near(EaseUtilities.OutQuartic(2f), 0f);
            Near(EaseUtilities.InQuintic(2f), 32f);
        }

        [Test]
        public void OriginalIntegralValuesAndUpperBranchDiscontinuity()
        {
            Near(EaseUtilities.InQuadIntegral(0.5f), 1f / 24f);
            Near(EaseUtilities.InQuadIntegral(1f), 1f / 3f);
            Near(EaseUtilities.OutQuadIntergral(0.5f), 5f / 24f);
            Near(EaseUtilities.OutQuadIntergral(1f), 2f / 3f);
            Near(EaseUtilities.InOutQuadIntergral(0.25f), 1f / 96f);
            Near(EaseUtilities.InOutQuadIntergral(0.49f), 0.0784326666667f);
            Near(EaseUtilities.InOutQuadIntergral(0.5f), -1f / 12f);
            Near(EaseUtilities.InOutQuadIntergral(0.75f), 3f / 32f);
            Near(EaseUtilities.InOutQuadIntergral(1f), 1f / 3f);
            Assert.That(EaseUtilities.InOutQuadIntergral(0.49f), Is.GreaterThan(0.07f));
            Assert.That(EaseUtilities.InOutQuadIntergral(0.5f), Is.LessThan(0f));
        }

        [Test]
        public void TranscendentalBounceAndBackNominalValues()
        {
            Near(EaseUtilities.InSinusoidal(0.5f), 0.29289321881345f);
            Near(EaseUtilities.OutSinusoidal(0.5f), 0.70710678118655f);
            Near(EaseUtilities.InOutSinusoidal(0.5f), 0.5f);
            Near(EaseUtilities.InExponential(0.5f), 1f / 32f);
            Near(EaseUtilities.OutExponential(0.5f), 31f / 32f);
            Near(EaseUtilities.InOutExponential(0.5f), 0.5f);
            Near(EaseUtilities.InElastic(0.5f), -1f / 64f);
            Near(EaseUtilities.OutElastic(0.5f), 65f / 64f);
            Near(EaseUtilities.InOutElastic(0.5f), 0.5f);
            Near(EaseUtilities.OutBounce(0.5f), 49f / 64f);
            Near(EaseUtilities.InBounce(0.5f), 15f / 64f);
            Near(EaseUtilities.InOutBounce(0.5f), 0.5f);
            Near(EaseUtilities.OutBounce(1.5f / 2.75f), 0.75f);
            Near(EaseUtilities.OutBounce(2.25f / 2.75f), 0.9375f);
            Near(EaseUtilities.OutBounce(2.625f / 2.75f), 0.984375f);
            Near(EaseUtilities.OutSmallBounce(0.5f), 25f / 36f);
            Near(EaseUtilities.InSmallBounce(0.5f), 11f / 36f);
            Near(EaseUtilities.OutSmallBounce(0.8f), 0.96f);
            Near(EaseUtilities.InOutSmallBounce(0.5f), 0.5f);
            Near(EaseUtilities.InBack(0.5f), -0.0876975f);
            Near(EaseUtilities.OutBack(0.5f), 1.0876975f);
            Near(EaseUtilities.InOutBack(0.5f), 0.5f);
        }

        [Test]
        public void LiteralDispatcherAndDefaultPassThrough()
        {
            float[] inputs = { -0.125f, 0f, 0.125f, 0.5f, 0.875f, 1f, 1.125f };
            for (int i = 0; i < Curves.Length; i++)
                foreach (float input in inputs)
                    SameBits(EaseUtilities.Ease((EaseUtilities.EaseType)i, input), Curves[i](input));
            int[] defaults = { -1, 27, 28, int.MinValue, int.MaxValue };
            float negativeZero = BitConverter.ToSingle(BitConverter.GetBytes(unchecked((int)0x80000000)), 0);
            float[] unchanged = { -3f, negativeZero, 0f, 2.5f, float.NegativeInfinity, float.PositiveInfinity };
            foreach (int value in defaults)
            {
                foreach (float input in unchanged)
                    SameBits(EaseUtilities.Ease((EaseUtilities.EaseType)value, input), input);
                Assert.That(float.IsNaN(EaseUtilities.Ease((EaseUtilities.EaseType)value, float.NaN)), Is.True);
            }
        }

        [Test]
        public void EndpointClampsAndNaNPropagation()
        {
            foreach (Func<float, float> curve in ClampedCurves)
            {
                SameBits(curve(float.NegativeInfinity), 0f);
                SameBits(curve(-0.5f), 0f);
                SameBits(curve(0f), 0f);
                SameBits(curve(1f), 1f);
                SameBits(curve(1.5f), 1f);
                SameBits(curve(float.PositiveInfinity), 1f);
                Assert.That(float.IsNaN(curve(float.NaN)), Is.True);
            }
            foreach (Func<float, float> curve in Curves)
                Assert.That(float.IsNaN(curve(float.NaN)), Is.True);
            Assert.That(float.IsNaN(EaseUtilities.InQuadIntegral(float.NaN)), Is.True);
            Assert.That(float.IsNaN(EaseUtilities.OutQuadIntergral(float.NaN)), Is.True);
            Assert.That(float.IsNaN(EaseUtilities.InOutQuadIntergral(float.NaN)), Is.True);
            for (int i = 0; i < Curves.Length; i++)
                Assert.That(float.IsNaN(EaseUtilities.Ease((EaseUtilities.EaseType)i, float.NaN)), Is.True);
        }
    }
}
