using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using System;

namespace Hardlight
{
	// Original HLSplines.Runtime 02000004; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "HLSplinesConfiguration", menuName = "Hardlight/Splines/Configuration")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class HLSplinesConfiguration : SystemConfigurationAsset
	{
		// 06000003 (1.10.1 RVA 0x1A53680)
		public override void Validate()
		{
		}

		// 06000004 (1.10.1 RVA 0x1A53690)
		public HLSplinesConfiguration()
		{
		}
	}
}
