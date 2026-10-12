using Unity.IL2CPP.CompilerServices;
using System;
using System.Collections.Generic;
using Hardlight;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace HardlightProject
{
	// Original Game.Runtime 02000ad5; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public sealed class UnloadSceneAfterFirstBoot : MonoBehaviour
	{
		[SerializeField]
		private bool m_isAddressable;

		private readonly SystemRef<HLPropertyStore> m_propertyStoreRef = ProcessManager.GetSystemRef<HLPropertyStore>();

		private bool m_forceUnload;

		// 06003e80 (1.10.1 RVA 0x6E9CB0)
		public void Action_Unload(bool forceUnload = true)
		{
			m_forceUnload = forceUnload;
			HLPropertyStore.AddLoadHandler(OnPropertyStoreLoaded);
		}

		// 06003e81 (1.10.1 RVA 0x6E9D50)
		private void OnPropertyStoreLoaded(HLPropertyList properties, bool isNewFile)
		{
			if (m_propertyStoreRef.Get().GetSaveIdentifier() != "HL_Default_Save_ID")
			{
				return;
			}
			HLPropertyStore.RemoveLoadHandler(OnPropertyStoreLoaded);
			if (properties.AsBool("HL_First_Boot", true) && !m_forceUnload)
			{
				return;
			}
			Scene scene = gameObject.scene;
			if (m_isAddressable)
			{
				UnloadAddressableScene(scene);
			}
			else
			{
				SceneManager.UnloadSceneAsync(scene);
			}
		}

		// 06003e82 (1.10.1 RVA 0x6E9ED0)
		private static void UnloadAddressableScene(Scene scene)
		{
			App app = ProcessManager.GetSystem<App>();
			if (app.Storage.TryGetValue(AppFSMKeys.LoadedLevels, out Dictionary<string, AsyncOperationHandle> loadedLevels) && loadedLevels.TryGetValue(scene.name, out AsyncOperationHandle handle))
			{
				Addressables.UnloadSceneAsync(handle, true);
				loadedLevels.Remove(scene.name);
				app.Storage.SetValue(AppFSMKeys.LoadedLevels, loadedLevels);
			}
		}

		// 06003e83 (1.10.1 RVA 0x6EA150)
		public UnloadSceneAfterFirstBoot()
		{
		}
	}
}
