// Original HLUnityUI.Runtime HLCurveAnimatedGraphic 0x020000dc; whole 25 ordinary methods,
// nested AnimationState 0x020000dd and four natural iterators 0x020000de..e1 (24 methods).
// Reconstructed from complete ARM64/x86_64 shipping instructions and raw declarations.
// Original source spelling, optimized fault timing and regenerated iterator layout remain held.
// Three animation bodies always update before their first yield; ordered timer < endTime
// ends on unordered NaN. Disabled restart is checked only after yield, before comparison.
// The cooldown iterator has empty Dispose: cancellation does not clear the global flag.
// The inherited transition test reads previous SelectionState, not Unity Selectable.transition.
// Ordinary constructor initializers run before the genuine HLSelectable base constructor.
// Genuine curve, status, haptics, scheduling and Unity providers are required and unexecuted.
// Inferred x/y locals preserve the observed sample calls before target/cached-position reads.
using System.Collections;
using System.Collections.Generic;
using Hardlight.Utils;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Hardlight
{
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.NullChecks, false)]
    public class HLCurveAnimatedGraphic : HLSelectable, IPointerClickHandler, IEventSystemHandler, ISubmitHandler
    {
        private static bool s_globalInputOnCooldown;
        [SerializeField] protected bool m_hasPressCooldown = true;
        [SerializeField] protected float m_cooldownTimeInSeconds = 0.15f;
        [SerializeField] protected bool m_animationDisabled;
        [SerializeField] protected bool m_allowMultiTouchClickthrough;
        [SerializeField] protected HLCurveAnimatedGraphicCurveCollection m_curveCollection;
        [SerializeField, HashEnum(typeof(VibrationType))] protected VibrationType m_hapticOnPress;
        [HashEnum(typeof(VibrationType)), SerializeField] protected VibrationType m_hapticOnRelease;
        [SerializeField, HashEnum(typeof(VibrationType))] protected VibrationType m_hapticOnDisabled;
        protected Vector3 m_cachedLocalPosition;
        protected Vector3 m_cachedLocalScale;
        protected Vector3 m_cachedLocalRotation;
        protected bool m_hasCached;
        protected bool m_clickAccepted;
        protected bool m_shouldResetInactiveAnim;
        protected bool m_onCooldown;
        protected bool m_hasStatusEffects;
        protected List<ButtonStatusEffects> m_buttonStatusEffects = new List<ButtonStatusEffects>();
        protected AnimationState m_currentAnimatingState;
        protected Coroutine m_animatingCoroutine;
        protected static Coroutine m_cooldownCoroutine;
        protected Coroutine m_waitForUICacheLocalTransformCoroutine;
        protected WaitForSeconds m_wait;
        private SystemRef<VibrationManager> m_vibrationManager;

        public enum AnimationState
        {
            NotAnimating = 0,
            AnimatingPress = 1,
            AnimatingRelease = 2,
            AnimatingDisabled = 3
        }

        public VibrationType HapticOnPress { get { return m_hapticOnPress; } }
        public VibrationType HapticOnRelease { get { return m_hapticOnRelease; } }
        public VibrationType HapticOnDisabled { get { return m_hapticOnDisabled; } }
        public float CooldownTimeInSeconds { get { return m_cooldownTimeInSeconds; } }
        public bool AnimationDisabled
        {
            get { return m_animationDisabled; }
            set { m_animationDisabled = value; }
        }

        protected override void Awake()
        {
            base.Awake();
            m_vibrationManager = ProcessManager.GetSystemRef<VibrationManager>(null, true);
            if (m_hasPressCooldown) m_wait = new WaitForSeconds(m_cooldownTimeInSeconds);
            m_onCooldown = false;
        }

        protected override void OnEnable()
        {
            if (Application.isPlaying)
                m_waitForUICacheLocalTransformCoroutine = CoroutineUtils.WaitForUI(CacheLocalTransform);
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            m_onCooldown = false;
            this.SafeStopCoroutine(ref m_animatingCoroutine);
            CoroutineUtils.StopUtilCoroutine(ref m_waitForUICacheLocalTransformCoroutine);
            m_currentAnimatingState = AnimationState.NotAnimating;
            ResetTransformToCache();
        }

        private void CacheLocalTransform()
        {
            m_cachedLocalPosition = m_targetTransform.localPosition;
            m_cachedLocalScale = m_targetTransform.localScale;
            m_cachedLocalRotation = m_targetTransform.localEulerAngles;
            m_hasCached = true;
        }

        private void ResetTransformToCache()
        {
            if (Application.isPlaying && m_hasCached)
            {
                m_targetTransform.localPosition = m_cachedLocalPosition;
                m_targetTransform.localScale = m_cachedLocalScale;
                m_targetTransform.localEulerAngles = m_cachedLocalRotation;
            }
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (m_allowMultiTouchClickthrough || eventData.pointerId == -1 || eventData.pointerId == 0)
            {
                Press();
                if (IsActive())
                {
                    EvaluateAndTransitionToSelectionState(eventData);
                    m_clickAccepted = false;
                }
            }
        }

        public virtual void OnSubmit(BaseEventData eventData)
        {
            Press();
            if (IsActive() && IsInteractable()) EvaluateAndTransitionToSelectionState(eventData);
        }

        public virtual bool Press()
        {
            if (!IsActive() || !IsInteractable() || s_globalInputOnCooldown)
            {
                m_clickAccepted = false;
                if (m_animatingCoroutine != null)
                {
                    if (m_currentAnimatingState == AnimationState.AnimatingDisabled)
                    {
                        m_shouldResetInactiveAnim = true;
                        return m_clickAccepted;
                    }
                    StopAnimationCoroutine();
                }
                if (!m_animationDisabled)
                {
                    ResetTransformToCache();
                    m_animatingCoroutine = StartCoroutine(AnimateOnPressDisabled());
                }
                return m_clickAccepted;
            }
            m_clickAccepted = m_currentAnimatingState != AnimationState.AnimatingDisabled;
            if (m_hasPressCooldown && gameObject.activeInHierarchy && m_clickAccepted)
            {
                CoroutineUtils.StopUtilCoroutine(ref m_cooldownCoroutine);
                m_cooldownCoroutine = CoroutineUtils.RunCoroutine(GlobalCooldownClicks());
            }
            return m_clickAccepted;
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, false);
            switch (state)
            {
                case SelectionState.Normal:
                    if (m_previousSelectionState == SelectionState.Highlighted && m_currentAnimatingState != AnimationState.AnimatingRelease)
                    {
                        StopAnimationCoroutine();
                        ResetTransformToCache();
                    }
                    break;
                case SelectionState.Highlighted:
                    if (m_currentAnimatingState != AnimationState.AnimatingDisabled && m_previousSelectionState == SelectionState.Pressed &&
                        gameObject.activeInHierarchy && !m_animationDisabled)
                    {
                        StopAnimationCoroutine();
                        m_animatingCoroutine = StartCoroutine(AnimateOnRelease(instant));
                    }
                    break;
                case SelectionState.Pressed:
                    if (m_currentAnimatingState != AnimationState.AnimatingDisabled && !m_onCooldown)
                    {
                        StopAnimationCoroutine();
                        if (gameObject.activeInHierarchy && !m_animationDisabled)
                        {
                            m_animatingCoroutine = StartCoroutine(AnimateOnPress(instant));
                            m_clickAccepted = false;
                        }
                    }
                    break;
                case SelectionState.Disabled:
                    if (m_previousSelectionState != SelectionState.Disabled)
                    {
                        if (m_clickAccepted)
                        {
                            StopAnimationCoroutine();
                            if (gameObject.activeInHierarchy && !m_animationDisabled)
                            {
                                m_animatingCoroutine = StartCoroutine(AnimateOnRelease(instant));
                                break;
                            }
                        }
                        ResetTransformToCache();
                    }
                    break;
            }
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            if (Application.isPlaying && m_targetTransform != null) CacheLocalTransform();
        }

        protected IEnumerator AnimateOnPress(bool instant = false)
        {
            m_currentAnimatingState = AnimationState.AnimatingPress;
            m_vibrationManager.Get().Vibrate(m_hapticOnPress);
            float endTime = Mathf.Max(m_curveCollection.OnPressedXCurve.EndTime(), m_curveCollection.OnPressedYCurve.EndTime());
            float timer = instant ? endTime : 0f;
            do
            {
                timer += Time.unscaledDeltaTime;
                float x = m_curveCollection.OnPressedXCurve.SampleAtTime(timer);
                float y = m_curveCollection.OnPressedYCurve.SampleAtTime(timer);
                m_targetTransform.localScale = new Vector3(x, y, 1f);
                yield return null;
            } while (timer < endTime);
            m_currentAnimatingState = AnimationState.NotAnimating;
            m_animatingCoroutine = null;
        }

        protected IEnumerator AnimateOnRelease(bool instant = false)
        {
            m_currentAnimatingState = AnimationState.AnimatingRelease;
            m_vibrationManager.Get().Vibrate(m_hapticOnRelease);
            float endTime = Mathf.Max(m_curveCollection.OnReleasedXCurve.EndTime(), m_curveCollection.OnReleasedYCurve.EndTime());
            float timer = instant ? endTime : 0f;
            do
            {
                timer += Time.unscaledDeltaTime;
                float x = m_curveCollection.OnReleasedXCurve.SampleAtTime(timer);
                float y = m_curveCollection.OnReleasedYCurve.SampleAtTime(timer);
                m_targetTransform.localScale = new Vector3(x, y, 1f);
                yield return null;
            } while (timer < endTime);
            m_currentAnimatingState = AnimationState.NotAnimating;
            m_animatingCoroutine = null;
        }

        protected IEnumerator AnimateOnPressDisabled(bool instant = false)
        {
            m_currentAnimatingState = AnimationState.AnimatingDisabled;
            m_vibrationManager.Get().Vibrate(m_hapticOnDisabled);
            m_cachedLocalPosition = m_targetTransform.localPosition;
            float endTime = Mathf.Max(m_curveCollection.OnDisabledXCurve.EndTime(), m_curveCollection.OnDisabledYCurve.EndTime());
            float timer = instant ? endTime : 0f;
            Vector2 scaler = m_curveCollection.OnDisabledAnimDirection;
            do
            {
                timer += Time.unscaledDeltaTime;
                float x = m_curveCollection.OnDisabledXCurve.SampleAtTime(timer);
                float y = m_curveCollection.OnDisabledYCurve.SampleAtTime(timer);
                m_targetTransform.localPosition = m_cachedLocalPosition + new Vector3(x * scaler.x, y * scaler.y, 0f);
                yield return null;
                if (m_shouldResetInactiveAnim)
                {
                    timer = 0f;
                    m_shouldResetInactiveAnim = false;
                }
            } while (timer < endTime);
            m_targetTransform.localPosition = m_cachedLocalPosition;
            m_currentAnimatingState = AnimationState.NotAnimating;
            m_animatingCoroutine = null;
        }

        private IEnumerator GlobalCooldownClicks()
        {
            s_globalInputOnCooldown = true;
            yield return CoroutineUtils.WaitForRealSeconds(m_cooldownTimeInSeconds);
            s_globalInputOnCooldown = false;
        }

        protected void StopAnimationCoroutine()
        {
            this.SafeStopCoroutine(ref m_animatingCoroutine);
            m_currentAnimatingState = AnimationState.NotAnimating;
        }

        public static void CancelGlobalInputCooldown()
        {
            s_globalInputOnCooldown = false;
        }

        public void RegisterButtonStatusEffector(ButtonStatusEffects effector)
        {
            if (m_buttonStatusEffects.Contains(effector)) return;
            m_buttonStatusEffects.Add(effector);
            m_hasStatusEffects = m_buttonStatusEffects.Count > 0;
            TriggerButtonStatusAffectors();
        }

        protected void TriggerButtonStatusAffectors()
        {
            if (!m_hasStatusEffects) return;
            for (int i = 0; i < m_buttonStatusEffects.Count; i++)
            {
                if (m_interactable) m_buttonStatusEffects[i].SetEnabledButtonState();
                else m_buttonStatusEffects[i].SetDisabledButtonState();
            }
        }

        public HLCurveAnimatedGraphic() { }
    }
}
