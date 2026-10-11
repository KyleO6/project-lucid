using System;
using System.Linq;
using System.Reflection;
using Hardlight;
using HardlightProject;
using NUnit.Framework;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

public sealed class OriginalStackedCameraTests
{
    private const BindingFlags Declared = BindingFlags.Public | BindingFlags.NonPublic |
        BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;

    [Test]
    public void StackedCameraRetainsWholeOriginalTypeFieldsAndOrderedAnnotations()
    {
        Type type = typeof(StackedCamera);
        Assert.That(type.FullName, Is.EqualTo("HardlightProject.StackedCamera"));
        Assert.That(type.Assembly.GetName().Name, Is.EqualTo("Game.Runtime"));
        Assert.That(type.BaseType, Is.EqualTo(typeof(MonoBehaviour)));
        Assert.That((int)type.Attributes, Is.EqualTo(1048577));
        Assert.That(type.IsGenericType, Is.False);
        Assert.That(type.GetNestedTypes(Declared), Is.Empty);
        Assert.That(type.GetProperties(Declared), Is.Empty);
        Assert.That(type.GetEvents(Declared), Is.Empty);
        Assert.That(type.GetInterfaces().Except(typeof(MonoBehaviour).GetInterfaces()), Is.Empty);

        var attributes = type.GetCustomAttributesData();
        Assert.That(attributes.Count, Is.EqualTo(3));
        AssertAttribute(attributes[0], typeof(RequireComponent), new[] { typeof(Type) });
        Assert.That(attributes[0].ConstructorArguments.Count, Is.EqualTo(1));
        Assert.That(attributes[0].ConstructorArguments[0].ArgumentType, Is.EqualTo(typeof(Type)));
        Assert.That(attributes[0].ConstructorArguments[0].Value, Is.EqualTo(typeof(Camera)));
        AssertOption(attributes[1], Option.ArrayBoundsChecks);
        AssertOption(attributes[2], Option.NullChecks);

        FieldInfo[] fields = type.GetFields(Declared).OrderBy(f => f.MetadataToken).ToArray();
        Assert.That(fields.Select(f => f.Name), Is.EqualTo(new[]
        {
            "m_camera", "m_audioListener", "m_cameraStackManagerRef"
        }));
        Assert.That(fields.Select(f => f.FieldType), Is.EqualTo(new[]
        {
            typeof(Camera), typeof(AudioListener), typeof(SystemRef<CameraStackManager>)
        }));
        foreach (FieldInfo field in fields)
        {
            Assert.That((int)field.Attributes, Is.EqualTo(1), field.Name);
            Assert.That(field.IsStatic || field.IsInitOnly || field.IsLiteral, Is.False, field.Name);
        }
        for (int i = 0; i < 2; ++i)
        {
            var fieldAttributes = fields[i].GetCustomAttributesData();
            Assert.That(fieldAttributes.Count, Is.EqualTo(1));
            AssertAttribute(fieldAttributes[0], typeof(SerializeField), Type.EmptyTypes);
            Assert.That(fieldAttributes[0].ConstructorArguments, Is.Empty);
        }
        Assert.That(fields[2].GetCustomAttributesData(), Is.Empty);
    }

    [Test]
    public void StackedCameraRetainsSixOriginalMethodsConstructorAndParameterContracts()
    {
        Type type = typeof(StackedCamera);
        MethodInfo[] methods = type.GetMethods(Declared).OrderBy(m => m.MetadataToken).ToArray();
        Assert.That(methods.Select(m => m.Name), Is.EqualTo(new[]
        {
            "Reset", "OnEnable", "OnValidate", "OnCameraStackManagerSystemStartup", "OnDisable"
        }));
        foreach (MethodInfo method in methods)
        {
            Assert.That((int)method.Attributes, Is.EqualTo(129), method.Name);
            Assert.That(method.GetMethodImplementationFlags(), Is.EqualTo(MethodImplAttributes.IL));
            Assert.That(method.ReturnType, Is.EqualTo(typeof(void)));
            // Unity Mono projects Retval even when the raw return Param row is absent.
            Assert.That(method.ReturnParameter.Attributes, Is.EqualTo(ParameterAttributes.Retval));
            Assert.That(method.ReturnParameter.GetCustomAttributesData(), Is.Empty);
            Assert.That(method.GetCustomAttributesData(), Is.Empty);
            Assert.That(method.IsGenericMethod, Is.False);
            Assert.That(method.IsAbstract || method.IsVirtual || method.IsStatic, Is.False);
            if (method.Name == "OnCameraStackManagerSystemStartup")
            {
                ParameterInfo[] parameters = method.GetParameters();
                Assert.That(parameters.Length, Is.EqualTo(1));
                Assert.That(parameters[0].Name, Is.EqualTo("cameraStackManager"));
                Assert.That(parameters[0].ParameterType, Is.EqualTo(typeof(CameraStackManager)));
                Assert.That(parameters[0].Attributes, Is.EqualTo(ParameterAttributes.None));
                Assert.That(parameters[0].HasDefaultValue, Is.False);
                Assert.That(parameters[0].GetCustomAttributesData(), Is.Empty);
            }
            else
                Assert.That(method.GetParameters(), Is.Empty);
        }
        ConstructorInfo[] constructors = type.GetConstructors(Declared);
        Assert.That(constructors.Length, Is.EqualTo(1));
        Assert.That((int)constructors[0].Attributes, Is.EqualTo(6278));
        Assert.That(constructors[0].GetMethodImplementationFlags(), Is.EqualTo(MethodImplAttributes.IL));
        Assert.That(constructors[0].GetParameters(), Is.Empty);
        Assert.That(constructors[0].GetCustomAttributesData(), Is.Empty);
        Assert.That(type.TypeInitializer, Is.Null);
    }

    private static void AssertAttribute(CustomAttributeData attribute, Type expectedType,
        Type[] constructorParameters)
    {
        Assert.That(attribute.AttributeType, Is.EqualTo(expectedType));
        Assert.That(attribute.Constructor.DeclaringType, Is.EqualTo(expectedType));
        Assert.That(attribute.Constructor.GetParameters().Select(p => p.ParameterType),
            Is.EqualTo(constructorParameters));
        Assert.That(attribute.NamedArguments, Is.Empty);
    }

    private static void AssertOption(CustomAttributeData attribute, Option expectedOption)
    {
        AssertAttribute(attribute, typeof(Il2CppSetOptionAttribute), new[] { typeof(Option), typeof(object) });
        Assert.That(attribute.ConstructorArguments.Count, Is.EqualTo(2));
        Assert.That(attribute.ConstructorArguments[0].ArgumentType, Is.EqualTo(typeof(Option)));
        Assert.That(Convert.ToInt32(attribute.ConstructorArguments[0].Value), Is.EqualTo((int)expectedOption));
        Assert.That(attribute.ConstructorArguments[1].ArgumentType, Is.EqualTo(typeof(bool)));
        Assert.That(attribute.ConstructorArguments[1].Value, Is.EqualTo(false));
    }
}
