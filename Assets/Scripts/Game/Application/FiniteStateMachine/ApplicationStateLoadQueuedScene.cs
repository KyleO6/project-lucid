using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using Hardlight;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace HardlightProject
{
	// Original Game.Runtime 020000eb; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateLoadQueuedScene : FSMState
	{
		// 060006c1 (1.10.1 RVA 0x64ACB0)
		private ApplicationStateLoadQueuedScene(FiniteStateMachine fsm, string stateName) : base(fsm, stateName)
		{
		}

		// 060006c2 (1.10.1 RVA 0x64ACE0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			return new ApplicationStateLoadQueuedScene(fsm, stateId);
		}

		// 060006c3 (1.10.1 RVA 0x64AD60)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			App app = ProcessManager.GetSystem<App>();
			if (!app.Storage.HasValue<string>(AppFSMKeys.SceneLoadQueued))
			{
				HLOutput.LogError("FSM state ApplicationStateLoadQueuedScene expects a value under SceneLoadQueued");
				return;
			}
			string sceneName = app.Storage.GetValue<string>(AppFSMKeys.SceneLoadQueued, null, true);
			Dictionary<string, AsyncOperationHandle> loadedLevels = user.Storage.GetValue<Dictionary<string, AsyncOperationHandle>>(AppFSMKeys.LoadedLevels, null, true);
			AsyncOperationHandle<SceneInstance> operation = Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Additive, true, 100);
			loadedLevels[sceneName] = operation;
			user.Storage.SetValue(AppFSMKeys.LoadedLevels, loadedLevels);
			app.Storage.SetValue(AppFSMKeys.SceneOpKey, operation);
		}
	}
}
