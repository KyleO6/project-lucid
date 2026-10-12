using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Hardlight
{
	// Original HLUnityUI.Runtime 020000c7; bodies translated from the 1.10.1 x86-64 binary.
	[RequireComponent(typeof(HLScrollRect))]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public sealed class HLHorizontalScrollIndicator : MonoBehaviour, IScrollLeftHandler, IEventSystemHandler, IScrollRightHandler
	{
		// 06000565 (1.10.1 RVA 0x1B660F0)
		public void OnScrollLeft(ScrollEventData eventData)
		{
		}

		// 06000566 (1.10.1 RVA 0x1B66100)
		public void OnScrollRight(ScrollEventData eventData)
		{
		}

		// 06000567 (1.10.1 RVA 0x1B66110)
		public HLHorizontalScrollIndicator()
		{
		}
	}
}
