using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020009a0; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.NullChecks, false)]
	public class UIWidgetButtonCharacterImageParameters : UIWidgetButtonParameters
	{
		public readonly CharacterDefinition CharacterDefinition;

		public readonly bool ShowSwapIcon;

		// 060037b9 (1.10.1 RVA 0x69AE40)
		public UIWidgetButtonCharacterImageParameters(CharacterDefinition characterDefinition, bool showSwapIcon, Action callback) : base(string.Empty, callback)
		{
			CharacterDefinition = characterDefinition;
			ShowSwapIcon = showSwapIcon;
		}
	}
}
