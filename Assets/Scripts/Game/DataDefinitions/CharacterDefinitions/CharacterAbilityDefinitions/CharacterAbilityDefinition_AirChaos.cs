using Unity.IL2CPP.CompilerServices;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020004b5; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public abstract class CharacterAbilityDefinition_AirChaos : CharacterAbilityDefinition_Air
	{
		[Tooltip("The time categories to animate over time.")]
		public List<TimeCategory> m_timeCategories = new List<TimeCategory>();

		[Tooltip("Delay before time scale animation is triggered.")]
		public float m_timeScaleDelay;

		[Tooltip("The time category scale animation over time.")]
		public AnimationCurve m_timeScaleAnimation = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 0f));

		// 06001ac0 (1.10.1 RVA 0x541060)
		protected CharacterAbilityDefinition_AirChaos()
		{
		}
	}
}
