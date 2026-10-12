using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020009d3; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public sealed class UIWidgetToggleDreamPowerSlotParameters : IUIWidgetParameters
	{
		public readonly DreamPowerDefinition Definition;

		public readonly UIDreamPowerStoreItemState State;

		public readonly bool Available;

		public readonly Action<DreamPowerDefinition> OnToggleChanged;

		// 0600388b (1.10.1 RVA 0x6A3C60)
		public UIWidgetToggleDreamPowerSlotParameters(DreamPowerDefinition definition, UIDreamPowerStoreItemState state, bool available, Action<DreamPowerDefinition> toggleChanged)
		{
			Definition = definition;
			State = state;
			Available = available;
			OnToggleChanged = toggleChanged;
		}
	}
}
