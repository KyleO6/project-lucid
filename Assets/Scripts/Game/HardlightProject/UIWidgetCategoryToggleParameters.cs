using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020009a7; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public sealed class UIWidgetCategoryToggleParameters : IUIWidgetParameters
	{
		public readonly bool IsSelected;

		public readonly int Collected;

		public readonly int Total;

		public readonly OrnamentCategory Category;

		// 060037d3 (1.10.1 RVA 0x69C1D0)
		public UIWidgetCategoryToggleParameters(bool isSelected, int collected, int total, OrnamentCategory category)
		{
			IsSelected = isSelected;
			Collected = collected;
			Total = total;
			Category = category;
		}
	}
}
