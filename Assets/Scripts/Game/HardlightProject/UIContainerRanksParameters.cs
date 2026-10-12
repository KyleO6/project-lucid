using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020008e4; bodies translated from the 1.10.1 x86-64 binary.
	public struct UIContainerRanksParameters : IUIContainerParameters
	{
		public readonly MissionDefinition MissionDefinition;

		public readonly ChallengeDefinition ChallengeDefinition;

		// 06003355 (1.10.1 RVA 0x637200)
		public UIContainerRanksParameters(MissionDefinition missionDefinition, ChallengeDefinition challengeDefinition)
		{
			this = default;
			ChallengeDefinition = challengeDefinition;
		}
	}
}
