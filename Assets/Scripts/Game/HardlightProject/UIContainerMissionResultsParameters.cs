using System.Runtime.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020008e6; bodies translated from the 1.10.1 x86-64 binary.
	public struct UIContainerMissionResultsParameters : IUIContainerParameters
	{
		public readonly MissionDefinition MissionDefinition { get; }

		public readonly GameplayLevelDefinition LevelDefinition { get; }

		// 06003360 (1.10.1 RVA 0x637230)
		public UIContainerMissionResultsParameters(MissionDefinition missionDefinition, GameplayLevelDefinition levelDefinition)
		{
			this = default;
			LevelDefinition = levelDefinition;
		}
	}
}
