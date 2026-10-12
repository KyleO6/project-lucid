using Unity.IL2CPP.CompilerServices;

namespace Hardlight
{
	// Original HLUnityUI.Runtime 020000b1; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class DynamicPageLoadParameters : GuiPageParameters
	{
		public DynamicPageEntry m_pageEntry;

		// 06000517 (1.10.1 RVA 0x1B63130)
		public DynamicPageLoadParameters()
		{
		}
	}
}
