using Unity.IL2CPP.CompilerServices;

namespace HardlightProject
{
	// Original Game.Runtime 02000830; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ButtonBinaryStateMessageData
	{
		public enum ButtonSubscribedToBinaryStateMessage
		{
			Interact,
			Back
		}

		public readonly ButtonSubscribedToBinaryStateMessage ButtonWithBinaryState;

		public readonly bool IsActive;

		// 06002f85 (1.10.1 RVA 0x606E90)
		public ButtonBinaryStateMessageData(ButtonSubscribedToBinaryStateMessage buttonWithBinaryState, bool isActive)
		{
			ButtonWithBinaryState = buttonWithBinaryState;
			IsActive = isActive;
		}
	}
}
