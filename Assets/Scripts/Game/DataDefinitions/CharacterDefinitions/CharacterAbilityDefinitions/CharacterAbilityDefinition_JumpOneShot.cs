using Unity.IL2CPP.CompilerServices;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020004e0; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "CharacterAbilityDefinition_JumpOneShot", menuName = "HardlightProject/DefinitionData/AbilityDef/JumpOneShot", order = 1)]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterAbilityDefinition_JumpOneShot : CharacterAbilityDefinition_Jump
	{
		[Tooltip("Period of time to allow a queued activation before being enabled.")]
		public float TriggerTimeBefore;

		[Tooltip("Abilities allowed to precede one-shot boost being triggered.")]
		public List<ActorAbilityType> TriggerAbilitiesBefore;

		[Tooltip("Abilities allowed to trigger one-shot boost dependant on preceding being valid.")]
		public List<ActorAbilityType> TriggerAbilitiesConditional;

		[Tooltip("Abilities allowed to trigger one-shot boost that ignore any preceding.")]
		public List<ActorAbilityType> TriggerAbilitiesUnique;

		[Tooltip("Period of time to allow a queued activation after being enabled.")]
		public float TriggerTimeAfter;

		public override Type ScriptType
		{
			get
			{
				return typeof(CharacterAbility_MovementJumpOneShot);
			}
		}

		// 06001b2f (1.10.1 RVA 0x543AB0)
		public bool CanTrigger(ActorAbilityType abilityTypeBefore, ActorAbilityType abilityType)
		{
			if (TriggerAbilitiesBefore.Contains(abilityTypeBefore) && TriggerAbilitiesConditional.Contains(abilityType))
			{
				return true;
			}
			return TriggerAbilitiesUnique.Contains(abilityType);
		}

		// 06001b30 (1.10.1 RVA 0x543B40)
		public CharacterAbilityDefinition_JumpOneShot()
		{
		}
	}
}
