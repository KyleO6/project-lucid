using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020008c6; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class UIContainerLevelViewParameters : IUIContainerParameters
	{
		public readonly GameplayLevelDefinition LevelDefinition;

		// 060032ad (1.10.1 RVA 0x636A00)
		public UIContainerLevelViewParameters(GameplayLevelDefinition levelDefinition)
		{
			LevelDefinition = levelDefinition;
		}
	}
}
