using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Hardlight
{
	// Original HLUnityUI.Runtime 020000d5; bodies translated from the 1.10.1 x86-64 binary.
	[RequireComponent(typeof(HLScrollRect))]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public sealed class HLVerticalScrollIndicator : MonoBehaviour, IScrollUpHandler, IEventSystemHandler, IScrollDownHandler
	{
		// 060005c6 (1.10.1 RVA 0x1B6B070)
		public void OnScrollUp(ScrollEventData eventData)
		{
		}

		// 060005c7 (1.10.1 RVA 0x1B6B080)
		public void OnScrollDown(ScrollEventData eventData)
		{
		}

		// 060005c8 (1.10.1 RVA 0x1B6B090)
		public HLVerticalScrollIndicator()
		{
		}
	}
}
