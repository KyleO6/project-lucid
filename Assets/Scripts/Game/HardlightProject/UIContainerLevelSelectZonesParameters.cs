using Unity.IL2CPP.CompilerServices;
using System;
using System.Runtime.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020008d3; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public sealed class UIContainerLevelSelectZonesParameters : IUIContainerParameters
	{
		public Action<GameplayLevelDefinition> OnLevelSelected { get; }

		public Action<GameplayLevelCategory> OnZoneSelected { get; }

		// 060032f4 (1.10.1 RVA 0x638230)
		public UIContainerLevelSelectZonesParameters(Action<GameplayLevelDefinition> onLevelSelected, Action<GameplayLevelCategory> onZoneSelected)
		{
			OnLevelSelected = onLevelSelected;
			OnZoneSelected = onZoneSelected;
		}
	}
}
