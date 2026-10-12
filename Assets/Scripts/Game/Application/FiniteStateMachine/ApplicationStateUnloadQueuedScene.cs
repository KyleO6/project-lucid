using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using Hardlight;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace HardlightProject
{
	// Original Game.Runtime 0200012c; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateUnloadQueuedScene : FSMState
	{
		// 0600076f (1.10.1 RVA 0x655230)
		private ApplicationStateUnloadQueuedScene(FiniteStateMachine fsm, string stateName) : base(fsm, stateName)
		{
		}

		// 06000770 (1.10.1 RVA 0x655260)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			return new ApplicationStateUnloadQueuedScene(fsm, stateId);
		}

		// 06000771 (1.10.1 RVA 0x6552E0)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			App app = ProcessManager.GetSystem<App>();
			if (!app.Storage.HasValue<string>(AppFSMKeys.SceneLoadQueued))
			{
				HLOutput.LogError("FSM state ApplicationStateUnloadQueuedScene expects a value under SceneLoadQueued");
				return;
			}
			string sceneName = app.Storage.GetValue<string>(AppFSMKeys.SceneLoadQueued, null, true);
			Dictionary<string, AsyncOperationHandle> loadedLevels = user.Storage.GetValue<Dictionary<string, AsyncOperationHandle>>(AppFSMKeys.LoadedLevels, null, true);
			if (!loadedLevels.TryGetValue(sceneName, out AsyncOperationHandle sceneHandle))
			{
				HLOutput.LogError("Attempting to unload scene " + sceneName + ", not found in App storage");
			}
			else
			{
				AsyncOperationHandle<SceneInstance> operation = AddressableManager.UnloadSceneAndClearResources(sceneHandle);
				app.Storage.SetValue(AppFSMKeys.SceneOpKey, operation);
				loadedLevels.Remove(sceneName);
				user.Storage.SetValue(AppFSMKeys.LoadedLevels, loadedLevels);
			}
			app.Storage.RemoveValue<string>(AppFSMKeys.SceneLoadQueued);
		}
	}
}
