using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020008a5; bodies translated from the 1.10.1 x86-64 binary.
	public class UIContainerCrossPromoParameters : IUIContainerParameters
	{
		public readonly int ImageIndex;

		// 060031ec (1.10.1 RVA 0x62A880)
		public UIContainerCrossPromoParameters(int imageIndex)
		{
			ImageIndex = imageIndex;
		}
	}
}
