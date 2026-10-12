using Unity.IL2CPP.CompilerServices;
using System;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020004df; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "CharacterAbilityDefinition_Jump", menuName = "HardlightProject/DefinitionData/AbilityDef/Jump", order = 1)]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterAbilityDefinition_Jump : CharacterAbilityDefinition_Movement
	{
		[Tooltip("Initial velocity added in the jump direction.")]
		public float JumpInitialVelocity;

		[Tooltip("Initial velocity minimum world up speed.")]
		public float JumpInitialVelocityUpMin;

		[Tooltip("The maximum power force applied to jumping.")]
		public float JumpPower;

		[Tooltip("Evaluated by Time Elapsed/Jump Time x Jump Power.")]
		public AnimationCurve JumpPowerCurve;

		[Tooltip("How long the jump button can be held down for.")]
		public float MaximumJumpTimeSeconds;

		[Tooltip("Character will jump for at least this many seconds regardless of button press duration.")]
		public float MinimumJumpTimeSeconds;

		[Tooltip("Delay the jump by this many seconds to allow for animation.")]
		public float JumpDelaySeconds;

		public override Type ScriptType
		{
			get
			{
				return typeof(CharacterAbility_MovementJump);
			}
		}

		// 06001b2d (1.10.1 RVA 0x543A10)
		public CharacterAbilityDefinition_Jump()
		{
		}
	}
}
