using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 0200091f; bodies translated from the 1.10.1 x86-64 binary.
	public class UIContainerRewardRestrictionParameters : IUIContainerParameters
	{
		public readonly CollectableType Type;

		public readonly int Count;

		// 06003498 (1.10.1 RVA 0x678DE0)
		public UIContainerRewardRestrictionParameters(CollectableType type, int count)
		{
			Type = type;
			Count = count;
		}
	}
}
