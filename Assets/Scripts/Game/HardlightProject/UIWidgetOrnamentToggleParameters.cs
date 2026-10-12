using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using Hardlight.Enums;
using Hardlight.Localisation;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020009be; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public sealed class UIWidgetOrnamentToggleParameters : IUIWidgetParameters
	{
		public readonly string Label;

		public readonly OrnamentDefinition Definition;

		public readonly bool Available;

		public readonly Action<OrnamentDefinition, bool, bool> SelectCallback;

		public readonly Action<RectTransform> HoverCallback;

		public readonly UIToggleGroup ToggleGroup;

		public readonly bool DefaultSelected;

		public bool IsNew;

		// 0600381e (1.10.1 RVA 0x69F390)
		public UIWidgetOrnamentToggleParameters(OrnamentDefinition ornamentDefinition, OrnamentManager.OrnamentStatus ornamentStatus, Action<OrnamentDefinition, bool, bool> selectCallback, Action<RectTransform> hoverCallback, UIToggleGroup toggleGroup, bool defaultSelected)
		{
			Label = StringTable.GetString(ornamentDefinition.LocalisedName);
			Definition = ornamentDefinition;
			Available = ornamentStatus.IsAvailable();
			SelectCallback = selectCallback;
			HoverCallback = hoverCallback;
			ToggleGroup = toggleGroup;
			DefaultSelected = defaultSelected;
			IsNew = !ornamentStatus.Seen;
		}
	}
}
