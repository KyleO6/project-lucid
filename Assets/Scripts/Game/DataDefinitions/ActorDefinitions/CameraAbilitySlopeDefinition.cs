using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000465; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CameraAbilitySlopeDefinition : CameraAbilityDefinition
	{
		[Tooltip("Min/max angle ranges from 0 inclusive to 360 exclusive.")]
		public List<Vector2> AngleRanges;

		// 060019b7 (1.10.1 RVA 0x7A8CD0)
		public CameraAbilitySlopeDefinition()
		{
		}
	}
}
