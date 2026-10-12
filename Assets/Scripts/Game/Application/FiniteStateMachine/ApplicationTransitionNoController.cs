using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using HardlightProject;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Hardlight
{
	// Original Game.Runtime 0200000e; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public sealed class ApplicationTransitionNoController : FSMTransition
	{
		private readonly SystemRef<CutsceneManager> m_cutsceneManagerRef;

		// 06000020 (1.10.1 RVA 0x507390)
		private ApplicationTransitionNoController(FiniteStateMachine fsm, FSMIdentifier transitionId) : base(fsm, transitionId)
		{
			m_cutsceneManagerRef = ProcessManager.GetSystemRef<CutsceneManager>();
		}

		// 06000021 (1.10.1 RVA 0x507430)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new ApplicationTransitionNoController(fsm, transitionId);
		}

		// 06000022 (1.10.1 RVA 0x507500)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext context)
		{
			if (m_cutsceneManagerRef.IsValid() && m_cutsceneManagerRef.Get().IsCutscenePlaying)
			{
				return false;
			}
			Dictionary<string, AsyncOperationHandle> loadedLevels = user.Storage.GetValue(AppFSMKeys.LoadedLevels, new Dictionary<string, AsyncOperationHandle>(), true);
			if (loadedLevels.ContainsKey("s_intro_video"))
			{
				return false;
			}
			HLInputModule inputModule = (HLInputModule)EventSystem.current.currentInputModule;
			return !user.GetAs<App>().ValidateHasAtLeastOneController(inputModule);
		}
	}
}
