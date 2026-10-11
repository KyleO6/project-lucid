using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Hardlight;
using HardlightProject;
using NUnit.Framework;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

public sealed class OriginalToggleCursorVisibilityContractsTests
{
    private const BindingFlags Declared = BindingFlags.DeclaredOnly | BindingFlags.Instance |
        BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
    private const BindingFlags InstanceConstructors = BindingFlags.DeclaredOnly | BindingFlags.Instance |
        BindingFlags.Public | BindingFlags.NonPublic;

    [Test]
    public void AuthoredCursorOwnerKeepsAllFieldsMethodsAndOrderedOptionsWithoutConstruction()
    {
        Type owner = typeof(ToggleCursorVisibility);
        Assert.AreEqual("Game.Runtime", owner.Assembly.GetName().Name);
        Assert.AreEqual("HardlightProject.ToggleCursorVisibility", owner.FullName);
        Assert.AreEqual(1048577, (int)owner.Attributes);
        Assert.AreEqual(typeof(MonoBehaviour), owner.BaseType);
        Assert.AreEqual("UnityEngine.CoreModule", owner.BaseType.Assembly.GetName().Name);
        Assert.IsFalse(owner.IsGenericType);
        Assert.AreEqual("HLUnityCore.Runtime", typeof(SystemRef<>).Assembly.GetName().Name);
        Assert.AreEqual("Game.Runtime", typeof(App).Assembly.GetName().Name);
        Assert.AreEqual("HLUnityUI.Runtime", typeof(UIRuntimeConfiguration).Assembly.GetName().Name);
        Assert.AreEqual("HLUnityUI.Runtime", typeof(UIInputType).Assembly.GetName().Name);
        Options(owner);
        FieldInfo[] fields = owner.GetFields(Declared).OrderBy(f => f.MetadataToken).ToArray();
        Assert.AreEqual(3, fields.Length);
        Field(fields[0], "m_appRef", typeof(SystemRef<App>), 33);
        Field(fields[1], "m_uiRuntimeConfigurationRef", typeof(SystemRef<UIRuntimeConfiguration>), 33);
        Field(fields[2], "m_forceReleaseCursor", typeof(bool), 1);
        Assert.AreEqual(0, owner.GetProperties(Declared).Length);
        Assert.AreEqual(0, owner.GetEvents(Declared).Length);
        Assert.AreEqual(0, owner.GetInterfaces().Length);
        Assert.AreEqual(2, owner.GetNestedTypes(Declared).Length);
        MethodInfo[] methods = owner.GetMethods(Declared);
        Assert.AreEqual(7, methods.Length);
        MethodInfo start = owner.GetMethod("Start", Declared);
        Method(start, 129, typeof(IEnumerator), Type.EmptyTypes, new string[0]);
        Type iterator = Iterator(owner, start);
        Assert.AreEqual(owner, iterator.DeclaringType);
        EmptyMethods(owner, new[] { "OnDestroy", "OnInputTypeChanged", "ForceReleaseCursor", "OnGameplayControlActive", "SetCursorVisibility" });
        Method(owner.GetMethod("OnDestroy", Declared), 129, typeof(void), Type.EmptyTypes, new string[0]);
        Method(owner.GetMethod("OnInputTypeChanged", Declared), 129, typeof(void), new[] { typeof(UIInputType) }, new[] { "inputType" });
        Method(owner.GetMethod("ForceReleaseCursor", Declared), 129, typeof(void), new[] { typeof(bool) }, new[] { "release" });
        Method(owner.GetMethod("OnGameplayControlActive", Declared), 129, typeof(void), new[] { typeof(bool) }, new[] { "on" });
        Method(owner.GetMethod("SetCursorVisibility", Declared), 129, typeof(void), new[] { typeof(bool) }, new[] { "on" });
        MethodInfo predicate = methods.Single(m => m.Name.StartsWith("<Start>b__", StringComparison.Ordinal));
        Method(predicate, 129, typeof(bool), Type.EmptyTypes, new string[0]);
        AttributeOnly(predicate, typeof(CompilerGeneratedAttribute));
        Constructor(owner, Type.EmptyTypes, new string[0], false);
        Assert.IsNull(owner.TypeInitializer);
        // typeof/MemberInfo/CAD only. The real constructor initializes SystemRefs;
        // Start and destruction reach input/services. None is called here.
    }

