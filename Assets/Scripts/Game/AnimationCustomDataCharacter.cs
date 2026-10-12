using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000098; bodies translated from the 1.10.1 x86-64 binary.
	[ExecuteInEditMode]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class AnimationCustomDataCharacter : MonoBehaviour
	{
		public float eye_l_u;

		public float eye_l_v;

		public float eye_r_u;

		public float eye_r_v;

		public float glint_l_u;

		public float glint_l_v;

		public float glint_r_u;

		public float glint_r_v;

		// 0600051b (1.10.1 RVA 0x52E580)
		public AnimationCustomDataCharacter()
		{
		}
	}
}
