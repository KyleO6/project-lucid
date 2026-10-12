using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020003a2; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeDefaultName("TriggerResetAirAbilities")]
	[GraphNodeMenuFormat("Character/Trigger/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterState_TriggerResetAirAbilities : FSMState_Trigger
	{
		// 06001635 (1.10.1 RVA 0x783890)
		protected CharacterState_TriggerResetAirAbilities(FiniteStateMachine fsm, FSMIdentifier stateId, TriggerJSONCtorArgs ctorArgs) : base(fsm, stateId, ctorArgs)
		{
			
		}

		// 06001636 (1.10.1 RVA 0x7838A0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new CharacterState_TriggerResetAirAbilities(fsm, transitionId, JsonUtility.FromJson<TriggerJSONCtorArgs>(jsonCtorArgs));
		}

		// 06001637 (1.10.1 RVA 0x783930)
		protected override void InvokeTriggerStart(IGraphUser user, FSMStateChangeAction action)
		{
			user.GetAs<Character>().GetAbilitiesOfType<CharacterAbility_Air>(ability => ability.Reset(false, true));
		}
	}
}