    [Test]
    public void CachedStartPredicateKeepsWholeDiscoveredCompilerRoleWithoutStaticReads()
    {
        Type owner = typeof(ToggleCursorVisibility);
        Type iterator = Iterator(owner, owner.GetMethod("Start", Declared));
        Type cache = owner.GetNestedTypes(Declared).Single(t => t != iterator);
        Assert.AreEqual(owner, cache.DeclaringType);
        Assert.AreEqual(owner.Namespace, cache.Namespace);
        Assert.AreEqual(owner.Assembly, cache.Assembly);
        Assert.IsTrue(cache.Name.StartsWith("<>c", StringComparison.Ordinal));
        GeneratedOwner(cache, 1057027);
        FieldInfo[] fields = cache.GetFields(Declared);
        Assert.AreEqual(2, fields.Length);
        FieldInfo singleton = fields.Single(f => f.FieldType == cache);
        Field(singleton, "<>9", cache, 54);
        FieldInfo predicateField = fields.Single(f => f.FieldType == typeof(Func<bool>));
        Assert.IsTrue(predicateField.Name.StartsWith("<>9__", StringComparison.Ordinal));
        Field(predicateField, predicateField.Name, typeof(Func<bool>), 22);
        MethodInfo predicate = cache.GetMethods(Declared).Single();
        Assert.IsTrue(predicate.Name.StartsWith("<Start>b__", StringComparison.Ordinal));
        Method(predicate, 131, typeof(bool), Type.EmptyTypes, new string[0]);
        Assert.AreEqual(0, predicate.GetCustomAttributesData().Count);
        Constructor(cache, Type.EmptyTypes, new string[0], false);
        ConstructorInfo initializer = cache.TypeInitializer;
        Assert.IsNotNull(initializer);
        Assert.AreEqual(6289, (int)initializer.Attributes);
        Assert.AreEqual(0, (int)initializer.GetMethodImplementationFlags());
        Parameters(initializer, Type.EmptyTypes, new string[0]);
        Assert.AreEqual(0, initializer.GetCustomAttributesData().Count);
        Assert.AreEqual(0, cache.GetInterfaces().Length);
        Assert.AreEqual(0, cache.GetProperties(Declared).Length);
        Assert.AreEqual(0, cache.GetEvents(Declared).Length);
        Assert.AreEqual(0, cache.GetNestedTypes(Declared).Length);
        // The original <>c/<>9__3_1 and callback ordinal are recorded by the source
        // packet. This test discovers the emitted role and never reads field values,
        // creates a Func, invokes .cctor or calls the EventSystem predicate.
    }

