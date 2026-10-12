using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 0200078c; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ParticleSystemPrefabPool : ComponentPrefabPool<ParticleSystem>
	{
		// 06002a48 (1.10.1 RVA 0x5D7BB0)
		public ParticleSystemPrefabPool()
		{
		}
	}
}
