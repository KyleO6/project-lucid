using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 0200006b; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Actor/{0}")]
	[GraphNodeDefaultName("TriggerToggleSSOA")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ActorState_TriggerToggleSSOA : FSMState_Trigger
	{
		[Serializable]
		[FiniteStateMachineJsonCtorArgs]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class ToggleSSOAJSONCtorArgs : TriggerJSONCtorArgs
		{
			public bool Enable;

			// 0600044e (1.10.1 RVA 0x5291A0)
			public ToggleSSOAJSONCtorArgs()
			{
			}
		}

		private readonly bool m_enable;

		// 0600044a (1.10.1 RVA 0x528E10)
		protected ActorState_TriggerToggleSSOA(FiniteStateMachine fsm, FSMIdentifier stateId, ToggleSSOAJSONCtorArgs ctorArgs) : base(fsm, stateId, ctorArgs)
		{
			m_enable = ctorArgs.Enable;
		}

		// 0600044b (1.10.1 RVA 0x528E40)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new ActorState_TriggerToggleSSOA(fsm, transitionId, JsonUtility.FromJson<ToggleSSOAJSONCtorArgs>(jsonCtorArgs));
		}

		// 0600044c (1.10.1 RVA 0x528EE0)
		protected override void InvokeTriggerStart(IGraphUser user, FSMStateChangeAction action)
		{
			IHasVisualProxy visualOwner = user as IHasVisualProxy;
			if (visualOwner == null || visualOwner.VisualProxy == null)
			{
				return;
			}
			visualOwner.VisualProxy.ToggleSSOA(m_enable);
		}

		// 0600044d (1.10.1 RVA 0x529040)
		protected override void InvokeTriggerEnd(IGraphUser user, FSMStateChangeAction action)
		{
			IHasVisualProxy visualOwner = user as IHasVisualProxy;
			if (visualOwner == null || visualOwner.VisualProxy == null)
			{
				return;
			}
			visualOwner.VisualProxy.ToggleSSOA(!m_enable);
		}
	}
}
