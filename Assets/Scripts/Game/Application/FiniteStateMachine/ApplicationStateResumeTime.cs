using Unity.IL2CPP.CompilerServices;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 0200011e; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateResumeTime : FSMState
	{
		// 0600074d (1.10.1 RVA 0x653B00)
		public ApplicationStateResumeTime(FiniteStateMachine fsm, string stateName) : base(fsm, stateName)
		{;}

		// 0600074e (1.10.1 RVA 0x653B30)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			return new ApplicationStateResumeTime(fsm, stateId);
		}

		// 0600074f (1.10.1 RVA 0x653BB0)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			TimeManager timeManager = ProcessManager.GetSystemSafe<TimeManager>();
			if (timeManager == null)
			{
				return;
			}
			timeManager.UpdateGameSpeedOverride();
			StackableDataHandle handle = user.Storage.GetValue<StackableDataHandle>(AppFSMKeys.PauseTimeOverride);
			if (handle != null)
			{
				timeManager.RemoveTimeSetting(handle);
				user.Storage.RemoveValue<StackableDataHandle>(AppFSMKeys.PauseTimeOverride);
			}
		}
	}
}
