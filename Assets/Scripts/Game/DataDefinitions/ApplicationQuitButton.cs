using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 0200082d; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationQuitButton : MonoBehaviour
	{
		// 06002f7c (1.10.1 RVA 0x606B60)
		public void Action_Quit()
		{
			Application.Quit(0);
		}

		// 06002f7d (1.10.1 RVA 0x606BC0)
		public ApplicationQuitButton()
		{
		}
	}
}
