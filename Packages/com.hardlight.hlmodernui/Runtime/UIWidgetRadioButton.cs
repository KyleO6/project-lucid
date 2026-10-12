using Unity.IL2CPP.CompilerServices;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Hardlight
{
	// Original HLModernUI.Runtime 02000047; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class UIWidgetRadioButton : MonoBehaviour
	{
		[SerializeField]
		private Image m_buttonImage;

		[SerializeField]
		private Sprite m_onSprite;

		[SerializeField]
		private Sprite m_offSprite;

		public Action<UIWidgetRadioButton> OnToggle;

		private bool selected;

		// 060001c7 (1.10.1 RVA 0x1A498C0)
		public void Action_OnClick()
		{
			Toggle();
		}

		// 060001c8 (1.10.1 RVA 0x1A49910)
		private void Toggle()
		{
			if (selected)
			{
				selected = false;
				m_buttonImage.sprite = m_offSprite;
				OnToggle?.Invoke(this);
			}
			else
			{
				selected = true;
				m_buttonImage.sprite = m_onSprite;
				OnToggle?.Invoke(this);
			}
		}

		// 060001c9 (1.10.1 RVA 0x1A49970)
		public void Select()
		{
			selected = true;
			m_buttonImage.sprite = m_onSprite;
		}

		// 060001ca (1.10.1 RVA 0x1A49990)
		public void Deselect()
		{
			selected = false;
			m_buttonImage.sprite = m_offSprite;
		}

		// 060001cb (1.10.1 RVA 0x1A499B0)
		public UIWidgetRadioButton()
		{
		}
	}
}
