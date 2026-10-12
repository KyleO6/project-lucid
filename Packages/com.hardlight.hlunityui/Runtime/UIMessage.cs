
namespace Hardlight
{
	// Original HLUnityUI.Runtime 02000077; bodies translated from the 1.10.1 x86-64 binary.
	public readonly struct UIMessage
	{
		private readonly UIEventType Type;

		// 0600037e (1.10.1 RVA 0x1B54110)
		public UIMessage(UIEventType type)
		{
			Type = type;
			// (no initialisers)
		}
	}
}
