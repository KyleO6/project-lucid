using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020009b6; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class UIWidgetImageArchetypeParameters : IUIWidgetParameters
	{
		public readonly CharacterArchetypeDefinition CharacterArchetypeDefinition;

		public readonly CharacterDefinition CharacterDefinition;

		// 0600380a (1.10.1 RVA 0x699CD0)
		public UIWidgetImageArchetypeParameters(CharacterArchetypeDefinition characterArchetypeDefinition, CharacterDefinition characterDefinition)
		{
			CharacterArchetypeDefinition = characterArchetypeDefinition;
			CharacterDefinition = characterDefinition;
		}
	}
}
