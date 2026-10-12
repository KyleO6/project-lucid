using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 02000170; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationTransitionFastLoader : FSMTransition
	{
		// 06000847 (1.10.1 RVA 0x6632A0)
		public ApplicationTransitionFastLoader(FiniteStateMachine fsm, FSMIdentifier transitionId) : base(fsm, transitionId)
		{
			
		}

		// 06000848 (1.10.1 RVA 0x6632B0)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new ApplicationTransitionFastLoader(fsm, transitionId);
		}

		// 06000849 (1.10.1 RVA 0x663310)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
		}

		// 0600084a (1.10.1 RVA 0x663320)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext updateContext)
		{
			return user.Storage.GetValue(AppFSMKeys.IsFastLoading, false, true);
		}
	}
}
