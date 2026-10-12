using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 0200078b; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ParticleSystemInstancePrefabPool : InstancePrefabPool<ParticleSystem>
	{
		// 06002a47 (1.10.1 RVA 0x5D7B70)
		public ParticleSystemInstancePrefabPool()
		{
		}
	}
}
