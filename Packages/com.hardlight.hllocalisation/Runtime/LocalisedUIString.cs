// Original HLLocalisation.Runtime Hardlight.Localisation.LocalisedUIString 0x0200000c.
// Native-derived complete source; original faults and callback order retained.
// 0x0600002c MakeDirty; arm64 [0x1a3b5cc,0x1a3b5d8), x86_64 [0x1a35840,0x1a35850).
// 0x0600002d get_Identifier; arm64 [0x1a3b5d8,0x1a3b5e0), x86_64 [0x1a35850,0x1a35860).
// 0x0600002e set_Identifier; arm64 [0x1a3b5e0,0x1a3b5f0), x86_64 [0x1a35860,0x1a35870).
// 0x0600002f OnEnable; arm64 [0x1a3b5f0,0x1a3b6ec), x86_64 [0x1a35870,0x1a35960).
// 0x06000030 OnDisable; arm64 [0x1a3b8e4,0x1a3b9d4), x86_64 [0x1a35aa0,0x1a35b80).
// 0x06000031 Update; arm64 [0x1a3b9d4,0x1a3ba04), x86_64 [0x1a35b80,0x1a35bb0).
// 0x06000032 FindTextTargets; arm64 [0x1a3ba04,0x1a3bbcc), x86_64 [0x1a35bb0,0x1a35d40).
// 0x06000033 UpdateString; arm64 [0x1a3b6ec,0x1a3b8e4), x86_64 [0x1a35960,0x1a35aa0).
// 0x06000034 GetLocalisedText; arm64 [0x1a3bbcc,0x1a3bcd0), x86_64 [0x1a35d40,0x1a35df0).
// 0x06000035 .ctor; arm64 [0x1a3bf1c,0x1a3bf24), x86_64 [0x1a36070,0x1a36080).
// OnEnable updates before subscription; Update clears dirty only after success.
// FindTextTargets resets all four targets, queries Text/3D/RTL then 2D only without RTL.
// UpdateString preserves null-only fallback and Text/3D/2D/RTL assignment order.
// Default identifier remains zero; no implicit target discovery or readiness substitute.
// StringTable and real text/font/component lifecycle still need independent Engine validation.
using Hardlight.Enums;
using Hardlight.Utils;
using RTLTMPro;
using TMPro;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Hardlight.Localisation
{
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [DisallowMultipleComponent]
    public class LocalisedUIString : MonoBehaviour
    {
        [HashEnum(typeof(Strings))]
        [SerializeField] private Strings m_identifier;
        [SerializeField] private Text m_uguiTextTarget;
        [SerializeField] private TextMeshProUGUI m_tmpUguiTextTarget;
        [SerializeField] private RTLTextMeshPro m_tmpRTLTextTarget;
        [SerializeField] private TextMeshPro m_tmpTextTarget;
        private bool m_dirty;

        public void MakeDirty()
        {
            m_dirty = true;
        }

        public Strings Identifier
        {
            get { return m_identifier; }
            set
            {
                m_identifier = value;
                m_dirty = true;
            }
        }

        public void OnEnable()
        {
            UpdateString();
            StringTable.StringsTableHandlers += MakeDirty;
        }

        public void OnDisable()
        {
            StringTable.StringsTableHandlers -= MakeDirty;
        }

        public void Update()
        {
            if (m_dirty)
            {
                UpdateString();
                m_dirty = false;
            }
        }

        public void FindTextTargets()
        {
            m_uguiTextTarget = null;
            m_tmpTextTarget = null;
            m_tmpRTLTextTarget = null;
            m_tmpUguiTextTarget = null;
            m_uguiTextTarget = GetComponent<Text>();
            m_tmpTextTarget = GetComponent<TextMeshPro>();
            m_tmpRTLTextTarget = GetComponent<RTLTextMeshPro>();
            if (m_tmpRTLTextTarget == null)
                m_tmpUguiTextTarget = GetComponent<TextMeshProUGUI>();
        }

        public void UpdateString(string overrideString = null)
        {
            string content = overrideString ?? GetLocalisedText();
            if (m_uguiTextTarget != null)
                m_uguiTextTarget.text = content;
            if (m_tmpTextTarget != null)
                m_tmpTextTarget.SetText(content);
            if (m_tmpUguiTextTarget != null)
                m_tmpUguiTextTarget.SetText(content);
            if (m_tmpRTLTextTarget != null)
                m_tmpRTLTextTarget.text = content;
        }

        private string GetLocalisedText()
        {
            string content = null;
            int numArgs;
            if (!StringTable.GetStringAndNumArgs(m_identifier, out content, out numArgs) &&
                m_identifier != Strings.NONE && string.IsNullOrEmpty(content))
                content = "NO STRING_ID SET";
            return content;
        }

        public LocalisedUIString() { }
    }
}
