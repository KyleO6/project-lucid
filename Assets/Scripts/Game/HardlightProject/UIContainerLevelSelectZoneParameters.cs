using Unity.IL2CPP.CompilerServices;
using System.Runtime.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020008cc; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public sealed class UIContainerLevelSelectZoneParameters : IUIContainerParameters
	{
		public GameplayLevelDefinition PreselectedLevel { get; }

		public GameplayLevelDefinition NextProductionLevelLocked { get; }

		// 060032d5 (1.10.1 RVA 0x639970)
		public UIContainerLevelSelectZoneParameters(GameplayLevelDefinition preselectedLevel, GameplayLevelDefinition nextProductionLevelLocked)
		{
			PreselectedLevel = preselectedLevel;
			NextProductionLevelLocked = nextProductionLevelLocked;
		}
	}
}
