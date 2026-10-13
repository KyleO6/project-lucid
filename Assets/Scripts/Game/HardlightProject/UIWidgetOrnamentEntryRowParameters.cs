using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020009b9; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.NullChecks, false)]
	public sealed class UIWidgetOrnamentEntryRowParameters : IUIWidgetParameters
	{
		public readonly IReadOnlyList<UIWidgetOrnamentToggleParameters> Toggles;

		// 06003812 (1.10.1 RVA 0x69E850)
		public UIWidgetOrnamentEntryRowParameters(List<UIWidgetOrnamentToggleParameters> toggles)
		{
			Toggles = new List<UIWidgetOrnamentToggleParameters>(toggles);
		}
	}
}
