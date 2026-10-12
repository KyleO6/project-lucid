using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020009ad; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public sealed class UIWidgetChallengesSliderMiniParameters : IUIWidgetParameters
	{
		public readonly float Value;

		public readonly int MaxValue;

		// 060037ea (1.10.1 RVA 0x69CA80)
		public UIWidgetChallengesSliderMiniParameters(float value, int maxValue)
		{
			Value = value;
			MaxValue = maxValue;
		}
	}
}
