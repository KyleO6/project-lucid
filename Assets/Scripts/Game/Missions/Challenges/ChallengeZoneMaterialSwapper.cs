using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000773; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ChallengeZoneMaterialSwapper : MonoBehaviour
	{
		[SerializeField]
		private ChallengeZoneIdentifier m_identifier;

		// 060029e6 (1.10.1 RVA 0x5D4AB0)
		public ChallengeZoneMaterialSwapper()
		{
		}
	}
}
