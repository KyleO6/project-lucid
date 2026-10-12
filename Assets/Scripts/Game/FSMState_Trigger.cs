using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 02000686; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public abstract class FSMState_Trigger : FSMState
	{
		protected enum EventType
		{
			EnterStart,
			LeaveStart,
			EnterStartLeaveEnd
		}

		[Serializable]
		[FiniteStateMachineJsonCtorArgs]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class TriggerJSONCtorArgs
		{
			public EventType ActiveEventType;

			// 0600233a (1.10.1 RVA 0x577EC0)
			public TriggerJSONCtorArgs()
			{;}
		}

		protected readonly EventType m_activeEventType;

		// 06002335 (1.10.1 RVA 0x577D50)
		protected FSMState_Trigger(FiniteStateMachine fsm, FSMIdentifier stateId, TriggerJSONCtorArgs ctorArgs) : base(fsm, stateId)
		{
			m_activeEventType = ctorArgs.ActiveEventType;
		}

		// 06002336 (1.10.1 RVA 0x584D50)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			if (m_activeEventType == EventType.EnterStart || m_activeEventType == EventType.EnterStartLeaveEnd)
			{
				InvokeTriggerStart(user, action);
			}
		}

		// 06002337 (1.10.1 RVA 0x584DB0)
		protected override void DoOnLeave(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnLeave(user, action);
			if (m_activeEventType == EventType.LeaveStart)
			{
				InvokeTriggerStart(user, action);
			}
			else if (m_activeEventType == EventType.EnterStartLeaveEnd)
			{
				InvokeTriggerEnd(user, action);
			}
		}

		protected abstract void InvokeTriggerStart(IGraphUser user, FSMStateChangeAction action);

		// 06002339 (1.10.1 RVA 0x584E30)
		protected virtual void InvokeTriggerEnd(IGraphUser user, FSMStateChangeAction action)
		{;}
	}
}
