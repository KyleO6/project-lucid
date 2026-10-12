using System;
using System.Runtime.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020008b0; bodies translated from the 1.10.1 x86-64 binary.
	public struct UIContainerDebugCollectableModifierParameters : IUIContainerParameters
	{
		public readonly string Name { get; }

		public readonly int Collected { get; }

		public readonly int Total { get; }

		public readonly Action OnIncrease { get; }

		public readonly Action OnDecrease { get; }

		// 06003235 (1.10.1 RVA 0x62F0A0)
		public UIContainerDebugCollectableModifierParameters(string name, int collected, int total, Action onIncrease, Action onDecrease)
		{
			Name = name;
			Collected = collected;
			Total = total;
			OnIncrease = onIncrease;
			OnDecrease = onDecrease;
		}
	}
}
