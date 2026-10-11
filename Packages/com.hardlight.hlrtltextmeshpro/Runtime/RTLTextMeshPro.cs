// Complete original shipping RTLTMPro.RTLTextMeshPro reconstruction; source-only.
// Metadata fixes sealed/private visibility, ordered annotations, six properties and all 17 methods.
// Both CPU captures reject null text, default Farsi false, capture alignment and store sorting directly.
// GetFixedText locals spell the observed receiver captures; original local spelling is unknown.
// Clear/Reverse and buffer construction use genuine suppliers whose complete algorithms remain held.
// Native inlining, class initialization, exception ordering and future TMP/Unity behavior are not equated.
using TMPro;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace RTLTMPro
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [ExecuteInEditMode]
    public sealed class RTLTextMeshPro : TextMeshProUGUI
    {
        [SerializeField] private bool preserveNumbers;
        [SerializeField] private bool farsi;
        [TextArea(3, 10)]
        [SerializeField] private string originalText;
        [SerializeField] private bool fixTags = true;
        [SerializeField] private bool forceFix;
        [SerializeField] private bool m_flipLeftAlignmentToRight;
        [SerializeField] private bool m_inverseGeometrySortingOrder;
        private int m_serialisedAlignment;
        private readonly FastStringBuilder finalText = new FastStringBuilder(RTLSupport.DefaultBufferSize);

        public override string text
        {
            get { return base.text; }
            set
            {
                bool unchanged = originalText == value;
                if (value == null || unchanged)
                    return;
                originalText = value;
                UpdateText();
            }
        }

        public string OriginalText { get { return originalText; } }

        public bool PreserveNumbers
        {
            get { return preserveNumbers; }
            set
            {
                if (preserveNumbers == value)
                    return;
                preserveNumbers = value;
                havePropertiesChanged = true;
            }
        }

        public bool Farsi
        {
            get { return farsi; }
            set
            {
                if (farsi == value)
                    return;
                farsi = value;
                havePropertiesChanged = true;
            }
        }

        public bool FixTags
        {
            get { return fixTags; }
            set
            {
                if (fixTags == value)
                    return;
                fixTags = value;
                havePropertiesChanged = true;
            }
        }

        public bool ForceFix
        {
            get { return forceFix; }
            set
            {
                if (forceFix == value)
                    return;
                forceFix = value;
                havePropertiesChanged = true;
            }
        }

        protected override void Awake()
        {
            if (m_serialisedAlignment == 0)
                m_serialisedAlignment = (int)alignment;
            base.Awake();
        }

        private void Update()
        {
            if (havePropertiesChanged && originalText != null)
                UpdateText();
        }

        public void UpdateText()
        {
            if (originalText == null)
                originalText = string.Empty;
            if (forceFix || TextUtils.IsRTLInput(originalText))
            {
                isRightToLeftText = true;
                base.text = GetFixedText(originalText);
                m_geometrySortingOrder = m_inverseGeometrySortingOrder ? VertexSortingOrder.Normal : VertexSortingOrder.Reverse;
                if (m_flipLeftAlignmentToRight)
                    FlipAlignment(true);
            }
            else
            {
                isRightToLeftText = false;
                base.text = originalText;
                m_geometrySortingOrder = m_inverseGeometrySortingOrder ? VertexSortingOrder.Reverse : VertexSortingOrder.Normal;
                if (m_flipLeftAlignmentToRight)
                    FlipAlignment(false);
            }
        }

        private void FlipAlignment(bool isRTL)
        {
            if (m_serialisedAlignment == 0)
                m_serialisedAlignment = (int)alignment;
            if (!isRTL)
            {
                alignment = (TextAlignmentOptions)m_serialisedAlignment;
                return;
            }
            if ((m_serialisedAlignment & 255) == 1)
                alignment = (TextAlignmentOptions)unchecked(m_serialisedAlignment + 3);
            else if ((m_serialisedAlignment & 255) == 4)
                alignment = (TextAlignmentOptions)unchecked(m_serialisedAlignment - 3);
        }

        private string GetFixedText(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;
            FastStringBuilder buffer = finalText;
            buffer.Clear();
            RTLSupport.FixRTL(input, buffer, farsi, fixTags, preserveNumbers);
            buffer = finalText;
            buffer.Reverse();
            return buffer.ToString();
        }

        public RTLTextMeshPro()
        {
        }
    }
}
