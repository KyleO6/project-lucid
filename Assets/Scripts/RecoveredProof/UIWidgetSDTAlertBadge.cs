// Original Game.Runtime.dll method tokens and half-open native evidence ranges.
// 0x06003824 OnValidate: ARM64 [0x67ab5c,0x67ab60); x86_64 [0x69f5c0,0x69f5d0).
// 0x06003825 SetAlert: ARM64 [0x67ab60,0x67ac78); x86_64 [0x69f5d0,0x69f6c0).
// 0x06003826 .ctor: ARM64 [0x67ac78,0x67ad08); x86_64 [0x69f6c0,0x69f740).
// Complete original Game.Runtime.dll HardlightProject.UIWidgetSDTAlertBadge 0x020009c0.
// Three methods, five original private serialized fields and their ordered annotations are preserved.
// ARM64 and x86_64 source evidence preserves empty OnValidate, activation order, count/alert branches,
// constructor literals !/{0}+/99 and the genuine RTLTextMeshPro.text virtual dispatch at slot66.
// Local variable spelling is reconstructed; optimizer fault/field-read timing and provider/Engine
// behavior are separate held claims. This source contains no offline replacement or invented checks.
using Hardlight;
using RTLTMPro;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace HardlightProject
{
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.NullChecks, false)]
    public sealed class UIWidgetSDTAlertBadge : UIWidget
    {
        [SerializeField]
        [Tooltip("Text to fill when in the alerting state.")]
        private string m_alertText = "!";

        [Min(1)]
        [SerializeField]
        [Tooltip("Limit of numerical value to display. If value goes over this, then the max value is used with the below text format.")]
        private int m_maxValue = 99;

        [SerializeField]
        [Tooltip("Text format to decorate max value with when max value is exceeded. Parameter {0} = max value.")]
        private string m_textFormatOverMaxValue = "{0}+";

        [SerializeField]
        [Tooltip("RTLTextMeshPro to fill text into.")]
        private RTLTextMeshPro m_text;

        [SerializeField]
        [Tooltip("Enable this object while the alert is active. Disable it when inactive.")]
        private GameObject m_enableObjectWhileActive;

        // Game.Runtime.dll 0x06003824: the shipping private validation method is empty.
        private void OnValidate() { }

        // Game.Runtime.dll 0x06003825. Activation precedes text dispatch and formatting.
        // The inactive branch leaves the previous text untouched. Alert wins over count.
        public void SetAlert(bool alert = false, int displayNumber = 0)
        {
            GameObject enableObject = m_enableObjectWhileActive;
            if (displayNumber <= 0 && !alert)
            {
                enableObject.SetActive(false);
                return;
            }

            enableObject.SetActive(true);
            if (alert)
            {
                m_text.text = m_alertText;
                return;
            }

            RTLTextMeshPro text = m_text;
            if (displayNumber > m_maxValue)
                text.text = string.Format(m_textFormatOverMaxValue, m_maxValue.ToString());
            else
                text.text = displayNumber.ToString();
        }

        // Game.Runtime.dll 0x06003826: field initializers precede the UIWidget base constructor.
        public UIWidgetSDTAlertBadge() { }
    }
}
