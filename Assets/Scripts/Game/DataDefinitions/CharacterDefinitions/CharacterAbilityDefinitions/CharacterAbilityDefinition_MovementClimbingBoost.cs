using Unity.IL2CPP.CompilerServices;
using System;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020004e8; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "CharacterAbilityDefinition_MovementClimbingBoost", menuName = "HardlightProject/DefinitionData/AbilityDef/MovementClimbingBoost", order = 1)]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterAbilityDefinition_MovementClimbingBoost : CharacterAbilityDefinition_Climbing
	{
		public CharacterStamina.Definition BoostDefinition;

		public override Type ScriptType
		{
			get
			{
				return typeof(CharacterAbility_MovementClimbingBoost);
			}
		}

		// 06001b44 (1.10.1 RVA 0x544050)
		public override bool TryGetBoostDefinition(out CharacterStamina.Definition boostDefinition)
		{
			boostDefinition = BoostDefinition;
			return true;
		}

		// 06001b45 (1.10.1 RVA 0x544070)
		public override void CalculateCachedValues()
		{
			base.CalculateCachedValues();
			BoostDefinition.CalculateCachedValues();
		}

		// 06001b46 (1.10.1 RVA 0x5440F0)
		public CharacterAbilityDefinition_MovementClimbingBoost()
		{
		}
	}
}
