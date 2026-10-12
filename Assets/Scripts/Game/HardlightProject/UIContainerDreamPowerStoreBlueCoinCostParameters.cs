using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 02000870; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public sealed class UIContainerDreamPowerStoreBlueCoinCostParameters : IUIContainerParameters
	{
		public readonly DreamPowerDefinition DreamPowerDefinition;

		public readonly UIDreamPowerStoreItemState ItemState;

		// 060030d5 (1.10.1 RVA 0x615B60)
		public UIContainerDreamPowerStoreBlueCoinCostParameters(DreamPowerDefinition dreamPowerDefinition, UIDreamPowerStoreItemState itemState)
		{
			DreamPowerDefinition = dreamPowerDefinition;
			ItemState = itemState;
		}
	}
}
