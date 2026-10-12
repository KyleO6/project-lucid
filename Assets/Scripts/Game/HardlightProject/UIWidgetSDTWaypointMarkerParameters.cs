using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020009cd; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class UIWidgetSDTWaypointMarkerParameters : IUIWidgetParameters
	{
		public readonly WaypointTarget Target;

		public readonly WaypointDefinition Definition;

		public readonly WaypointContainerType ContainerType;

		// 06003875 (1.10.1 RVA 0x6A30A0)
		public UIWidgetSDTWaypointMarkerParameters(WaypointTarget target, WaypointDefinition definition, WaypointContainerType containerType)
		{
			Target = target;
			Definition = definition;
			ContainerType = containerType;
		}
	}
}
