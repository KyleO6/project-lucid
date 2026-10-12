using Unity.IL2CPP.CompilerServices;
using System;
using UnityEngine;

namespace Hardlight
{
	// Original HLSplines.Runtime 02000076; bodies translated from the 1.10.1 x86-64 binary.
	[Serializable]
	[ExecuteInEditMode]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class EditorAutoProfiler : MonoBehaviour
	{
		// 06000313 (1.10.1 RVA 0x1A762C0)
		public EditorAutoProfiler()
		{
		}
	}
}
