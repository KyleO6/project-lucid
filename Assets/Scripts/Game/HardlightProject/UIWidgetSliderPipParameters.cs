using System.Runtime.CompilerServices;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020009cf; bodies translated from the 1.10.1 x86-64 binary.
	public struct UIWidgetSliderPipParameters : IUIWidgetParameters
	{
		public readonly Color Colour { get; }

		// 06003879 (1.10.1 RVA 0x6A31F0)
		public UIWidgetSliderPipParameters(Color colour)
		{
			Colour = colour;
			// (no initialisers)
		}
	}
}
