using Unity.IL2CPP.CompilerServices;
using System;
using System.Collections.Generic;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000069; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeDefaultName("TriggerRestoreStoredVelocity")]
	[GraphNodeMenuFormat("Actor/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ActorState_TriggerRestoreStoredVelocity : FSMState_Trigger
	{
		[Serializable]
		[FiniteStateMachineJsonCtorArgs]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class RestoreVelocityJSONCtorArgs : TriggerJSONCtorArgs
		{
			public ActorAbilityType AbilityType;

			public float TimeSinceAbilityActiveSeconds;

			// 06000449 (1.10.1 RVA 0x528E00)
			public RestoreVelocityJSONCtorArgs()
			{
			}
		}

		private readonly ActorAbilityType m_abilityType;

		private readonly float m_timeSinceAbilityActiveSeconds;

		// 06000446 (1.10.1 RVA 0x5288A0)
		protected ActorState_TriggerRestoreStoredVelocity(FiniteStateMachine fsm, FSMIdentifier stateId, RestoreVelocityJSONCtorArgs ctorArgs) : base(fsm, stateId, ctorArgs)
		{
			m_abilityType = ctorArgs.AbilityType;
			m_timeSinceAbilityActiveSeconds = ctorArgs.TimeSinceAbilityActiveSeconds;
		}

		// 06000447 (1.10.1 RVA 0x5288D0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new ActorState_TriggerRestoreStoredVelocity(fsm, transitionId, JsonUtility.FromJson<RestoreVelocityJSONCtorArgs>(jsonCtorArgs));
		}

		// 06000448 (1.10.1 RVA 0x528980)
		protected override void InvokeTriggerStart(IGraphUser user, FSMStateChangeAction action)
		{
			Actor actor = user.GetAs<Actor>();
			if (!actor.TryGetAbility(m_abilityType, out ActorAbility ability))
				return;
			if (m_timeSinceAbilityActiveSeconds > 0f)
			{
				Dictionary<ActorAbilityType, ActorAbilityInUseData> abilities = actor.Storage.GetValue<Dictionary<ActorAbilityType, ActorAbilityInUseData>>(ActorFSMKeys.AbilitiesInUse, null, true);
				if (!abilities.TryGetValue(m_abilityType, out ActorAbilityInUseData data) || actor.GetTotalFixedTime() - data.TimeLastUsedSeconds > m_timeSinceAbilityActiveSeconds)
					return;
			}
			if (ability is IVelocityStore velocityStore)
				velocityStore.VelocityStorage.RestoreStoredVelocity();
		}
	}
}
