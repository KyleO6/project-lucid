using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Hardlight
{
	// Original HLModernUI.Runtime 02000046; bodies translated from the 1.10.1 x86-64 binary.
	[RequireComponent(typeof(Slider))]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class UIWidgetProgressBar : MonoBehaviour
	{
		private Slider m_slider;

		// 060001c4 (1.10.1 RVA 0x1A49840)
		private void Start()
		{
			m_slider = GetComponent<Slider>();
		}

		// 060001c5 (1.10.1 RVA 0x1A49890)
		public void SetValue(float value)
		{
			m_slider.value = value;
		}

		// 060001c6 (1.10.1 RVA 0x1A498B0)
		public UIWidgetProgressBar()
		{
		}
	}
}
