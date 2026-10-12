using Unity.IL2CPP.CompilerServices;
using System;
using UnityEngine;

namespace Hardlight
{
	// Original HLSplines.Runtime 02000006; bodies translated from the 1.10.1 x86-64 binary.
	[Serializable]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class MetadataFixed : Metadata
	{
		// 06000010 (1.10.1 RVA 0x1A53900)
		public MetadataFixed(MetadataKeyType key, string value) : base(key, value)
		{
		}
	}
}
