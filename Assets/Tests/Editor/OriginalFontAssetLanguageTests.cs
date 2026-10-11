using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hardlight;
using Hardlight.Enums;
using HardlightProject;
using NUnit.Framework;
using TMPro;
using Unity.IL2CPP.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace ProjectLucid.Tests
{
    public sealed class OriginalFontAssetLanguageTests
    {
        private const BindingFlags Declared = BindingFlags.Public | BindingFlags.NonPublic |
            BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;

        [Test]
        public void FontAuthoredDeclarationsRetainExactOriginalContracts()
        {
            // Original authored roles are 02000956/957/958: three types, eleven
            // methods including constructors, eleven fields and seven parameters.
            // Four natural compiler owners are separately qualified metadata.
            Type data = typeof(FontAssetAndFallbacks);
            Type pairing = typeof(FontAssetAndFallbacks.FontAssetFallbackPairing);
            Type setter = typeof(FontAssetLanguageSetter);
            Assert.That((int)data.Attributes, Is.EqualTo(1048577));
            Assert.That((int)pairing.Attributes, Is.EqualTo(1056770));
            Assert.That((int)setter.Attributes, Is.EqualTo(1048833));
            Assert.That(data.BaseType, Is.EqualTo(typeof(ScriptableObject)));
            Assert.That(pairing.BaseType, Is.EqualTo(typeof(object)));
            Assert.That(setter.BaseType, Is.EqualTo(typeof(MonoBehaviour)));
            Assert.That(data.GetInterfaces(), Is.EquivalentTo(new[] { typeof(ISerializationCallbackReceiver) }));
            Assert.That(pairing.GetInterfaces(), Is.Empty);
            AssertOptions(data, false, 1, 2);
            AssertOptions(pairing, true, 1, 2);
            AssertOptions(setter, false, 2, 1);

            AssertFields(data, new[] { "m_fontAsset", "m_fallbackCollection" },
                new[] { typeof(TMP_FontAsset), typeof(List<FontAssetAndFallbacks.FontAssetFallbackPairing>) },
                new[] { 6, 6 });
            AssertFields(pairing, new[] { "Name", "m_language", "m_fontsForLanguage" },
                new[] { typeof(string), typeof(Languages), typeof(List<AssetReferenceT<TMP_FontAsset>>) },
                new[] { 6, 6, 6 });
            AssertFields(setter, new[] { "m_fontAssetsAndFallBackCollectionRefs", "m_fontAssetsAndFallBackCollection",
                "m_asyncOperationHandles", "m_appRef", "m_assetManager", "m_isRespondingToLanguageChange" },
                new[] { typeof(List<AssetReferenceT<FontAssetAndFallbacks>>), typeof(List<FontAssetAndFallbacks>),
                    typeof(Dictionary<FontAssetAndFallbacks, List<AsyncOperationHandle<TMP_FontAsset>>>),
                    typeof(SystemRef<App>), typeof(AddressableManager), typeof(bool) },
                new[] { 1, 33, 33, 33, 1, 1 });
            AssertFieldAttribute(pairing.GetField("Name", Declared), typeof(HideInInspector));
            AssertFieldAttribute(setter.GetField("m_fontAssetsAndFallBackCollectionRefs", Declared), typeof(SerializeField));
            Assert.That(data.GetMethods(Declared).Select(m => m.Name),
                Is.EquivalentTo(new[] { "OnBeforeSerialize", "OnAfterDeserialize" }));
            Assert.That(pairing.GetMethods(Declared), Is.Empty);
            Assert.That(setter.GetMethods(Declared).Select(m => m.Name), Is.EquivalentTo(new[] {
                "Start", "ShouldInvokeStringHandlers", "LanguageChanged", "OnDestroy", "OnAppValid", "WaitForHandlesMarkReady" }));
            AssertMethod(data, "OnBeforeSerialize", typeof(void), 486, new Type[0], new string[0]);
            AssertMethod(data, "OnAfterDeserialize", typeof(void), 486, new Type[0], new string[0]);
            AssertMethod(setter, "Start", typeof(void), 129, new Type[0], new string[0]);
            AssertMethod(setter, "ShouldInvokeStringHandlers", typeof(bool), 129, new Type[0], new string[0]);
            AssertMethod(setter, "LanguageChanged", typeof(void), 129, new[] { typeof(Languages) }, new[] { "language" });
            AssertMethod(setter, "OnDestroy", typeof(void), 129, new Type[0], new string[0]);
            Type[] arguments = { typeof(IGraphUser), typeof(List<AsyncOperationHandle>), typeof(GraphStorageKey) };
            string[] argumentNames = { "app", "handles", "varName" };
            AssertMethod(setter, "OnAppValid", typeof(void), 129, arguments, argumentNames);
            AssertMethod(setter, "WaitForHandlesMarkReady", typeof(IEnumerator), 145, arguments, argumentNames);
            foreach (Type type in new[] { data, pairing, setter })
            {
                ConstructorInfo[] constructors = type.GetConstructors(Declared);
                Assert.That(constructors.Length, Is.EqualTo(1));
                Assert.That((int)constructors[0].Attributes, Is.EqualTo(6278));
                Assert.That(constructors[0].GetParameters(), Is.Empty);
            }
            // Metadata inspection above never constructs the component or attributes.
            var ordinaryPairing = new FontAssetAndFallbacks.FontAssetFallbackPairing();
            Assert.That(ordinaryPairing.Name, Is.Null);
            Assert.That(ordinaryPairing.m_language, Is.EqualTo(default(Languages)));
            Assert.That(ordinaryPairing.m_fontsForLanguage, Is.Null);
        }

        [Test]
        public void FontConfigurationNamingAndSerializationUseGenuineOwnedData()
        {
            FontAssetAndFallbacks config = null;
            try
            {
                config = ScriptableObject.CreateInstance<FontAssetAndFallbacks>();
                Languages[] languages = (Languages[])Enum.GetValues(typeof(Languages));
                Assert.That(languages.Length, Is.GreaterThan(0));
                var first = new FontAssetAndFallbacks.FontAssetFallbackPairing { Name = "before-first", m_language = languages[0] };
                var last = new FontAssetAndFallbacks.FontAssetFallbackPairing { Name = "before-last", m_language = languages[languages.Length - 1] };
                var rows = new List<FontAssetAndFallbacks.FontAssetFallbackPairing> { first, last };
                config.m_fallbackCollection = rows;
                config.OnBeforeSerialize();
                Assert.That(first.Name, Is.EqualTo(first.m_language.ToString()));
                Assert.That(last.Name, Is.EqualTo(last.m_language.ToString()));
                Assert.That(ReferenceEquals(config.m_fallbackCollection, rows), Is.True);
                using (var serialized = new SerializedObject(config))
                {
                    serialized.Update();
                    SerializedProperty font = serialized.FindProperty("m_fontAsset");
                    SerializedProperty collection = serialized.FindProperty("m_fallbackCollection");
                    Assert.That(font, Is.Not.Null);
                    Assert.That(font.propertyType, Is.EqualTo(SerializedPropertyType.ObjectReference));
                    Assert.That(font.objectReferenceValue == null, Is.True);
                    Assert.That(collection, Is.Not.Null);
                    Assert.That(collection.isArray, Is.True);
                    Assert.That(collection.arraySize, Is.EqualTo(2));
                    Assert.That(collection.GetArrayElementAtIndex(0).FindPropertyRelative("Name").stringValue,
                        Is.EqualTo(first.m_language.ToString()));
                    Assert.That(collection.GetArrayElementAtIndex(1).FindPropertyRelative("Name").stringValue,
                        Is.EqualTo(last.m_language.ToString()));
                }
                string firstName = first.Name;
                string lastName = last.Name;
                config.OnAfterDeserialize();
                Assert.That(ReferenceEquals(config.m_fallbackCollection, rows), Is.True);
                Assert.That(ReferenceEquals(rows[0], first), Is.True);
                Assert.That(ReferenceEquals(rows[1], last), Is.True);
                Assert.That(first.Name, Is.EqualTo(firstName));
                Assert.That(last.Name, Is.EqualTo(lastName));
            }
            finally { if (config != null) Object.DestroyImmediate(config); }
        }

        [Test]
        public void FontConfigurationNullCollectionAndEntryRetainPartialNamingFaults()
        {
            FontAssetAndFallbacks config = null;
            try
            {
                config = ScriptableObject.CreateInstance<FontAssetAndFallbacks>();
                config.m_fallbackCollection = null;
                Assert.Throws<NullReferenceException>(() => config.OnBeforeSerialize());
                Assert.That(config.m_fallbackCollection, Is.Null);
                var first = new FontAssetAndFallbacks.FontAssetFallbackPairing { Name = "before", m_language = default(Languages) };
                var last = new FontAssetAndFallbacks.FontAssetFallbackPairing { Name = "untouched" };
                var rows = new List<FontAssetAndFallbacks.FontAssetFallbackPairing> { first, null, last };
                config.m_fallbackCollection = rows;
                Assert.Throws<NullReferenceException>(() => config.OnBeforeSerialize());
                Assert.That(first.Name, Is.EqualTo(first.m_language.ToString()));
                Assert.That(last.Name, Is.EqualTo("untouched"));
                Assert.That(ReferenceEquals(config.m_fallbackCollection, rows), Is.True);
                Assert.That(rows.Count, Is.EqualTo(3));
                Assert.That(rows[1], Is.Null);
                config.OnAfterDeserialize();
                Assert.That(rows[1], Is.Null);
                Assert.That(last.Name, Is.EqualTo("untouched"));
            }
            finally { if (config != null) Object.DestroyImmediate(config); }
        }

        [Test]
        public void FontStaticWaitEmptyListYieldsBeforeCompletionAndEarlyDisposeDoesNotPublish()
        {
            var storage = new FSMStorage(0);
            var user = new FSMUser(storage);
            var key = new GraphStorageKey(110010101);
            IEnumerator iterator = null;
            try
            {
                Assert.That(ReferenceEquals(user.Storage, storage), Is.True);
                storage.SetValue(key, false);
                var handles = new List<AsyncOperationHandle>();
                iterator = Wait(user, handles, key);
                Assert.That(storage.GetValue(key, true, false), Is.False);
                Assert.That(iterator.MoveNext(), Is.True);
                Assert.That(iterator.Current, Is.Null);
                Assert.That(storage.GetValue(key, true, false), Is.False);
                Assert.That(iterator.MoveNext(), Is.False);
                Assert.That(storage.GetValue(key, false, false), Is.True);
                Assert.That(handles, Is.Empty);
                ((IDisposable)iterator).Dispose();
                iterator = null;

                storage.SetValue(key, false);
                iterator = Wait(user, new List<AsyncOperationHandle> { default(AsyncOperationHandle) }, key);
                Assert.That(iterator.MoveNext(), Is.True);
                Assert.That(iterator.Current, Is.Null);
                Assert.That(iterator.MoveNext(), Is.True);
                Assert.That(iterator.Current, Is.TypeOf<AsyncOperationHandle>());
                ((IDisposable)iterator).Dispose();
                iterator = null;
                Assert.That(storage.GetValue(key, true, false), Is.False);
            }
            finally
            {
                try { if (iterator is IDisposable disposable) disposable.Dispose(); }
                finally { user.DestroyUser(); Assert.That(ReferenceEquals(user.Storage, storage), Is.True); Assert.That(storage.GetCollection(), Is.Empty); }
            }
        }

        [Test]
        public void FontStaticWaitBoxesInvalidHandlesAndPreservesNullFaultOrder()
        {
            var storage = new FSMStorage(0);
            var user = new FSMUser(storage);
            var key = new GraphStorageKey(110010102);
            IEnumerator iterator = null;
            try
            {
                storage.SetValue(key, false);
                var handles = new List<AsyncOperationHandle> { default(AsyncOperationHandle), default(AsyncOperationHandle) };
                iterator = Wait(user, handles, key);
                Assert.That(iterator.MoveNext(), Is.True);
                Assert.That(iterator.Current, Is.Null);
                for (int index = 0; index < handles.Count; index++)
                {
                    Assert.That(iterator.MoveNext(), Is.True);
                    Assert.That(iterator.Current, Is.TypeOf<AsyncOperationHandle>());
                    Assert.That((AsyncOperationHandle)iterator.Current, Is.EqualTo(handles[index]));
                    Assert.That(storage.GetValue(key, true, false), Is.False);
                }
                Assert.That(iterator.MoveNext(), Is.False);
                Assert.That(storage.GetValue(key, false, false), Is.True);
                Assert.That(handles.Count, Is.EqualTo(2));
                ((IDisposable)iterator).Dispose(); iterator = null;

                storage.SetValue(key, false);
                iterator = Wait(user, null, key);
                Assert.That(iterator.MoveNext(), Is.True);
                Assert.That(iterator.Current, Is.Null);
                Assert.Throws<NullReferenceException>(() => iterator.MoveNext());
                Assert.That(storage.GetValue(key, true, false), Is.False);
                ((IDisposable)iterator).Dispose(); iterator = null;
                iterator = Wait(null, new List<AsyncOperationHandle>(), key);
                Assert.That(iterator.MoveNext(), Is.True);
                Assert.That(iterator.Current, Is.Null);
                Assert.Throws<NullReferenceException>(() => iterator.MoveNext());
                Assert.That(storage.GetValue(key, true, false), Is.False);
            }
            finally
            {
                try { if (iterator is IDisposable disposable) disposable.Dispose(); }
                finally { user.DestroyUser(); Assert.That(ReferenceEquals(user.Storage, storage), Is.True); Assert.That(storage.GetCollection(), Is.Empty); }
            }
        }

        private static IEnumerator Wait(IGraphUser user, List<AsyncOperationHandle> handles, GraphStorageKey key)
        {
            MethodInfo factory = typeof(FontAssetLanguageSetter).GetMethod("WaitForHandlesMarkReady", Declared);
            Assert.That(factory, Is.Not.Null);
            Assert.That(factory.IsPrivate && factory.IsStatic, Is.True);
            return (IEnumerator)factory.Invoke(null, new object[] { user, handles, key });
        }

        private static void AssertMethod(Type owner, string name, Type returns, int flags, Type[] arguments, string[] names)
        {
            MethodInfo method = owner.GetMethod(name, Declared);
            Assert.That(method, Is.Not.Null);
            Assert.That(method.ReturnType, Is.EqualTo(returns));
            Assert.That((int)method.Attributes, Is.EqualTo(flags));
            Assert.That(method.GetMethodImplementationFlags(), Is.EqualTo(MethodImplAttributes.IL));
            Assert.That(method.IsGenericMethod, Is.False);
            ParameterInfo[] parameters = method.GetParameters();
            Assert.That(parameters.Select(p => p.ParameterType), Is.EqualTo(arguments));
            Assert.That(parameters.Select(p => p.Name), Is.EqualTo(names));
            foreach (ParameterInfo parameter in parameters)
            {
                Assert.That(parameter.Attributes, Is.EqualTo(ParameterAttributes.None));
                Assert.That(parameter.HasDefaultValue, Is.False);
                Assert.That(parameter.GetCustomAttributesData(), Is.Empty);
            }
        }

        private static void AssertFields(Type owner, string[] names, Type[] types, int[] flags)
        {
            FieldInfo[] fields = owner.GetFields(Declared).OrderBy(f => f.MetadataToken).ToArray();
            Assert.That(fields.Select(f => f.Name), Is.EqualTo(names));
            Assert.That(fields.Select(f => f.FieldType), Is.EqualTo(types));
            Assert.That(fields.Select(f => (int)f.Attributes), Is.EqualTo(flags));
            foreach (FieldInfo field in fields)
                if (field.Name != "Name" && field.Name != "m_fontAssetsAndFallBackCollectionRefs")
                    Assert.That(field.GetCustomAttributesData(), Is.Empty);
        }

        private static void AssertFieldAttribute(FieldInfo field, Type expected)
        {
            IList<CustomAttributeData> attributes = field.GetCustomAttributesData();
            Assert.That(attributes.Count, Is.EqualTo(1));
            Assert.That(attributes[0].AttributeType, Is.EqualTo(expected));
            Assert.That(attributes[0].Constructor.GetParameters(), Is.Empty);
            Assert.That(attributes[0].ConstructorArguments, Is.Empty);
            Assert.That(attributes[0].NamedArguments, Is.Empty);
        }

        private static void AssertOptions(Type owner, bool serializable, int first, int second)
        {
            Assert.That(owner.IsSerializable, Is.EqualTo(serializable));
            var options = new List<CustomAttributeData>();
            int serializableProjection = 0;
            foreach (CustomAttributeData attribute in owner.GetCustomAttributesData())
            {
                if (attribute.AttributeType == typeof(SerializableAttribute))
                {
                    serializableProjection++;
                    Assert.That(serializable, Is.True);
                    Assert.That(attribute.Constructor.GetParameters(), Is.Empty);
                    Assert.That(attribute.ConstructorArguments, Is.Empty);
                    Assert.That(attribute.NamedArguments, Is.Empty);
                }
                else options.Add(attribute);
            }
            // SerializableAttribute is the exact special-bit projection; stored
            // options retain their full ordered constructor/argument contract.
            Assert.That(serializableProjection, Is.EqualTo(serializable ? 1 : 0));
            Assert.That(options.Count, Is.EqualTo(2));
            int[] expected = { first, second };
            for (int index = 0; index < options.Count; index++)
            {
                CustomAttributeData attribute = options[index];
                Assert.That(attribute.AttributeType, Is.EqualTo(typeof(Il2CppSetOptionAttribute)));
                Assert.That(attribute.Constructor.GetParameters().Select(p => p.ParameterType),
                    Is.EqualTo(new[] { typeof(Option), typeof(object) }));
                Assert.That(attribute.ConstructorArguments.Count, Is.EqualTo(2));
                Assert.That(attribute.ConstructorArguments[0].ArgumentType, Is.EqualTo(typeof(Option)));
                Assert.That(Convert.ToInt32(attribute.ConstructorArguments[0].Value), Is.EqualTo(expected[index]));
                Assert.That(attribute.ConstructorArguments[1].ArgumentType, Is.EqualTo(typeof(bool)));
                Assert.That(attribute.ConstructorArguments[1].Value, Is.EqualTo(false));
                Assert.That(attribute.NamedArguments, Is.Empty);
            }
        }
    }
}
