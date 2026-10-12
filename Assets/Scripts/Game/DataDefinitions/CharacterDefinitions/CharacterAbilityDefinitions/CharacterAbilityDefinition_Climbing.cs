using Unity.IL2CPP.CompilerServices;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020004c5; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public abstract class CharacterAbilityDefinition_Climbing : CharacterAbilityDefinition
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		public class ClimbingLateralMotion
		{
			[Tooltip("Lateral acceleration, which is scaled by input magnitude.")]
			public float Acceleration = 10f;

			[Tooltip("Maximum speed that can be accelerated to with control input.")]
			public float SpeedMax = 5f;

			[Tooltip("Deceleration when lateral control input is neutral or speed is over max.")]
			public float Deceleration = 10f;

			[Tooltip("Scale the vertical speed based on how close to SpeedMax the current lateral speed is. x-axis: ratio of current lateral speed to SpeedMax, where 1 means the current speed is >= SpeedMax. y-axis: multiplier to apply to vertical speed limit.")]
			public AnimationCurve VerticalLateralSpeedBias;

			// 06001aff (1.10.1 RVA 0x542D30)
			public ClimbingLateralMotion()
			{
			}
		}

		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		public class ClimbingVerticalMotion
		{
			[Tooltip("Vertical acceleration, which is scaled by input magnitude.")]
			public float Acceleration = 10f;

			[Tooltip("Maximum speed that can be accelerated to with control input in the upward direction.")]
			public float SpeedMaxUpward = 5f;

			[Tooltip("Maximum speed that can be accelerated to with control input in the downward direction.")]
			public float SpeedMaxDownward = 5f;

			[Tooltip("Deceleration when vertical control input is neutral or speed is over max.")]
			public float Deceleration = 10f;

			// 06001b00 (1.10.1 RVA 0x542D50)
			public ClimbingVerticalMotion()
			{
			}
		}

		[Tooltip("Layers to check for climbing. Colliders must also have the climbing metaData set.")]
		[Header("Climbing angle restrictions")]
		public LayerMask TrackMask = -1;

		[Tooltip("Don't climb further when angle exceeds this.")]
		public float MaxClimbAngle = 120f;

		[Tooltip("Don't climb further when angle is less than this.")]
		public float MinClimbAngle = 60f;

		[Tooltip("Prevent climbing from one face to another where the change in angle is greater than this.")]
		[Range(0f, 90f)]
		public float MaxAngleChange = 90f;

		[Tooltip("Values controlling side-to-side movement speed.")]
		[Header("Climbing motion")]
		public ClimbingLateralMotion LateralMotion;

		[Tooltip("Values controlling up and down movement speed.")]
		public ClimbingVerticalMotion VerticalMotion;

		[Tooltip("Jump angle (degrees), added to surface normal in direction away from gravity.")]
		[Header("Jumping")]
		[Range(-90f, 90f)]
		public float JumpAngleExtra;

		[Header("Other")]
		[Tooltip("Rotation speed (degrees per second) for character body when moving to different surface normals.")]
		[Min(0f)]
		public float RotationSpeed = 360f;

		public float MaxClimbAngleCosine { get; private set; }

		public float MinClimbAngleCosine { get; private set; }

		public float MaxAngleChangeCosine { get; private set; }

		// 06001afd (1.10.1 RVA 0x542C60)
		public override void CalculateCachedValues()
		{
			base.CalculateCachedValues();
			MaxClimbAngleCosine = Mathf.Cos(MaxClimbAngle * Mathf.Deg2Rad);
			MinClimbAngleCosine = Mathf.Cos(MinClimbAngle * Mathf.Deg2Rad);
			MaxAngleChangeCosine = Mathf.Cos(MaxAngleChange * Mathf.Deg2Rad);
		}

		// 06001afe (1.10.1 RVA 0x542CD0)
		protected CharacterAbilityDefinition_Climbing()
		{
		}
	}
}
