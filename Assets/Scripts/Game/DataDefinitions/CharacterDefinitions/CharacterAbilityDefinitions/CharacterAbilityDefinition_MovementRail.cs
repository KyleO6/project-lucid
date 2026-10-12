using Unity.IL2CPP.CompilerServices;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020004f0; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public abstract class CharacterAbilityDefinition_MovementRail : CharacterAbilityDefinition_MovementTracker
	{
		[Tooltip("Minimum time to stay in the state.")]
		[Min(0f)]
		public float StateTimeMin;

		[Tooltip("Minimum initial speed on attachment.")]
		[Min(0f)]
		public float AttachSpeedMin = 35f;

		[Tooltip("Input threshold to activate turn around transition.")]
		[Range(0f, 1f)]
		public float TurnAroundInputThreshold = 0.5f;

		[Min(0f)]
		[Tooltip("Input timer to activate turn around transition.")]
		public float TurnAroundInputTime = 1f;

		[Tooltip("Deceleration during turn around transition.")]
		[Min(0f)]
		public float TurnAroundDeceleration = 50f;

		[Min(0f)]
		[Tooltip("Speed threshold to activate turn around.")]
		public float TurnAroundSpeedThreshold = 1f;

		[Tooltip("Angle off gravity threshold to activate jump.")]
		[Min(0f)]
		public float JumpAngleOffGravityThreshold = 45f;

		[Tooltip("Magnetism gravity multiplier at increasing distances from the rail.")]
		public AnimationCurve JumpMagnetism;

		[Tooltip("Max distance character is allowed away from the rail when jumping.")]
		public float JumpDistanceMax = 4f;

		[Tooltip("Max jump angle to rail for applying gravity, otherwise magnetism kicks in.")]
		public float JumpOnRailAngleMax = 35f;

		[Min(0f)]
		[Tooltip("Distance from exit to snap character to knot's transform.")]
		public float JumpDistanceToSnapToExit;

		public float JumpAngleCosineOffGravityThreshold { get; private set; }

		public float JumpOnRailAngleCosineMax { get; private set; }

		// 06001b62 (1.10.1 RVA 0x544690)
		public override void CalculateCachedValues()
		{
			base.CalculateCachedValues();
			JumpAngleCosineOffGravityThreshold = Mathf.Cos(JumpAngleOffGravityThreshold * Mathf.Deg2Rad);
			JumpOnRailAngleCosineMax = Mathf.Cos(Mathf.Deg2Rad * JumpOnRailAngleMax);
		}

		// 06001b63 (1.10.1 RVA 0x5446F0)
		protected CharacterAbilityDefinition_MovementRail()
		{
		}
	}
}
