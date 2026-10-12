using Unity.IL2CPP.CompilerServices;
using System;

namespace HardlightProject
{
	// Original Game.Runtime 0200098e; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ScreenControlGroupMessageData
	{
		public ScreenControlsGroup GroupToEnable;

		// 06003743 (1.10.1 RVA 0x696A20)
		public ScreenControlGroupMessageData()
		{
		}
	}
}
