using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 0200006d; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeDefaultName("TriggerVelocityImpulse")]
	[GraphNodeMenuFormat("Actor/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ActorState_TriggerVelocityImpulse : FSMState_Trigger
	{
		[Serializable]
		[FiniteStateMachineJsonCtorArgs]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class VelocityImpulseJSONCtorArgs : TriggerJSONCtorArgs
		{
			public float ImpulseForce;

			public string OptionalVariableName;

			// 06000452 (1.10.1 RVA 0x5294E0)
			public VelocityImpulseJSONCtorArgs()
			{
			}
		}

		private readonly float m_impulseForce;

		private readonly GraphStorageKey m_optionalVarName;

		private readonly bool m_usingFSMVariable;

		// 0600044f (1.10.1 RVA 0x5291B0)
		protected ActorState_TriggerVelocityImpulse(FiniteStateMachine fsm, FSMIdentifier stateId, VelocityImpulseJSONCtorArgs ctorArgs) : base(fsm, stateId, ctorArgs)
		{
			m_impulseForce = ctorArgs.ImpulseForce;
			m_usingFSMVariable = !string.IsNullOrEmpty(ctorArgs.OptionalVariableName);
			if (m_usingFSMVariable)
			{
				m_optionalVarName = new GraphStorageKey(ConvertNameToId(ctorArgs.OptionalVariableName));
			}
		}

		// 06000450 (1.10.1 RVA 0x529220)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new ActorState_TriggerVelocityImpulse(fsm, transitionId, JsonUtility.FromJson<VelocityImpulseJSONCtorArgs>(jsonCtorArgs));
		}

		// 06000451 (1.10.1 RVA 0x529310)
		protected override void InvokeTriggerStart(IGraphUser user, FSMStateChangeAction action)
		{
			Actor actor = user.GetAs<Actor>();
			Vector3 velocity = m_usingFSMVariable
				? user.Storage.GetValue(m_optionalVarName, Vector3.zero, true)
				: actor.WorldVelocity + actor.WorldUp * m_impulseForce;
			actor.SetWorldVelocity(velocity);
			actor.OrientateHeadingToForward();
		}
	}
}
