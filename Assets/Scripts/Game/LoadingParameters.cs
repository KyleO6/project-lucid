using Unity.IL2CPP.CompilerServices;
using System.Runtime.CompilerServices;

namespace HardlightProject
{
	// Original Game.Runtime 020006c5; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class LoadingParameters
	{
		public GameplayLevelDefinition LevelDefinition { get; }

		// 060024c2 (1.10.1 RVA 0x599F50)
		public LoadingParameters(GameplayLevelDefinition levelDefinition)
		{
			LevelDefinition = levelDefinition;
		}
	}
}