    [Test]
    public void StartIteratorKeepsCompleteInterfacesCaptureAndDebuggerAnnotationsWithoutMoving()
    {
        Type owner = typeof(ToggleCursorVisibility);
        Type iterator = Iterator(owner, owner.GetMethod("Start", Declared));
        Assert.AreEqual(owner, iterator.DeclaringType);
        Assert.AreEqual(owner.Namespace, iterator.Namespace);
        Assert.AreEqual(owner.Assembly, iterator.Assembly);
        Assert.IsTrue(iterator.Name.StartsWith("<Start>d__", StringComparison.Ordinal));
        GeneratedOwner(iterator, 1048835);
        FieldInfo[] fields = iterator.GetFields(Declared).OrderBy(f => f.MetadataToken).ToArray();
        Assert.AreEqual(3, fields.Length);
        Field(fields[0], "<>1__state", typeof(int), 1);
        Field(fields[1], "<>2__current", typeof(object), 1);
        Field(fields[2], "<>4__this", owner, 6);
        CollectionAssert.AreEquivalent(new[] { typeof(IEnumerator<object>), typeof(IEnumerator), typeof(IDisposable) }, iterator.GetInterfaces());
        Assert.AreEqual(5, iterator.GetMethods(Declared).Length);
        Constructor(iterator, new[] { typeof(int) }, new[] { "<>1__state" }, true);
        MethodInfo dispose = InterfaceTarget(iterator, typeof(IDisposable), "Dispose");
        Method(dispose, 481, typeof(void), Type.EmptyTypes, new string[0]);
        AttributeOnly(dispose, typeof(DebuggerHiddenAttribute));
        MethodInfo move = InterfaceTarget(iterator, typeof(IEnumerator), "MoveNext");
        Method(move, 481, typeof(bool), Type.EmptyTypes, new string[0]);
        Assert.AreEqual(0, move.GetCustomAttributesData().Count);
        MethodInfo reset = InterfaceTarget(iterator, typeof(IEnumerator), "Reset");
        Method(reset, 481, typeof(void), Type.EmptyTypes, new string[0]);
        AttributeOnly(reset, typeof(DebuggerHiddenAttribute));
        MethodInfo genericCurrent = InterfaceTarget(iterator, typeof(IEnumerator<object>), "get_Current");
        MethodInfo current = InterfaceTarget(iterator, typeof(IEnumerator), "get_Current");
        foreach (MethodInfo getter in new[] { genericCurrent, current })
        {
            Method(getter, 2529, typeof(object), Type.EmptyTypes, new string[0]);
            AttributeOnly(getter, typeof(DebuggerHiddenAttribute));
        }
        Assert.AreNotEqual(genericCurrent, current);
        PropertyInfo[] properties = iterator.GetProperties(Declared);
        Assert.AreEqual(2, properties.Length);
        CollectionAssert.AreEquivalent(new[] { genericCurrent, current }, properties.Select(p => p.GetGetMethod(true)).ToArray());
        foreach (PropertyInfo property in properties)
        {
            Assert.AreEqual(0, (int)property.Attributes);
            Assert.AreEqual(typeof(object), property.PropertyType);
            Assert.AreEqual(0, property.GetIndexParameters().Length);
            Assert.IsNull(property.GetSetMethod(true));
            Assert.AreEqual(0, property.GetCustomAttributesData().Count);
        }
        Assert.AreEqual(0, iterator.GetEvents(Declared).Length);
        Assert.AreEqual(0, iterator.GetNestedTypes(Declared).Length);
        // Interface maps inspect genuine metadata only. No iterator construction,
        // MoveNext/Current/Reset/Dispose, waits, event subscription or App call.
    }

    private static Type Iterator(Type owner, MethodInfo start)
    {
        IList<CustomAttributeData> attributes = start.GetCustomAttributesData();
        Assert.AreEqual(1, attributes.Count);
        CustomAttributeData attribute = attributes[0];
        Assert.AreEqual(typeof(IteratorStateMachineAttribute), attribute.AttributeType);
        Assert.AreEqual("mscorlib", attribute.AttributeType.Assembly.GetName().Name);
        CollectionAssert.AreEqual(new[] { typeof(Type) }, attribute.Constructor.GetParameters().Select(p => p.ParameterType).ToArray());
        Assert.AreEqual(1, attribute.ConstructorArguments.Count);
        Assert.AreEqual(typeof(Type), attribute.ConstructorArguments[0].ArgumentType);
        Assert.AreEqual(0, attribute.NamedArguments.Count);
        Type iterator = (Type)attribute.ConstructorArguments[0].Value;
        Assert.IsNotNull(iterator); Assert.AreEqual(owner, iterator.DeclaringType);
        return iterator;
    }

    private static MethodInfo InterfaceTarget(Type type, Type contract, string name)
    {
        InterfaceMapping map = type.GetInterfaceMap(contract);
        int index = Array.FindIndex(map.InterfaceMethods, m => m.Name == name);
        Assert.IsTrue(index >= 0);
        return map.TargetMethods[index];
    }

    private static void GeneratedOwner(Type type, int flags)
    {
        Assert.AreEqual(flags, (int)type.Attributes);
        Assert.AreEqual(typeof(object), type.BaseType); Assert.IsFalse(type.IsGenericType);
        IList<CustomAttributeData> attributes = type.GetCustomAttributesData();
        CustomAttributeData[] serializable = attributes.Where(a => a.AttributeType == typeof(SerializableAttribute)).ToArray();
        bool bit = (flags & 0x2000) != 0;
        Assert.AreEqual(bit ? 1 : 0, serializable.Length);
        foreach (CustomAttributeData marker in serializable) EmptyAttribute(marker, typeof(SerializableAttribute));
        CustomAttributeData[] raw = attributes.Where(a => a.AttributeType != typeof(SerializableAttribute)).ToArray();
        Assert.AreEqual(1, raw.Length); EmptyAttribute(raw[0], typeof(CompilerGeneratedAttribute));
        // Serializable is a TypeDef-bit reflection pseudoattribute. Its physical
        // raw-CAD/profile token is a separate current metadata gate, not normalized.
    }

