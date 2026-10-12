using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020009b3; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public sealed class UIWidgetDreamPowerDescriptionParameters : IUIWidgetParameters
	{
		public readonly DreamPowerDefinition Definition;

		// 06003803 (1.10.1 RVA 0x69DEE0)
		public UIWidgetDreamPowerDescriptionParameters(DreamPowerDefinition definition)
		{
			Definition = definition;
		}
	}
}
