using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 02000155; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateEnableGameplayRenderScale : FSMState
	{
		private readonly SystemRef<VisualQualityManager_SDT> m_visualQualityManagerRef = ProcessManager.GetSystemRef<VisualQualityManager_SDT>();

		// 060007fe (1.10.1 RVA 0x65FD80)
		public ApplicationStateEnableGameplayRenderScale(FiniteStateMachine fsm, string stateName) : base(fsm, stateName)
		{
		}

		// 060007ff (1.10.1 RVA 0x65FE20)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new ApplicationStateEnableGameplayRenderScale(fsm, transitionId);
		}

		// 06000800 (1.10.1 RVA 0x65FE90)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			m_visualQualityManagerRef.Get().EnableGameplayRenderScale();
		}
	}
}