    private static void Options(Type type)
    {
        IList<CustomAttributeData> attributes = type.GetCustomAttributesData(); Assert.AreEqual(2, attributes.Count);
        Option[] options = { Option.ArrayBoundsChecks, Option.NullChecks };
        for (int i = 0; i < attributes.Count; i++)
        {
            CustomAttributeData attribute = attributes[i];
            Assert.AreEqual(typeof(Il2CppSetOptionAttribute), attribute.AttributeType);
            Assert.AreEqual("HLUnityCore.Runtime", attribute.AttributeType.Assembly.GetName().Name);
            CollectionAssert.AreEqual(new[] { typeof(Option), typeof(object) }, attribute.Constructor.GetParameters().Select(p => p.ParameterType).ToArray());
            Assert.AreEqual(2, attribute.ConstructorArguments.Count);
            Assert.AreEqual(typeof(Option), attribute.ConstructorArguments[0].ArgumentType);
            Assert.AreEqual((int)options[i], Convert.ToInt32(attribute.ConstructorArguments[0].Value));
            Assert.AreEqual(typeof(bool), attribute.ConstructorArguments[1].ArgumentType);
            Assert.AreEqual(false, attribute.ConstructorArguments[1].Value);
            Assert.AreEqual(0, attribute.NamedArguments.Count);
        }
    }

    private static void Field(FieldInfo field, string name, Type type, int flags)
    {
        Assert.AreEqual(name, field.Name); Assert.AreEqual(type, field.FieldType);
        Assert.AreEqual(flags, (int)field.Attributes);
        Assert.IsFalse(field.IsLiteral); Assert.IsFalse(field.IsNotSerialized);
        Assert.AreEqual(0, field.GetCustomAttributesData().Count);
        // Values/default state are not read: construction would reach real providers.
    }

    private static void EmptyMethods(Type type, string[] names)
    {
        foreach (string name in names) Assert.AreEqual(0, type.GetMethod(name, Declared).GetCustomAttributesData().Count);
    }

    private static void Method(MethodInfo method, int flags, Type result, Type[] types, string[] names)
    {
        Assert.IsNotNull(method); Assert.AreEqual(flags, (int)method.Attributes);
        Assert.AreEqual(0, (int)method.GetMethodImplementationFlags());
        Assert.AreEqual(result, method.ReturnType); Assert.IsFalse(method.IsGenericMethod);
        Parameters(method, types, names);
        Assert.AreEqual(0, method.ReturnParameter.GetCustomAttributesData().Count);
    }

    private static void Constructor(Type type, Type[] types, string[] names, bool debuggerHidden)
    {
        ConstructorInfo constructor = type.GetConstructors(InstanceConstructors).Single();
        Assert.AreEqual(6278, (int)constructor.Attributes);
        Assert.AreEqual(0, (int)constructor.GetMethodImplementationFlags());
        Parameters(constructor, types, names);
        if (debuggerHidden) AttributeOnly(constructor, typeof(DebuggerHiddenAttribute));
        else Assert.AreEqual(0, constructor.GetCustomAttributesData().Count);
    }

    private static void Parameters(MethodBase method, Type[] types, string[] names)
    {
        ParameterInfo[] parameters = method.GetParameters();
        CollectionAssert.AreEqual(types, parameters.Select(p => p.ParameterType).ToArray());
        CollectionAssert.AreEqual(names, parameters.Select(p => p.Name).ToArray());
        foreach (ParameterInfo parameter in parameters)
        {
            Assert.AreEqual(0, (int)parameter.Attributes); Assert.IsFalse(parameter.HasDefaultValue);
            Assert.AreEqual(0, parameter.GetCustomAttributesData().Count);
        }
    }

    private static void AttributeOnly(MemberInfo member, Type type)
    {
        IList<CustomAttributeData> attributes = member.GetCustomAttributesData();
        Assert.AreEqual(1, attributes.Count); EmptyAttribute(attributes[0], type);
    }

    private static void EmptyAttribute(CustomAttributeData attribute, Type type)
    {
        Assert.AreEqual(type, attribute.AttributeType);
        Assert.AreEqual("mscorlib", attribute.AttributeType.Assembly.GetName().Name);
        Assert.AreEqual(0, attribute.Constructor.GetParameters().Length);
        Assert.AreEqual(0, attribute.ConstructorArguments.Count);
        Assert.AreEqual(0, attribute.NamedArguments.Count);
    }
}
