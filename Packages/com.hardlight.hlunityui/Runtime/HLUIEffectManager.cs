// Original HLUnityUI.Runtime Hardlight.HLUIEffectManager 02000049 and HLAudioCueKVP 0200004a.
// Reconstructed from complete ARM64/x86_64 bodies and literal declarations; original C# spelling is inferred.
// Awake logs before base registration and keeps the first audio cue per key, including a null clip.
// OnDestroy uses the genuine safe system lookup and Unity null semantics before its base callback.
// Curve end-time reads length once, returns zero only for an empty curve, and otherwise reads the final key.
// Samples pass raw time through; no clamping, finite-value validation or null guards are added.
// ReferencePixelsPerUnit's original setter does nothing on both architectures.
// Eighteen curve allocations, five colour initializers and comparer-backed dictionary allocation retain order.
// No provider/native/service bodies were executed. Source form, optimizer/fault equivalence, current emission,
// serialized assets, genuine supplier algorithms and Engine/bootstrap acceptance remain separate requirements.
using System;
using System.Collections.Generic;
using Hardlight.Utils;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Hardlight
{
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.NullChecks, false)]
    public class HLUIEffectManager : MonoSingleton<HLUIEffectManager>
    {
        public const float SubPageLeftRightEdgeBufferPixelsPortrait = 300f;

        public const float SubPageLeftRightEdgeBufferPixelsLandscape = 25f;

        public const float FadeInAlpha = 1f;

        public const float FadeOutAlpha = 0f;

        [Tooltip("Curve to sample from to shrink UI elements on press down")]
        [SerializeField]
        [Header("Sample curves")]
        private AnimationCurve m_onPressBounceCurve = new AnimationCurve();

        [Tooltip("Curve to sample from to bounce UI elements on press complete")]
        [SerializeField]
        private AnimationCurve m_onReleaseBounceCurve = new AnimationCurve();

        [Tooltip("Curve to sample from to shake UI elements on press when disabled")]
        [SerializeField]
        private AnimationCurve m_onDisabledPressShakeCurve = new AnimationCurve();

        [Tooltip("Value to scale disabled curve value along X and y")]
        [SerializeField]
        private Vector2 m_disableWobbleScaler = new Vector2(10f, 0f);

        [Tooltip("Curve to sample from to bounce UI elements on enable")]
        [SerializeField]
        private AnimationCurve m_onEnableBounceCurve = new AnimationCurve();

        [SerializeField]
        [Tooltip("Curve to sample from for the slide on/off animation stretch or bounce")]
        private AnimationCurve m_uiUnclampedSlideAnimationCurve = new AnimationCurve();

        [Tooltip("Curve to sample from for the slide on/off animation when clamped")]
        [SerializeField]
        private AnimationCurve m_uiClampedSlideAnimationCurve = new AnimationCurve();

        [SerializeField]
        [Tooltip("Curve to sample from for bubble popping effect")]
        private AnimationCurve m_bubblePowerupPopCurve = new AnimationCurve();

        [SerializeField]
        [Tooltip("Curve to sample from for menu throbber colour lerp")]
        private AnimationCurve m_menuThrobberColourLerpCurve = new AnimationCurve();

        [SerializeField]
        [Tooltip("Curve to control the FTUE hand bounce")]
        private AnimationCurve m_FTUEHandBounceCurve = new AnimationCurve();

        [Tooltip("Curve to control the speed of the FTUE hand bounce over time")]
        [SerializeField]
        private AnimationCurve m_FTUEHandUrgencyOverTime = new AnimationCurve();

        [Tooltip("Curve to control the way the UI scale changer acts when scaling up")]
        [SerializeField]
        private AnimationCurve m_UIScaleChangerIncreaseCurve = new AnimationCurve();

        [Tooltip("Curve to control the way the UI scale changer acts when scaling down")]
        [SerializeField]
        private AnimationCurve m_UIScaleChangerDecreaseCurve = new AnimationCurve();

        [Header("Common Effect Curves")]
        [SerializeField]
        private AnimationCurve m_effectCurveSmooth = new AnimationCurve();

        [SerializeField]
        private AnimationCurve m_effectCurvePulse = new AnimationCurve();

        [Header("Panel Shine Effect Curves")]
        [SerializeField]
        private AnimationCurve m_panelShineLength = new AnimationCurve();

        [SerializeField]
        private AnimationCurve m_panelShineThickness = new AnimationCurve();

        [SerializeField]
        private AnimationCurve m_panelShineAlpha = new AnimationCurve();

        [SerializeField]
        [Header("Attack Feedback Curves")]
        private AnimationCurve m_attackFeedbackAlphaCurve = new AnimationCurve();

        [Tooltip("Disabled primary button background image")]
        [SerializeField]
        [Header("Disabled Effects")]
        private Sprite m_disabledButtonSprite;

        [SerializeField]
        [Tooltip("Disabled secondary button background image")]
        private Sprite m_disabledSecondaryButtonSprite;

        [SerializeField]
        [Tooltip("Disabled TextMeshPro Material Preset")]
        private Material m_disabledPrimaryTextMeshProMaterialPreset;

        [SerializeField]
        [Tooltip("Disabled TextMeshPro Material Preset")]
        private Material m_disabledSecondaryTextMeshProMaterialPreset;

        [Tooltip("Disabled button face colour")]
        [SerializeField]
        private Color m_disabledButtonFaceColour = Color.white;

        [Tooltip("Disabled button text colour")]
        [SerializeField]
        private Color m_disabledButtonTextColour = new Color(0.647058845f, 0.807843149f, 1f, 1f);

        [Tooltip("Disabled button text outline colour")]
        [SerializeField]
        private Color m_disabledButtonTextOutlineColour = new Color(0.0627450943f, 0.258823544f, 0.643137276f, 1f);

        [Tooltip("This is the start colour for any disabled gradient")]
        [SerializeField]
        private Color m_disabledGradientStartColour = Color.white;

        [Tooltip("This is the end colour for any disabled gradient")]
        [SerializeField]
        private Color m_disabledGradientEndColour = Color.white;

        [Tooltip("This value should match the Reference Pixels Per Unit used on the Canvas Scaler components")]
        [SerializeField]
        [Header("Global Values")]
        private float m_referencePixelsPerUnit = 100f;

        [Tooltip("This List Will Become A Dictionary of KeyVal Pairs for look up during runtime")]
        [SerializeField]
        [Header("Button Audio")]
        private List<HLAudioCueKVP> m_audioCueList = new List<HLAudioCueKVP>();

        private Dictionary<HLAudioTypes, AudioClip> m_audioCues = new Dictionary<HLAudioTypes, AudioClip>(HardlightUIEnumComparers.HLAudioTypeComparer);


        public Vector2 DisableWobbleScaler => m_disableWobbleScaler;

        protected override void Awake()
        {
            ProcessManager.GetSystem<HLUnityCore>().CustomKeyString("HLUIEffectManager", "Awake");
            base.Awake();
            AudioClip clip = null;
            for (int i = 0; i < m_audioCueList.Count; ++i)
            {
                if (!m_audioCues.TryGetValue(m_audioCueList[i].AudioType, out clip))
                    m_audioCues.Add(m_audioCueList[i].AudioType, m_audioCueList[i].AudioClip);
            }
        }

        protected override void OnDestroy()
        {
            HLUnityCore core = ProcessManager.GetSystemSafe<HLUnityCore>();
            if (core != null)
                core.CustomKeyString("HLUIEffectManager", "OnDestroy");
            base.OnDestroy();
        }

        public static float GetOrientationSpecificPageBleedSize(UIOrientation orientation)
        {
            return orientation == UIOrientation.Landscape
                ? SubPageLeftRightEdgeBufferPixelsLandscape
                : SubPageLeftRightEdgeBufferPixelsPortrait;
        }

        public void PlayAudioClip(HLAudioTypes audioType, float volume = 1f)
        {
            if (audioType == HLAudioTypes.None) return;
            if (m_audioCues.ContainsKey(audioType))
            {
                AudioClip clip = m_audioCues[audioType];
                if (clip != null)
                    UISoundEffectManager.Instance.PlayAudioClip(clip, volume);
            }
        }

        public void PlayAudioClipWithSemitoneOffset(HLAudioTypes audioType, int semitoneChange, float volume = 1f)
        {
            if (audioType == HLAudioTypes.None) return;
            if (m_audioCues.ContainsKey(audioType))
            {
                AudioClip clip = m_audioCues[audioType];
                if (clip != null)
                    UISoundEffectManager.Instance.PlayAudioClipWithSemitoneOffset(clip, semitoneChange, volume);
            }
        }

        public Sprite GetDisabledButtonBackground() => m_disabledButtonSprite;

        public Sprite GetDisabledSecondaryButtonBackground() => m_disabledSecondaryButtonSprite;

        public Material GetDisabledPrimaryTextMeshProMaterialPreset() => m_disabledPrimaryTextMeshProMaterialPreset;

        public Material GetDisabledSecondaryTextMeshProMaterialPreset() => m_disabledSecondaryTextMeshProMaterialPreset;

        public Color GetDisabledButtonFaceColour() => m_disabledButtonFaceColour;

        public Color GetDisabledTextColour() => m_disabledButtonTextColour;

        public Color GetDisabledTextOutlineColour() => m_disabledButtonTextOutlineColour;

        public Color GetDisabledGradientStartColour() => m_disabledGradientStartColour;

        public Color GetDisabledGradientEndColour() => m_disabledGradientEndColour;

        public static float GetCurveEndTime(AnimationCurve curve)
        {
            int length = curve.length;
            if (length == 0) return 0f;
            return curve[length - 1].time;
        }

        public float GetOnPressCurveEndTime() => GetCurveEndTime(m_onPressBounceCurve);

        public float GetOnReleaseCurveEndTime() => GetCurveEndTime(m_onReleaseBounceCurve);

        public float GetOnDisabledPressCurveEndTime() => GetCurveEndTime(m_onDisabledPressShakeCurve);

        public float GetOnEnableBounceEndTime() => GetCurveEndTime(m_onEnableBounceCurve);

        public float GetUIUnclampedSlideAnimationCurveEndTime() => GetCurveEndTime(m_uiUnclampedSlideAnimationCurve);

        public float GetUIClampedSlideAnimationCurveEndTime() => GetCurveEndTime(m_uiClampedSlideAnimationCurve);

        public float GetBubblePowerupPopCurveEndTime() => GetCurveEndTime(m_bubblePowerupPopCurve);

        public float GetMenuThrobberColourLerpCurveEndTime() => GetCurveEndTime(m_menuThrobberColourLerpCurve);

        public float GetFTUEHandBounceCurveEndTime() => GetCurveEndTime(m_FTUEHandBounceCurve);

        public float GetFTUEHandUrgencyCurveEndTime() => GetCurveEndTime(m_FTUEHandUrgencyOverTime);

        public AnimationCurve EffectCurveSmooth() => m_effectCurveSmooth;

        public AnimationCurve EffectCurvePulse() => m_effectCurvePulse;

        public AnimationCurve PanelShineLengthOverTime() => m_panelShineLength;

        public AnimationCurve PanelShineThicknessOverTime() => m_panelShineThickness;

        public AnimationCurve PanelShineAlphaOverTime() => m_panelShineAlpha;

        public float GetUIScaleChangerIncreaseCurveEndTime() => GetCurveEndTime(m_UIScaleChangerIncreaseCurve);

        public float GetUIScaleChangerDecreaseCurveEndTime() => GetCurveEndTime(m_UIScaleChangerDecreaseCurve);

        public float GetEffectCurveSmoothEndTime() => GetCurveEndTime(m_effectCurveSmooth);

        public float GetEffectCurvePulseEndTime() => GetCurveEndTime(m_effectCurvePulse);

        public float GetPanelShineLengthCurveEndTime() => GetCurveEndTime(m_panelShineLength);

        public float GetPanelShineThicknessCurveEndTime() => GetCurveEndTime(m_panelShineThickness);

        public float GetPanelShineAlphaCurveEndTime() => GetCurveEndTime(m_panelShineAlpha);

        public float GetAttackFeedbackAlphaCurveEndTime() => GetCurveEndTime(m_attackFeedbackAlphaCurve);

        public float SampleOnPressCurve(float time) => m_onPressBounceCurve.Evaluate(time);

        public float SampleOnReleaseCurve(float time) => m_onReleaseBounceCurve.Evaluate(time);

        public float SampleOnDisablePressCurve(float time) => m_onDisabledPressShakeCurve.Evaluate(time);

        public float SampleOnEnableBounceCurve(float time) => m_onEnableBounceCurve.Evaluate(time);

        public float SampleUIUnclampedSlideAnimationCurve(float time) => m_uiUnclampedSlideAnimationCurve.Evaluate(time);

        public float SampleUIClampedSlideAnimationCurve(float time) => m_uiClampedSlideAnimationCurve.Evaluate(time);

        public float SampleBubblePowerupPopCurve(float time) => m_bubblePowerupPopCurve.Evaluate(time);

        public float SampleMenuThrobberColourLerpCurve(float time) => m_menuThrobberColourLerpCurve.Evaluate(time);

        public float SampleFTUEHandBounceCurve(float time) => m_FTUEHandBounceCurve.Evaluate(time);

        public float SampleFTUEHandUrgencyCurve(float time) => m_FTUEHandUrgencyOverTime.Evaluate(time);

        public float SampleEffectCurveSmooth(float time) => m_effectCurveSmooth.Evaluate(time);

        public float SampleEffectCurvePulse(float time) => m_effectCurvePulse.Evaluate(time);

        public float SamplePanelShineLengthCurve(float time) => m_panelShineLength.Evaluate(time);

        public float SamplePanelShineThicknessCurve(float time) => m_panelShineThickness.Evaluate(time);

        public float SamplePanelShineAlphaCurve(float time) => m_panelShineAlpha.Evaluate(time);

        public float SampleUIScaleChangerIncreaseCurve(float time) => m_UIScaleChangerIncreaseCurve.Evaluate(time);

        public float SampleUIScaleChangerDecreaseCurve(float time) => m_UIScaleChangerDecreaseCurve.Evaluate(time);

        public float SampleAttackFeedbackAlphaCurve(float time) => m_attackFeedbackAlphaCurve.Evaluate(time);

        public float ReferencePixelsPerUnit
        {
            get { return m_referencePixelsPerUnit; }
            set { }
        }

        public HLUIEffectManager() { }

        [Serializable]
        private class HLAudioCueKVP
        {
            [SerializeField]
            private AudioClip m_audioClip;

            [HashEnum(typeof(HLAudioTypes))]
            [SerializeField]
            private HLAudioTypes m_audioType;

            public AudioClip AudioClip => m_audioClip;

            public HLAudioTypes AudioType => m_audioType;

            public HLAudioCueKVP() { }

        }
    }
}
