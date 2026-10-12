using Unity.IL2CPP.CompilerServices;
using System.Runtime.CompilerServices;
using Hardlight;
using Hardlight.Enums;

namespace HardlightProject
{
	// Original Game.Runtime 0200091c; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class UIContainerQRCodeParameters : IUIContainerParameters
	{
		public Strings URLStringId { get; }

		// 06003492 (1.10.1 RVA 0x678950)
		public UIContainerQRCodeParameters(Strings urlStringId)
		{
			URLStringId = urlStringId;
		}
	}
}
