// Original RTLTextMeshPro3D15, protected fields/ForceFix; no own Awake or FlipAlignment.
// Farsi/fixTags default true; changed null is accepted, repaired to empty in UpdateText.
// UpdateText publishes dirty=true after base text. Buffer rereads, original callback/fault
// and TMP font/material/lifecycle/Engine equivalence remain held; no substitute provider.
using TMPro;
using UnityEngine;

namespace RTLTMPro
{
    [ExecuteInEditMode]
    public class RTLTextMeshPro3D : TextMeshPro
    {
        [SerializeField] protected bool preserveNumbers;
        [SerializeField] protected bool farsi = true;
        [SerializeField, TextArea(3, 10)] protected string originalText;
        [SerializeField] protected bool fixTags = true;
        [SerializeField] protected bool forceFix;
        protected readonly FastStringBuilder finalText = new FastStringBuilder(2048);

        public override string text
        {
            get { return base.text; }
            set
            {
                if (originalText == value) return;
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
                if (preserveNumbers == value) return;
                preserveNumbers = value;
                havePropertiesChanged = true;
            }
        }

        public bool Farsi
        {
            get { return farsi; }
            set
            {
                if (farsi == value) return;
                farsi = value;
                havePropertiesChanged = true;
            }
        }

        public bool FixTags
        {
            get { return fixTags; }
            set
            {
                if (fixTags == value) return;
                fixTags = value;
                havePropertiesChanged = true;
            }
        }

        protected bool ForceFix
        {
            get { return forceFix; }
            set
            {
                if (forceFix == value) return;
                forceFix = value;
                havePropertiesChanged = true;
            }
        }

        protected void Update()
        {
            if (havePropertiesChanged) UpdateText();
        }

        public void UpdateText()
        {
            if (originalText == null) originalText = string.Empty;
            if (forceFix || TextUtils.IsRTLInput(originalText))
            {
                isRightToLeftText = true;
                base.text = GetFixedText(originalText);
            }
            else
            {
                isRightToLeftText = false;
                base.text = originalText;
            }
            havePropertiesChanged = true;
        }

        private string GetFixedText(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            finalText.Clear();
            RTLSupport.FixRTL(input, finalText, farsi, fixTags, preserveNumbers);
            finalText.Reverse();
            return finalText.ToString();
        }

        public RTLTextMeshPro3D() { }
    }
}
