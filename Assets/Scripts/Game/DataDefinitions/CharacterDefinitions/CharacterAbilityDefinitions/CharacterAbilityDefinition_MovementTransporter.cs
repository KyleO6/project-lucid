using Unity.IL2CPP.CompilerServices;
using System;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020004f6; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "CharacterAbilityDefinition_MovementTransporter", menuName = "HardlightProject/DefinitionData/AbilityDef/MovementTransporter", order = 1)]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterAbilityDefinition_MovementTransporter : CharacterAbilityDefinition_MovementTracker
	{
		public override Type ScriptType
		{
			get
			{
				return typeof(CharacterAbility_MovementTransporter);
			}
		}

		// 06001b6e (1.10.1 RVA 0x544A20)
		public CharacterAbilityDefinition_MovementTransporter()
		{
		}
	}
}
