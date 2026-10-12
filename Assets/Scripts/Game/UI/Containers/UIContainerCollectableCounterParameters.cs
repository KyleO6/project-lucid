using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 02000894; bodies translated from the 1.10.1 x86-64 binary.
	public struct UIContainerCollectableCounterParameters : IUIContainerParameters
	{
		public readonly CollectableState State;

		public readonly CollectableDefinition Definition;

		public readonly bool CollectableExists;

		public readonly bool InGameplay;

		// 0600319b (1.10.1 RVA 0x6251C0)
		public UIContainerCollectableCounterParameters(CollectableState state, CollectableDefinition definition, bool collectableExists)
		{
			State = state;
			Definition = definition;
			CollectableExists = collectableExists;
			InGameplay = true;
		}

		// 0600319c (1.10.1 RVA 0x614420)
		public UIContainerCollectableCounterParameters(CollectableDefinition definition, bool inGameplay)
		{
			Definition = definition;
			InGameplay = inGameplay;
			State = null;
			CollectableExists = false;
		}
	}
}
