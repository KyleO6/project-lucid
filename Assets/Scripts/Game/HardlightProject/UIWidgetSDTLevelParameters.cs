using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020009c5; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public sealed class UIWidgetSDTLevelParameters : IUIWidgetParameters
	{
		public GameplayLevelDefinition LevelDefinition { get; }

		public ProgressionCollectableLevel LevelProgress { get; }

		public IReadOnlyDictionary<CollectableType, ProgressionCollectable> OverallProgress { get; }

		public bool IsNextLevel { get; }

		public bool Hidden { get; }

		public bool Locked { get; }

		public bool UnlockSeen { get; }

		public bool IsNew { get; }

		public int LevelNumericalName { get; }

		// 06003856 (1.10.1 RVA 0x6A1EF0)
		public UIWidgetSDTLevelParameters(GameplayLevelDefinition levelDefinition, ProgressionCollectableLevel levelProgress, IReadOnlyDictionary<CollectableType, ProgressionCollectable> overallProgress, bool isNextLevel, bool locked, bool unlockSeen, bool isNew, int levelNumericalName, bool hidden)
		{
			LevelDefinition = levelDefinition;
			LevelProgress = levelProgress;
			OverallProgress = overallProgress;
			IsNextLevel = isNextLevel;
			Hidden = hidden;
			Locked = locked;
			UnlockSeen = unlockSeen;
			IsNew = isNew;
			LevelNumericalName = levelNumericalName;
		}
	}
}
