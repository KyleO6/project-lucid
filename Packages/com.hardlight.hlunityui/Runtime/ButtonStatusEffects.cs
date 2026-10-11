// Reconstructed whole HLUnityUI.Runtime ButtonStatusEffects0200005e/11 methods from both native slices.
// Original C# bytes and local spellings are unavailable; color-property spelling and optimized getter calls are inferred.
// Native setter commits after both appearance calls; a fault preserves the earlier value. Both enable/disable capture before Initialise.
// Initialise caches enabled sprite/color first, then optional real HLUIEffectManager styles, then optional shared font material, then publishes initialized.
// Five genuine manager getters are retained as provider contracts; their complete original algorithms/source/emission remain separately held.
// Awake uses Unity Object equality, searches children only for a null monitor, then registers once; disable does not unregister.
// Text replacement short-circuits target, existing shared material, new material; absent current material is not repaired.
// SetButtonFace writes sprite before color and commits its flag only inside image-present and Application.isPlaying guards.
// Constructor writes magenta and white before real MonoBehaviour base construction; all other fields use managed zero initialization.
// All original faults, Unity equality, reentrant receiver rereads and provider holds are preserved; no Engine/runtime equivalence claim.
using RTLTMPro;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Hardlight
{
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [RequireComponent(typeof(HLCurveAnimatedGraphic))]
    public class ButtonStatusEffects : MonoBehaviour
    {
        private bool m_isInitialised;
        [SerializeField] private HLCurveAnimatedGraphic m_targetToMonitor;
        [SerializeField] private Image m_targetImage;
        [SerializeField] private bool m_isSecondaryButton;
        [SerializeField] private RTLTextMeshPro m_targetTMPUI;
        [SerializeField] private bool m_isSecondaryFont;
        private Sprite m_disabledBackground;
        private Color m_disabledButtonFaceColour = Color.magenta;
        private Material m_disabledTextMeshProMaterial;
        private Sprite m_enabledSprite;
        private Color m_enabledButtonFaceColour = Color.white;
        private Material m_enabledTextMeshProMaterial;
        private bool m_interactable;

        public bool Interactable
        {
            get { return m_interactable; }
            set
            {
                if (m_interactable == value) return;
                Initialise();
                SetTextMeshProAppearance(value ? m_enabledTextMeshProMaterial : m_disabledTextMeshProMaterial);
                SetButtonFace(value);
                m_interactable = value;
            }
        }

        private void Awake()
        {
            if (m_targetToMonitor == null)
                m_targetToMonitor = GetComponentInChildren<HLCurveAnimatedGraphic>();
            if (m_targetToMonitor != null)
                m_targetToMonitor.RegisterButtonStatusEffector(this);
        }

        private void OnEnable()
        {
            bool enabled = m_interactable;
            Initialise();
            SetTextMeshProAppearance(enabled ? m_enabledTextMeshProMaterial : m_disabledTextMeshProMaterial);
            SetButtonFace(enabled);
        }

        private void OnDisable()
        {
            bool enabled = m_interactable;
            Initialise();
            SetTextMeshProAppearance(enabled ? m_enabledTextMeshProMaterial : m_disabledTextMeshProMaterial);
            SetButtonFace(enabled);
        }

        private void Initialise()
        {
            if (m_isInitialised) return;
            if (m_targetImage != null)
            {
                m_enabledSprite = m_targetImage.sprite;
                m_enabledButtonFaceColour = m_targetImage.color;
            }
            if (HLUIEffectManager.Instance != null)
            {
                m_disabledButtonFaceColour = HLUIEffectManager.Instance.GetDisabledButtonFaceColour();
                m_disabledBackground = m_isSecondaryButton
                    ? HLUIEffectManager.Instance.GetDisabledSecondaryButtonBackground()
                    : HLUIEffectManager.Instance.GetDisabledButtonBackground();
                m_disabledTextMeshProMaterial = m_isSecondaryFont
                    ? HLUIEffectManager.Instance.GetDisabledSecondaryTextMeshProMaterialPreset()
                    : HLUIEffectManager.Instance.GetDisabledPrimaryTextMeshProMaterialPreset();
            }
            if (m_targetTMPUI != null)
                m_enabledTextMeshProMaterial = m_targetTMPUI.fontSharedMaterial;
            m_isInitialised = true;
        }

        private void SetTextMeshProAppearance(Material newMaterial)
        {
            if (m_targetTMPUI != null && m_targetTMPUI.fontSharedMaterial != null && newMaterial != null)
                m_targetTMPUI.fontSharedMaterial = newMaterial;
        }

        private void SetButtonFace(bool enabled)
        {
            if (m_targetImage != null && Application.isPlaying)
            {
                if (enabled)
                {
                    m_targetImage.sprite = m_enabledSprite;
                    m_targetImage.color = m_enabledButtonFaceColour;
                }
                else
                {
                    m_targetImage.sprite = m_disabledBackground;
                    m_targetImage.color = m_disabledButtonFaceColour;
                }
                m_interactable = enabled;
            }
        }

        public void SetDisabledButtonState()
        {
            Initialise();
            SetTextMeshProAppearance(m_disabledTextMeshProMaterial);
            SetButtonFace(false);
        }

        public void SetEnabledButtonState()
        {
            Initialise();
            SetTextMeshProAppearance(m_enabledTextMeshProMaterial);
            SetButtonFace(true);
        }

        public ButtonStatusEffects() { }
    }
}
