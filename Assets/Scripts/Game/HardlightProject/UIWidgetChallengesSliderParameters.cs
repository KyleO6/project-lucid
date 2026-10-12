using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020009aa; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public sealed class UIWidgetChallengesSliderParameters : IUIWidgetParameters
	{
		public readonly float Value;

		public readonly int MaxValue;

		public readonly int Size;

		// 060037df (1.10.1 RVA 0x69C6D0)
		public UIWidgetChallengesSliderParameters(float value, int maxValue, int size)
		{
			Value = value;
			MaxValue = maxValue;
			Size = size;
		}
	}
}
