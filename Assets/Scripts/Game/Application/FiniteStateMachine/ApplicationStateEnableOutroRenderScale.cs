using Unity.IL2CPP.CompilerServices;
using Hardlight;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace HardlightProject
{
	// Original Game.Runtime 02000157; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateEnableOutroRenderScale : FSMState
	{
		private readonly SystemRef<VisualQualityManager_SDT> m_visualQualityManagerRef;

		// 06000804 (1.10.1 RVA 0x6600F0)
		public ApplicationStateEnableOutroRenderScale(FiniteStateMachine fsm, string stateName) : base(fsm, stateName)
		{
			m_visualQualityManagerRef = ProcessManager.GetSystemRef<VisualQualityManager_SDT>();
		}

		// 06000805 (1.10.1 RVA 0x660190)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new ApplicationStateEnableOutroRenderScale(fsm, transitionId);
		}

		// 06000806 (1.10.1 RVA 0x660200)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			VisualQualityManager_SDT visualQualityManager = m_visualQualityManagerRef.Get();
			if (QualitySettings.GetQualityLevel() != 0)
			{
				visualQualityManager.EnableMenuRenderScale();
			}
			else
			{
				visualQualityManager.EnableGameplayRenderScale();
			}
		}
	}
}
