using Unity.IL2CPP.CompilerServices;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 0200075f; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class DirectionInfluenceData
	{
		public Vector3 TargetVelocity;

		public Vector3 CurrentVelocity;

		public float CurrentBlendTimeSeconds;

		public StackableDataHandle InfluenceModifierHandle;

		public StackableDataHandle MaxVelocityModifierHandle;

		// 06002944 (1.10.1 RVA 0x5CA150)
		public DirectionInfluenceData()
		{
		}
	}
}
