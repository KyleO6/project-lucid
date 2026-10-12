using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020004f7; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public abstract class CharacterAbilityDefinition_Rails : CharacterAbilityDefinition
	{
		[Tooltip("Period of time, when active, between evaluating the current target.")]
		public float TargetingIntervalSeconds = 0.1f;

		[Tooltip("Period of time, after leaving ability, to evaluate the current target.")]
		public float CooldownTimeSeconds = 0.1f;

		// 06001b6f (1.10.1 RVA 0x544A40)
		protected CharacterAbilityDefinition_Rails()
		{
		}
	}
}
