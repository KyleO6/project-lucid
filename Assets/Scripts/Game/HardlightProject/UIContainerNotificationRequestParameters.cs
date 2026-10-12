using Unity.IL2CPP.CompilerServices;
using System.Runtime.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020008f2; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public sealed class UIContainerNotificationRequestParameters : IUIContainerParameters
	{
		public NotificationType Notification { get; }

		// 060033b3 (1.10.1 RVA 0x66BE60)
		public UIContainerNotificationRequestParameters(NotificationType notification)
		{
			Notification = notification;
		}
	}
}
