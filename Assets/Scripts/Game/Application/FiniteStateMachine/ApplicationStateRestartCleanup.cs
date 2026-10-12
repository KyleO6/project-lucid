using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 0200014f; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateRestartCleanup : FSMState
	{
		// 060007eb (1.10.1 RVA 0x65F0A0)
		private ApplicationStateRestartCleanup(FiniteStateMachine fsm, string stateName) : base(fsm, stateName)
		{;}

		// 060007ec (1.10.1 RVA 0x65F0D0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			return new ApplicationStateRestartCleanup(fsm, stateId);
		}

		// 060007ed (1.10.1 RVA 0x65F150)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			user.Storage.RemoveValue<bool>(AppFSMKeys.RestartInProgress);
		}
	}
}
