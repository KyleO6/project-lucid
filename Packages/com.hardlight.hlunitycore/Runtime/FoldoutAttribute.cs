using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Hardlight
{
	// Original HLUnityCore.Runtime 0200026c; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class FoldoutAttribute : PropertyAttribute
	{
		public readonly bool StartExpanded;

		// 06000f67 (1.10.1 RVA 0x1B1E590)
		public FoldoutAttribute(bool startExpanded = false)
		{
			StartExpanded = startExpanded;
		}
	}
}
