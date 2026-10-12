using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020003a4; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Character/Trigger/{0}")]
	[GraphNodeDefaultName("TriggerResetAirBuffers")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterState_TriggerResetAirBuffers : FSMState_Trigger
	{
		// 0600163b (1.10.1 RVA 0x783B10)
		protected CharacterState_TriggerResetAirBuffers(FiniteStateMachine fsm, FSMIdentifier stateId, TriggerJSONCtorArgs ctorArgs) : base(fsm, stateId, ctorArgs)
		{
			
		}

		// 0600163c (1.10.1 RVA 0x783B20)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new CharacterState_TriggerResetAirBuffers(fsm, transitionId, JsonUtility.FromJson<TriggerJSONCtorArgs>(jsonCtorArgs));
		}

		// 0600163d (1.10.1 RVA 0x783BB0)
		protected override void InvokeTriggerStart(IGraphUser user, FSMStateChangeAction action)
		{
			user.GetAs<Character>().GetAbilitiesOfType<CharacterAbility_MovementAir>(ability => ability.ResetBuffers(false));
		}
	}
}
