using System;
using System.Linq;
using System.Reflection;
using Hardlight;
using NUnit.Framework;
using UnityEngine;

namespace ProjectLucid.Tests
{
    // Ordinary struct cases allocate no Selectable, scene object or system provider.
    // These are rebuilt managed contracts; native pointer/fault parity remains separate.
    public sealed class OriginalHLNavigationTests
    {
        private const BindingFlags Declared = BindingFlags.Public | BindingFlags.NonPublic
            | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;

        [Test]
        public void WholeNavigationDeclarationsRetainOriginalFieldsModesAndAccessors()
        {
            Type type = typeof(HLNavigation);
            Assert.That(type.Assembly.GetName().Name, Is.EqualTo("HLUnityUI.Runtime"));
            Assert.That(type.FullName, Is.EqualTo("Hardlight.HLNavigation"));
            Assert.That((int)type.Attributes, Is.EqualTo(1057033));
            Assert.That(type.IsValueType, Is.True);
            Assert.That(type.IsSerializable, Is.True);
            CollectionAssert.AreEqual(new[] { typeof(IEquatable<HLNavigation>) }, type.GetInterfaces());
            Assert.That(type.GetConstructors(Declared), Is.Empty);
            Assert.That(type.GetEvents(Declared), Is.Empty);
            CollectionAssert.AreEqual(new[] { typeof(HLNavigation.NavMode) }, type.GetNestedTypes(Declared));

            Type mode = typeof(HLNavigation.NavMode);
            Assert.That((int)mode.Attributes, Is.EqualTo(258));
            Assert.That(Enum.GetUnderlyingType(mode), Is.EqualTo(typeof(int)));
            Assert.That(mode.GetCustomAttributesData().Count(a => a.AttributeType == typeof(FlagsAttribute)), Is.EqualTo(1));
            CollectionAssert.AreEqual(new[] { "None", "Horizontal", "Vertical", "Automatic", "Explicit" }, Enum.GetNames(mode));
            CollectionAssert.AreEqual(new[] { 0, 1, 2, 3, 4 }, Enum.GetValues(mode).Cast<HLNavigation.NavMode>().Select(v => (int)v).ToArray());

            FieldInfo[] fields = type.GetFields(Declared).OrderBy(f => f.MetadataToken).ToArray();
            CollectionAssert.AreEqual(new[] { "m_mode", "m_selectOnUp", "m_selectOnDown", "m_selectOnLeft", "m_selectOnRight" }, fields.Select(f => f.Name).ToArray());
            CollectionAssert.AreEqual(new[] { mode, typeof(HLSelectable), typeof(HLSelectable), typeof(HLSelectable), typeof(HLSelectable) }, fields.Select(f => f.FieldType).ToArray());
            foreach (FieldInfo field in fields)
            {
                Assert.That((int)field.Attributes, Is.EqualTo(1), field.Name);
                Assert.That(field.GetCustomAttributesData().Select(a => a.AttributeType).ToArray(), Is.EqualTo(new[] { typeof(SerializeField) }), field.Name);
            }

            string[] methodNames = { "get_Mode", "set_Mode", "get_SelectOnUp", "set_SelectOnUp", "get_SelectOnDown", "set_SelectOnDown", "get_SelectOnLeft", "set_SelectOnLeft", "get_SelectOnRight", "set_SelectOnRight", "get_DefaultNavigation", "Equals" };
            MethodInfo[] methods = type.GetMethods(Declared).OrderBy(m => m.MetadataToken).ToArray();
            CollectionAssert.AreEqual(methodNames, methods.Select(m => m.Name).ToArray());
            for (int i = 0; i < methods.Length; i++)
            {
                MethodInfo method = methods[i];
                Assert.That((int)method.Attributes, Is.EqualTo(i < 10 ? 2182 : i == 10 ? 2198 : 486), method.Name);
                Assert.That((int)method.GetMethodImplementationFlags(), Is.EqualTo(0), method.Name);
                Assert.That(method.IsGenericMethod, Is.False, method.Name);
                ParameterInfo[] parameters = method.GetParameters();
                Type valueType = i < 2 ? mode : typeof(HLSelectable);
                Type returnType = i < 10 ? (i % 2 == 0 ? valueType : typeof(void)) : i == 10 ? type : typeof(bool);
                Assert.That(method.ReturnType, Is.EqualTo(returnType), method.Name);
                Assert.That(parameters.Length, Is.EqualTo(i < 10 ? i % 2 : i == 10 ? 0 : 1), method.Name);
                if (parameters.Length == 1)
                {
                    Assert.That(parameters[0].Name, Is.EqualTo(i == 11 ? "other" : "value"));
                    Assert.That(parameters[0].ParameterType, Is.EqualTo(i == 11 ? type : valueType));
                    Assert.That((int)parameters[0].Attributes, Is.EqualTo(0));
                    Assert.That(parameters[0].HasDefaultValue, Is.False);
                }
            }
            PropertyInfo[] properties = type.GetProperties(Declared).OrderBy(p => p.MetadataToken).ToArray();
            CollectionAssert.AreEqual(new[] { "Mode", "SelectOnUp", "SelectOnDown", "SelectOnLeft", "SelectOnRight", "DefaultNavigation" }, properties.Select(p => p.Name).ToArray());
            foreach (PropertyInfo property in properties)
            {
                Assert.That(property.GetIndexParameters(), Is.Empty);
                Assert.That(property.GetGetMethod(), Is.Not.Null);
                Assert.That(property.CanWrite, Is.EqualTo(property.Name != "DefaultNavigation"));
            }
        }

        [Test]
        public void OrdinaryNavigationPreservesZeroDefaultUndefinedModesAndValueCopies()
        {
            HLNavigation zero = default(HLNavigation);
            HLNavigation automatic = HLNavigation.DefaultNavigation;
            Assert.That(zero.Mode, Is.EqualTo(HLNavigation.NavMode.None));
            Assert.That(automatic.Mode, Is.EqualTo(HLNavigation.NavMode.Automatic));
            Assert.That(zero.Equals(default(HLNavigation)), Is.True);
            Assert.That(zero.Equals(automatic), Is.False);
            Assert.That(automatic.SelectOnUp, Is.Null);
            Assert.That(automatic.SelectOnDown, Is.Null);
            Assert.That(automatic.SelectOnLeft, Is.Null);
            Assert.That(automatic.SelectOnRight, Is.Null);

            HLNavigation copy = automatic;
            copy.Mode = (HLNavigation.NavMode)(-17);
            copy.SelectOnUp = null;
            copy.SelectOnDown = null;
            copy.SelectOnLeft = null;
            copy.SelectOnRight = null;
            Assert.That((int)copy.Mode, Is.EqualTo(-17));
            Assert.That(automatic.Mode, Is.EqualTo(HLNavigation.NavMode.Automatic));
            Assert.That(copy.Equals(automatic), Is.False);
            HLNavigation same = default(HLNavigation);
            same.Mode = (HLNavigation.NavMode)(-17);
            Assert.That(copy.Equals(same), Is.True);
            same.Mode = (HLNavigation.NavMode)47;
            Assert.That((int)same.Mode, Is.EqualTo(47));
            Assert.That(copy.Equals(same), Is.False);
            Assert.That(HLNavigation.DefaultNavigation.Equals(automatic), Is.True);
        }
    }
}
