using Unity.IL2CPP.CompilerServices;
using System;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020004fd; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "CharacterAbilityDefinition_Rolling", menuName = "HardlightProject/DefinitionData/AbilityDef/Rolling", order = 1)]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterAbilityDefinition_Rolling : CharacterAbilityDefinition_MovementGround
	{
		[Tooltip("When speed falls below threshold, rolling state is exited after specified time.")]
		public float AutoExitSpeedThreshold = 5f;

		[Tooltip("When speed is above threshold, ignore ledge braking.")]
		public float LedgeBrakingSpeedThreshold = 50f;

		public override Type ScriptType
		{
			get
			{
				return typeof(CharacterAbility_Rolling);
			}
		}

		// 06001b86 (1.10.1 RVA 0x545160)
		public CharacterAbilityDefinition_Rolling()
		{
		}
	}
}
