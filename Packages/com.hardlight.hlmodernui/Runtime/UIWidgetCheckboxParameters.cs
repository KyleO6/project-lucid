using Unity.IL2CPP.CompilerServices;
using System;

namespace Hardlight
{
	// Original HLModernUI.Runtime 02000044; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class UIWidgetCheckboxParameters : IUIWidgetParameters
	{
		public readonly Action<bool> Callback;

		// 060001c0 (1.10.1 RVA 0x1A49710)
		public UIWidgetCheckboxParameters(Action<bool> callback)
		{
			Callback = callback;
		}
	}
}
