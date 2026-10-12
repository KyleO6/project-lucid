using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 02000905; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public sealed class UIContainerPlayerProgressionParameters : IUIContainerParameters
	{
		public Queue<IMetaGameUnlock> Unlocks { get; }

		// 06003435 (1.10.1 RVA 0x673550)
		public UIContainerPlayerProgressionParameters(Queue<IMetaGameUnlock> unlocks)
		{
			Unlocks = unlocks;
		}
	}
}
