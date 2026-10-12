using Unity.IL2CPP.CompilerServices;
using System;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020004e7; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "CharacterAbilityDefinition_MovementClimbing", menuName = "HardlightProject/DefinitionData/AbilityDef/MovementClimbing", order = 1)]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterAbilityDefinition_MovementClimbing : CharacterAbilityDefinition_Climbing
	{
		public override Type ScriptType
		{
			get
			{
				return typeof(CharacterAbility_MovementClimbing);
			}
		}

		// 06001b42 (1.10.1 RVA 0x543F90)
		public CharacterAbilityDefinition_MovementClimbing()
		{
		}
	}
}
