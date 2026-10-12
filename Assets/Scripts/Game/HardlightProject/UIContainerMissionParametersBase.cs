using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020008d9; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public abstract class UIContainerMissionParametersBase : IUIContainerParameters
	{
		public readonly MissionState MissionState;

		// 06003307 (1.10.1 RVA 0x63BCC0)
		protected UIContainerMissionParametersBase(MissionState missionState)
		{
			MissionState = missionState;
		}
	}
}
