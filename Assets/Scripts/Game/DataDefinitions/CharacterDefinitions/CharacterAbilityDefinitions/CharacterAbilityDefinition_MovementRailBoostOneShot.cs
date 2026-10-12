using Unity.IL2CPP.CompilerServices;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020004f3; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "CharacterAbilityDefinition_MovementRailBoostOneShot", menuName = "HardlightProject/DefinitionData/AbilityDef/MovementRailBoostOneShot", order = 1)]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterAbilityDefinition_MovementRailBoostOneShot : CharacterAbilityDefinition_MovementRailBoostBase
	{
		[Tooltip("Period of time to allow a queued activation before being enabled.")]
		public float TriggerTimeBefore;

		[Tooltip("Abilities allowed to precede one-shot boost being triggered.")]
		public List<ActorAbilityType> TriggerAbilitiesBefore;

		[Tooltip("Abilities allowed to trigger one-shot boost.")]
		public List<ActorAbilityType> TriggerAbilities;

		[Tooltip("Period of time to allow a queued activation after being enabled.")]
		public float TriggerTimeAfter;

		public override Type ScriptType
		{
			get
			{
				return typeof(CharacterAbility_MovementRailBoostOneShot);
			}
		}

		// 06001b69 (1.10.1 RVA 0x5448C0)
		public CharacterAbilityDefinition_MovementRailBoostOneShot()
		{
		}
	}
}
