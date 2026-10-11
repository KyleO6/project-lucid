using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Hardlight;
using HardlightProject;
using NUnit.Framework;
using Unity.IL2CPP.CompilerServices;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class OriginalBadgeStartTests
{
    private const BindingFlags Declared = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

    [Test]
    public void BadgeDeclarationsRetainOriginalSerializedFieldsAndOptionalLeaf()
    {
        Type t = typeof(UIWidgetSDTAlertBadge);
        Assert.AreEqual(1048833, (int)t.Attributes);
        Assert.AreEqual(typeof(UIWidget), t.BaseType);
        Assert.AreEqual("Game.Runtime", t.Assembly.GetName().Name);
        Options(t);
        Fields(t, new[] {
            F("m_alertText", typeof(string), 1), F("m_maxValue", typeof(int), 1),
            F("m_textFormatOverMaxValue", typeof(string), 1),
            F("m_text", typeof(RTLTMPro.RTLTextMeshPro), 1),
            F("m_enableObjectWhileActive", typeof(GameObject), 1) });
        FieldInfo[] fields = t.GetFields(Declared).OrderBy(f => f.MetadataToken).ToArray();
        string[] tooltips = {
            "Text to fill when in the alerting state.",
            "Limit of numerical value to display. If value goes over this, then the max value is used with the below text format.",
            "Text format to decorate max value with when max value is exceeded. Parameter {0} = max value.",
            "RTLTextMeshPro to fill text into.",
            "Enable this object while the alert is active. Disable it when inactive." };
        for (int i = 0; i < fields.Length; i++)
        {
            IList<CustomAttributeData> rows = fields[i].GetCustomAttributesData();
            int at = 0;
            if (i == 1) { Attribute(rows[at++], typeof(MinAttribute), new[] { typeof(float) }, new object[] { 1f }); }
            Attribute(rows[at++], typeof(SerializeField), Type.EmptyTypes, new object[0]);
            Attribute(rows[at++], typeof(TooltipAttribute), new[] { typeof(string) }, new object[] { tooltips[i] });
            Assert.AreEqual(at, rows.Count);
        }
        MethodInfo validate = t.GetMethod("OnValidate", Declared);
        Method(validate, M("OnValidate", 129, typeof(void), Type.EmptyTypes, new string[0]));
        MethodInfo alert = t.GetMethod("SetAlert", Declared);
        Assert.AreEqual(134, (int)alert.Attributes);
        Assert.AreEqual(typeof(void), alert.ReturnType);
        ParameterInfo[] parameters = alert.GetParameters();
        Assert.AreEqual(2, parameters.Length);
        Type[] types = { typeof(bool), typeof(int) }; object[] values = { false, 0 }; string[] names = { "alert", "displayNumber" };
        for (int i = 0; i < parameters.Length; i++)
        {
            Assert.AreEqual(names[i], parameters[i].Name); Assert.AreEqual(types[i], parameters[i].ParameterType);
            Assert.AreEqual(4112, (int)parameters[i].Attributes); Assert.IsTrue(parameters[i].HasDefaultValue);
            Assert.AreEqual(values[i], parameters[i].DefaultValue);
            // Mono exposes the Optional bit as a metadata pseudoattribute; it is separate from stored shipping CAD.
            IList<CustomAttributeData> rows = parameters[i].GetCustomAttributesData(); Assert.AreEqual(1, rows.Count);
            Attribute(rows[0], typeof(System.Runtime.InteropServices.OptionalAttribute), Type.EmptyTypes, new object[0]);
        }
        Assert.AreEqual(0, alert.GetCustomAttributesData().Count); Assert.AreEqual(0, alert.ReturnParameter.GetCustomAttributesData().Count);
        Constructor(t); Assert.AreEqual(2, t.GetMethods(Declared).Length);
        Assert.AreEqual(0, t.GetNestedTypes(Declared).Length); Assert.AreEqual(0, t.GetProperties(Declared).Length); Assert.AreEqual(0, t.GetEvents(Declared).Length);
    }

    [Test]
    public void BadgeInactiveConstructionRetainsGenuineSerializedDefaults()
    {
        GameObject root = null;
        try
        {
            root = new GameObject("Owned inactive alert badge defaults"); root.SetActive(false);
            UIWidgetSDTAlertBadge badge = root.AddComponent<UIWidgetSDTAlertBadge>();
            Assert.IsFalse(root.activeInHierarchy); Assert.AreSame(root, badge.gameObject);
            using (var serialized = new SerializedObject(badge))
            {
                serialized.Update(); Defaults(serialized);
                Assert.IsTrue(serialized.FindProperty("m_text").objectReferenceValue == null);
                Assert.IsTrue(serialized.FindProperty("m_enableObjectWhileActive").objectReferenceValue == null);
            }
            badge.Setup(null); // Genuine inherited empty hook, including a null parameter.
            Assert.IsFalse(root.activeInHierarchy);
        }
        finally { if (root != null) Object.DestroyImmediate(root); }
    }

    [Test]
    public void BadgeNonpositiveFalseAlertsDisableOwnedTargetBeforeTextUse()
    {
        GameObject root = null, target = null;
        try
        {
            root = new GameObject("Owned inactive alert badge leaf"); root.SetActive(false);
            UIWidgetSDTAlertBadge badge = root.AddComponent<UIWidgetSDTAlertBadge>();
            target = new GameObject("Owned badge activation target"); target.transform.SetParent(root.transform, false);
            using (var serialized = new SerializedObject(badge))
            {
                serialized.Update(); serialized.FindProperty("m_enableObjectWhileActive").objectReferenceValue = target;
                serialized.ApplyModifiedPropertiesWithoutUndo(); serialized.Update();
                Assert.AreSame(target, serialized.FindProperty("m_enableObjectWhileActive").objectReferenceValue);
                Assert.IsTrue(serialized.FindProperty("m_text").objectReferenceValue == null);
            }
            target.SetActive(true); Assert.IsTrue(target.activeSelf); Assert.IsFalse(target.activeInHierarchy);
            badge.SetAlert(); Assert.IsFalse(target.activeSelf);
            foreach (int count in new[] { int.MinValue, -1, 0 })
            {
                target.SetActive(true); badge.SetAlert(false, count);
                Assert.IsFalse(target.activeSelf); Assert.IsFalse(root.activeInHierarchy);
                using (var serialized = new SerializedObject(badge))
                {
                    serialized.Update(); Defaults(serialized);
                    Assert.IsTrue(serialized.FindProperty("m_text").objectReferenceValue == null);
                    Assert.AreSame(target, serialized.FindProperty("m_enableObjectWhileActive").objectReferenceValue);
                }
            }
        }
        finally
        {
            try { if (target != null) Object.DestroyImmediate(target); }
            finally { if (root != null) Object.DestroyImmediate(root); }
        }
    }

    [Test]
    public void StartAuthoredDeclarationsRetainOriginalBindingsAndCallbacksWithoutConstruction()
    {
        Type t = typeof(UIContainerStart);
        Assert.AreEqual(1048833, (int)t.Attributes); Assert.AreEqual(typeof(UIContainer), t.BaseType);
        Assert.AreEqual("Game.Runtime", t.Assembly.GetName().Name); Options(t);
        Assert.IsTrue(typeof(ISaveGameListener).IsAssignableFrom(t));
        Fields(t, new[] {
            F("m_sweetDreamsCategory", typeof(HardlightProject.GameplayLevelCategory), 1),
            F("m_mediaAlertBadge", typeof(HardlightProject.UIWidgetSDTAlertBadge), 1),
            F("m_bonusZoneButton", typeof(HardlightProject.UIWidgetButtonStart), 1),
            F("m_challengesButton", typeof(HardlightProject.UIWidgetButtonStart), 1),
            F("m_challengesAnalyticsEvent", typeof(Hardlight.UIModernEvent), 1),
            F("CanContinue", typeof(Hardlight.UI.Binding.Bindable<System.Boolean>), 38),
            F("CanLoad", typeof(Hardlight.UI.Binding.Bindable<System.Boolean>), 38),
            F("FirstTime", typeof(Hardlight.UI.Binding.Bindable<System.Boolean>), 38),
            F("BonusZoneUnlocked", typeof(Hardlight.UI.Binding.Bindable<System.Boolean>), 38),
            F("ChallengesProgressAdventure", typeof(Hardlight.UI.Binding.Bindable<System.String>), 38),
            F("RewardsFormatString", typeof(Hardlight.UI.Binding.Bindable<System.String>), 38),
            F("OrbRewardsCollected", typeof(Hardlight.UI.Binding.Bindable<System.Int32>), 38),
            F("OrbRewardsTotal", typeof(Hardlight.UI.Binding.Bindable<System.Int32>), 38),
            F("MoonRewardsCollected", typeof(Hardlight.UI.Binding.Bindable<System.Int32>), 38),
            F("MoonRewardsTotal", typeof(Hardlight.UI.Binding.Bindable<System.Int32>), 38),
            F("m_saveManagerRef", typeof(Hardlight.SystemRef<HardlightProject.SaveManager>), 33),
            F("m_app", typeof(HardlightProject.App), 1),
            F("m_progressionManager", typeof(HardlightProject.ProgressionManager), 1),
            F("m_transitionManager", typeof(Hardlight.UIScreenTransitionManager), 1),
            F("m_levelManager", typeof(HardlightProject.LevelManager), 1),
            F("m_ornamentManager", typeof(HardlightProject.OrnamentManager), 1),
            F("m_musicTrackManager", typeof(HardlightProject.MusicTrackManager), 1),
        });
        FieldInfo[] fields = t.GetFields(Declared).OrderBy(f => f.MetadataToken).ToArray();
        for (int i = 0; i < fields.Length; i++)
        {
            IList<CustomAttributeData> rows = fields[i].GetCustomAttributesData();
            Assert.AreEqual(i < 5 ? 1 : 0, rows.Count);
            if (i < 5) Attribute(rows[0], typeof(SerializeField), Type.EmptyTypes, new object[0]);
        }
        MethodContract[] methods = {
            M("Start", 129, typeof(void), new Type[] {  }, new string[] {  }),
            M("OnDestroy", 129, typeof(void), new Type[] {  }, new string[] {  }),
            M("Action_Continue", 134, typeof(void), new Type[] {  }, new string[] {  }),
            M("Action_Challenges", 134, typeof(void), new Type[] {  }, new string[] {  }),
            M("Action_BonusZone", 134, typeof(void), new Type[] {  }, new string[] {  }),
            M("UpdateUI", 129, typeof(void), new Type[] {  }, new string[] {  }),
            M("UpdateUIAfterTransition", 129, typeof(System.Collections.IEnumerator), new Type[] { typeof(HardlightProject.SaveManager), typeof(HardlightProject.SaveDataGame) }, new string[] { "saveManager", "currentSave" }),
            M("UpdateBonusZoneUI", 129, typeof(System.Collections.IEnumerator), new Type[] { typeof(HardlightProject.SaveDataGame) }, new string[] { "currentSave" }),
            M("UpdateChallengesUI", 129, typeof(System.Collections.IEnumerator), new Type[] { typeof(HardlightProject.SaveDataGame) }, new string[] { "currentSave" }),
            M("UpdateLanguageDependentUI", 129, typeof(void), new Type[] { typeof(Hardlight.Enums.Languages) }, new string[] { "_" }),
            M("UpdateRewards", 145, typeof(void), new Type[] { typeof(System.Collections.Generic.IReadOnlyDictionary<HardlightProject.CollectableType, HardlightProject.ProgressionCollectable>), typeof(HardlightProject.CollectableType), typeof(Hardlight.UI.Binding.Bindable<System.Int32>), typeof(Hardlight.UI.Binding.Bindable<System.Int32>) }, new string[] { "overallProgress", "collectableType", "collectedBindable", "totalBindable" }),
            M("AreAllSaveSlotsEmpty", 129, typeof(System.Boolean), new Type[] {  }, new string[] {  }),
            M("StartGame", 145, typeof(void), new Type[] {  }, new string[] {  }),
            M("UpdateMediaAlertBadge", 129, typeof(void), new Type[] {  }, new string[] {  }),
            M("OnSaveGameOpen", 486, typeof(void), new Type[] { typeof(HardlightProject.SaveDataGame) }, new string[] { "saveDataGame" }),
            M("OnSaveGameClose", 486, typeof(void), new Type[] { typeof(HardlightProject.SaveDataGame) }, new string[] { "saveDataGame" }),
            M("GetTotalNewMediaCount", 129, typeof(System.Int32), new Type[] {  }, new string[] {  }),
        };
        foreach (MethodContract expected in methods) Method(t.GetMethod(expected.Name, Declared), expected);
        Constructor(t);
        // Two compiler-generated predicates remain distinct from the 18 authored methods, including the constructor.
        MethodInfo[] predicates = t.GetMethods(Declared).Where(m => !methods.Any(e => e.Name == m.Name)).ToArray();
        Assert.AreEqual(2, predicates.Length);
        foreach (string role in new[] { "UpdateBonusZoneUI", "UpdateChallengesUI" })
        {
            MethodInfo predicate = predicates.Single(m => m.Name.StartsWith("<" + role + ">b__", StringComparison.Ordinal));
            Method(predicate, M(predicate.Name, 129, typeof(bool), Type.EmptyTypes, new string[0]));
            IList<CustomAttributeData> rows = predicate.GetCustomAttributesData(); Assert.AreEqual(1, rows.Count);
            Attribute(rows[0], typeof(CompilerGeneratedAttribute), Type.EmptyTypes, new object[0]);
        }
        Assert.AreEqual(19, t.GetMethods(Declared).Length); Assert.AreEqual(0, t.GetProperties(Declared).Length); Assert.AreEqual(0, t.GetEvents(Declared).Length);
        InterfaceMapping map = t.GetInterfaceMap(typeof(ISaveGameListener));
        CollectionAssert.AreEquivalent(new[] { "OnSaveGameOpen", "OnSaveGameClose" }, map.TargetMethods.Select(m => m.Name).ToArray());
        Assert.IsTrue(map.TargetMethods.All(m => m.DeclaringType == t));
    }

    [Test]
    public void StartNaturalIteratorsRetainCompleteRolesWithoutStartingMenuFlow()
    {
        Type owner = typeof(UIContainerStart); var seen = new List<Type>();
        foreach (string role in new[] { "UpdateBonusZoneUI", "UpdateChallengesUI", "UpdateUIAfterTransition" })
        {
            MethodInfo factory = owner.GetMethod(role, Declared);
            IList<CustomAttributeData> rows = factory.GetCustomAttributesData(); Assert.AreEqual(1, rows.Count);
            CustomAttributeData row = rows[0];
            Assert.AreEqual(typeof(IteratorStateMachineAttribute), row.AttributeType);
            CollectionAssert.AreEqual(new[] { typeof(Type) }, row.Constructor.GetParameters().Select(p => p.ParameterType).ToArray());
            Assert.AreEqual(1, row.ConstructorArguments.Count); Assert.AreEqual(typeof(Type), row.ConstructorArguments[0].ArgumentType);
            Assert.AreEqual(0, row.NamedArguments.Count);
            Type iterator = (Type)row.ConstructorArguments[0].Value;
            Assert.AreEqual(owner, iterator.DeclaringType); Assert.IsTrue(iterator.Name.StartsWith("<" + role + ">d__", StringComparison.Ordinal));
            Assert.AreEqual(1048835, (int)iterator.Attributes); Assert.AreEqual(typeof(object), iterator.BaseType); Assert.IsFalse(seen.Contains(iterator)); seen.Add(iterator);
            IList<CustomAttributeData> generated = iterator.GetCustomAttributesData(); Assert.AreEqual(1, generated.Count);
            Attribute(generated[0], typeof(CompilerGeneratedAttribute), Type.EmptyTypes, new object[0]);
            CollectionAssert.AreEquivalent(new[] { typeof(IEnumerator), typeof(IEnumerator<object>), typeof(IDisposable) }, iterator.GetInterfaces());
            var expected = new List<FieldContract> { F("<>1__state", typeof(int), 1), F("<>2__current", typeof(object), 1), F("<>4__this", owner, 6), F("currentSave", typeof(SaveDataGame), 6) };
            if (role == "UpdateUIAfterTransition") expected.Add(F("saveManager", typeof(SaveManager), 6));
            FieldInfo[] fields = iterator.GetFields(Declared); Assert.AreEqual(expected.Count, fields.Length);
            foreach (FieldContract f in expected) Field(fields.Single(x => x.Name == f.Name), f);
            MethodInfo[] methods = iterator.GetMethods(Declared); Assert.AreEqual(5, methods.Length);
            string[] names = { "System.IDisposable.Dispose", "MoveNext", "System.Collections.Generic.IEnumerator<System.Object>.get_Current", "System.Collections.IEnumerator.Reset", "System.Collections.IEnumerator.get_Current" };
            int[] flags = { 481, 481, 2529, 481, 2529 }; Type[] returns = { typeof(void), typeof(bool), typeof(object), typeof(void), typeof(object) };
            for (int i = 0; i < names.Length; i++) Method(methods.Single(m => m.Name == names[i]), M(names[i], flags[i], returns[i], Type.EmptyTypes, new string[0]));
            ConstructorInfo constructor = iterator.GetConstructors(Declared).Single(); Assert.AreEqual(6278, (int)constructor.Attributes);
            CollectionAssert.AreEqual(new[] { typeof(int) }, constructor.GetParameters().Select(p => p.ParameterType).ToArray());
            Assert.AreEqual("<>1__state", constructor.GetParameters()[0].Name); Assert.AreEqual(0, (int)constructor.GetParameters()[0].Attributes);
            PropertyInfo[] properties = iterator.GetProperties(Declared); Assert.AreEqual(2, properties.Length);
            CollectionAssert.AreEquivalent(new[] { "System.Collections.Generic.IEnumerator<System.Object>.Current", "System.Collections.IEnumerator.Current" }, properties.Select(p => p.Name).ToArray());
            foreach (PropertyInfo property in properties) { Assert.AreEqual(typeof(object), property.PropertyType); Assert.IsNull(property.GetSetMethod(true)); Assert.AreEqual(0, property.GetIndexParameters().Length); }
            Assert.AreEqual(0, iterator.GetNestedTypes(Declared).Length); Assert.AreEqual(0, iterator.GetEvents(Declared).Length);
        }
        CollectionAssert.AreEquivalent(seen, owner.GetNestedTypes(Declared));
        // No constructors, factories, MoveNext, callbacks or services are invoked in this metadata case.
    }

    [Test]
    public void StartRewardsPublishCollectedBeforeTotalAndUseZerosForMissingEntry()
    {
        var progress = new Dictionary<CollectableType, ProgressionCollectable> {
            { CollectableType.Orb, new ProgressionCollectable(CollectableType.Moon, 17, 29) } };
        var collected = new Hardlight.UI.Binding.Bindable<int>(-11);
        var total = new Hardlight.UI.Binding.Bindable<int>(-22);
        var observed = new List<string>();
        Action onCollected = () => observed.Add("collected:" + collected.Value + ":" + total.Value);
        Action onTotal = () => observed.Add("total:" + collected.Value + ":" + total.Value);
        collected.AddListener(onCollected); total.AddListener(onTotal);
        try
        {
            Rewards(progress, CollectableType.Orb, collected, total);
            CollectionAssert.AreEqual(new[] { "collected:17:-22", "total:17:29" }, observed);
            Assert.AreEqual(CollectableType.Moon, progress[CollectableType.Orb].Type);
            observed.Clear(); Rewards(progress, CollectableType.Moon, collected, total);
            CollectionAssert.AreEqual(new[] { "collected:0:29", "total:0:0" }, observed);
            Assert.AreEqual(0, collected.Value); Assert.AreEqual(0, total.Value);
            observed.Clear(); Rewards(progress, CollectableType.Moon, collected, total);
            Assert.AreEqual(0, observed.Count); Assert.AreEqual(1, progress.Count);
        }
        finally { try { collected.RemoveListener(onCollected); } finally { total.RemoveListener(onTotal); } }
    }

    [Test]
    public void StartRewardsRetainOwnedCallbackAndNullFaultPublicationOrder()
    {
        var progress = new Dictionary<CollectableType, ProgressionCollectable> {
            { CollectableType.Orb, new ProgressionCollectable(CollectableType.Orb, -3, -7) } };
        var collected = new Hardlight.UI.Binding.Bindable<int>(5);
        var total = new Hardlight.UI.Binding.Bindable<int>(9);
        var failure = new InvalidOperationException("Owned collected callback failure");
        int calls = 0; Action onCollected = () => { calls++; throw failure; };
        collected.AddListener(onCollected);
        try
        {
            TargetInvocationException wrapper = Assert.Throws<TargetInvocationException>(() => Rewards(progress, CollectableType.Orb, collected, total));
            Assert.AreSame(failure, wrapper.InnerException); Assert.AreEqual(1, calls);
            Assert.AreEqual(-3, collected.Value); Assert.AreEqual(9, total.Value);
        }
        finally { collected.RemoveListener(onCollected); }
        TargetInvocationException nullDictionary = Assert.Throws<TargetInvocationException>(() => Rewards(null, CollectableType.Orb, collected, total));
        Assert.AreEqual(typeof(NullReferenceException), nullDictionary.InnerException.GetType());
        Assert.AreEqual(-3, collected.Value); Assert.AreEqual(9, total.Value);
        TargetInvocationException nullCollected = Assert.Throws<TargetInvocationException>(() => Rewards(progress, CollectableType.Orb, null, total));
        Assert.AreEqual(typeof(NullReferenceException), nullCollected.InnerException.GetType()); Assert.AreEqual(9, total.Value);
        int observed = 0; Action observe = () => observed++;
        collected.AddListener(observe);
        try
        {
            // The failed callback left this genuine Bindable uninitialised; another assignment still publishes.
            TargetInvocationException nullTotal = Assert.Throws<TargetInvocationException>(() => Rewards(progress, CollectableType.Orb, collected, null));
            Assert.AreEqual(typeof(NullReferenceException), nullTotal.InnerException.GetType());
            Assert.AreEqual(-3, collected.Value); Assert.AreEqual(1, observed);
        }
        finally { collected.RemoveListener(observe); }
    }

    private static void Rewards(IReadOnlyDictionary<CollectableType, ProgressionCollectable> progress,
        CollectableType type, Hardlight.UI.Binding.Bindable<int> collected, Hardlight.UI.Binding.Bindable<int> total)
    {
        MethodInfo method = typeof(UIContainerStart).GetMethod("UpdateRewards", Declared);
        Assert.IsNotNull(method); Assert.IsTrue(method.IsPrivate); Assert.IsTrue(method.IsStatic);
        method.Invoke(null, new object[] { progress, type, collected, total });
    }

    private sealed class FieldContract { public string Name; public Type Type; public int Flags; }
    private sealed class MethodContract { public string Name; public int Flags; public Type Return; public Type[] Parameters; public string[] Names; }
    private static FieldContract F(string name, Type type, int flags) { return new FieldContract { Name = name, Type = type, Flags = flags }; }
    private static MethodContract M(string name, int flags, Type result, Type[] parameters, string[] names) { return new MethodContract { Name = name, Flags = flags, Return = result, Parameters = parameters, Names = names }; }
    private static void Fields(Type owner, FieldContract[] expected)
    {
        FieldInfo[] actual = owner.GetFields(Declared).OrderBy(f => f.MetadataToken).ToArray(); Assert.AreEqual(expected.Length, actual.Length);
        for (int i = 0; i < actual.Length; i++) Field(actual[i], expected[i]);
    }
    private static void Field(FieldInfo actual, FieldContract expected)
    {
        Assert.AreEqual(expected.Name, actual.Name); Assert.AreEqual(expected.Type, actual.FieldType); Assert.AreEqual(expected.Flags, (int)actual.Attributes);
        Assert.IsFalse(actual.IsLiteral); Assert.IsFalse(actual.IsNotSerialized); Assert.IsFalse(actual.IsStatic);
    }
    private static void Method(MethodInfo actual, MethodContract expected)
    {
        Assert.IsNotNull(actual, expected.Name); Assert.AreEqual(expected.Name, actual.Name); Assert.AreEqual(expected.Flags, (int)actual.Attributes);
        Assert.AreEqual(expected.Return, actual.ReturnType); Assert.IsFalse(actual.IsGenericMethod);
        ParameterInfo[] parameters = actual.GetParameters(); CollectionAssert.AreEqual(expected.Parameters, parameters.Select(p => p.ParameterType).ToArray());
        CollectionAssert.AreEqual(expected.Names, parameters.Select(p => p.Name).ToArray());
        foreach (ParameterInfo parameter in parameters) { Assert.AreEqual(0, (int)parameter.Attributes); Assert.IsFalse(parameter.HasDefaultValue); Assert.AreEqual(0, parameter.GetCustomAttributesData().Count); }
        Assert.AreEqual(0, actual.ReturnParameter.GetCustomAttributesData().Count);
    }
    private static void Constructor(Type type)
    {
        ConstructorInfo constructor = type.GetConstructors(Declared).Single(); Assert.AreEqual(6278, (int)constructor.Attributes);
        Assert.AreEqual(0, constructor.GetParameters().Length); Assert.AreEqual(0, constructor.GetCustomAttributesData().Count);
    }
    private static void Options(Type type)
    {
        IList<CustomAttributeData> rows = type.GetCustomAttributesData(); Assert.AreEqual(2, rows.Count);
        for (int i = 0; i < rows.Count; i++)
        {
            CustomAttributeData row = rows[i]; Assert.AreEqual(typeof(Il2CppSetOptionAttribute), row.AttributeType);
            CollectionAssert.AreEqual(new[] { typeof(Option), typeof(object) }, row.Constructor.GetParameters().Select(p => p.ParameterType).ToArray());
            Assert.AreEqual(2, row.ConstructorArguments.Count); Assert.AreEqual(typeof(Option), row.ConstructorArguments[0].ArgumentType);
            Assert.AreEqual(i == 0 ? 2 : 1, Convert.ToInt32(row.ConstructorArguments[0].Value));
            Assert.AreEqual(typeof(bool), row.ConstructorArguments[1].ArgumentType); Assert.AreEqual(false, row.ConstructorArguments[1].Value); Assert.AreEqual(0, row.NamedArguments.Count);
        }
    }
    private static void Attribute(CustomAttributeData actual, Type type, Type[] signature, object[] values)
    {
        Assert.AreEqual(type, actual.AttributeType); CollectionAssert.AreEqual(signature, actual.Constructor.GetParameters().Select(p => p.ParameterType).ToArray());
        Assert.AreEqual(values.Length, actual.ConstructorArguments.Count);
        for (int i = 0; i < values.Length; i++) { Assert.AreEqual(signature[i], actual.ConstructorArguments[i].ArgumentType); Assert.AreEqual(values[i], actual.ConstructorArguments[i].Value); }
        Assert.AreEqual(0, actual.NamedArguments.Count);
    }
    private static void Defaults(SerializedObject serialized)
    {
        Assert.AreEqual("!", serialized.FindProperty("m_alertText").stringValue);
        Assert.AreEqual(99, serialized.FindProperty("m_maxValue").intValue);
        Assert.AreEqual("{0}+", serialized.FindProperty("m_textFormatOverMaxValue").stringValue);
    }
}
