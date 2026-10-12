using Unity.IL2CPP.CompilerServices;
using System;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020004cc; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public abstract class CharacterAbilityDefinition_HalfPipe : CharacterAbilityDefinition
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		public class HalfPipeGravity
		{
			[Tooltip("Multiplies the character's current gravity whilst ascending.")]
			public float GravityMultiplierAscending;

			[Tooltip("Multiplies the character's current gravity whilst descending.")]
			public float GravityMultiplierDescending;

			[Tooltip("Maximum attainable speed when descending.")]
			public float TerminalSpeed;

			// 06001b0a (1.10.1 RVA 0x543040)
			public HalfPipeGravity()
			{
			GravityMultiplierAscending = 1.0f;
			GravityMultiplierDescending = 1.0f;
			TerminalSpeed = 1e+02f;
		}
		}

		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		public class HalfPipeGravityOverride
		{
			[Tooltip("When vertical speed falls below this threshold, this override will be used.")]
			public float VerticalSpeedThreshold;

			[Tooltip("Gravity override settings to be used.")]
			public HalfPipeGravity Gravity;

			// 06001b0b (1.10.1 RVA 0x543060)
			public HalfPipeGravityOverride()
			{
			}
		}

		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		public class HalfPipeMotion
		{
			[Tooltip("Default gravity settings.")]
			public HalfPipeGravity Gravity;

			[Tooltip("List of gravity overrides determined by current vertical speed.")]
			public HalfPipeGravityOverride[] GravityOverrides;

			// 06001b0c (1.10.1 RVA 0x543070)
			public HalfPipeMotion()
			{
			}
		}

		[Tooltip("Input required to engage a lean to exit.")]
		public float ExitLeanDeadzone;

		[Tooltip("Lean time to exit.")]
		public float ExitLeanTime;

		[Tooltip("Initial period of time that an air activation is not valid to trigger exit.")]
		public float ExitAirTimeThreshold;

		[Tooltip("The offset distance to nudge the character away from the tracked collider.  This helps on landing to not collide with low resolution colliders.")]
		public float OffsetFromTracker = 0.1f;

		// 06001b09 (1.10.1 RVA 0x543020)
		protected CharacterAbilityDefinition_HalfPipe()
		{
		}
	}
}
