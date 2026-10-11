// Original HLUnityUI.Runtime source preservation; source form, providers, native faults and Engine parity remain held.
// Original owner and literal natural iterator <ApplyCurrentCustomisation>d__15. Bool argument is ignored; iterator returns false without yielding true. Shrink-path current[i] fault is preserved. Callback loops reread their array field; asset.name is used separately for AddClip and Play.
// 0x0600042f Awake arm64 0x1b638d4..0x1b638d8; x86_64 0x1b5b250..0x1b5b260.
// 0x06000430 SetButtonState arm64 0x1b61f08..0x1b62124; x86_64 0x1b59a30..0x1b59c00.
// 0x06000431 SetButtonCanvasLayer arm64 0x1b63b3c..0x1b63b94; x86_64 0x1b5b470..0x1b5b4c0.
// 0x06000432 SetButtonCanvasLayer arm64 0x1b61ea0..0x1b61ef8; x86_64 0x1b599c0..0x1b59a10.
// 0x06000433 ResetCanvasLayer arm64 0x1b61ef8..0x1b61f08; x86_64 0x1b59a10..0x1b59a30.
// 0x06000434 ApplyCurrentCustomisation arm64 0x1b63ac4..0x1b63b3c; x86_64 0x1b5b400..0x1b5b470.
// 0x06000435 ReactivateBehaviours arm64 0x1b60a04..0x1b60a9c; x86_64 0x1b58550..0x1b585e0.
// 0x06000436 ResetAnchorIfNeeded arm64 0x1b638d8..0x1b63ac4; x86_64 0x1b5b260..0x1b5b400.
// 0x06000437 .ctor arm64 0x1b63bc0..0x1b63cc4; x86_64 0x1b5b4e0..0x1b5b5d0.
// 0x06000438 Deactivate arm64 0x1b63cc4..0x1b63d30; x86_64 0x1b5b5d0..0x1b5b630.
// 0x06000439 Activate arm64 0x1b63d30..0x1b63d9c; x86_64 0x1b5b630..0x1b5b690.
// 0x0600043a OnHighlightBegin arm64 0x1b63d9c..0x1b63e10; x86_64 0x1b5b690..0x1b5b6f0.
// 0x0600043b OnHighlightUpdated arm64 0x1b63e10..0x1b63e94; x86_64 0x1b5b6f0..0x1b5b770.
// 0x0600043c OnHighlightEnd arm64 0x1b63e94..0x1b63f08; x86_64 0x1b5b770..0x1b5b7d0.
// 0x0600043d .ctor arm64 0x1b63f08..0x1b63f10; x86_64 0x1b5b7d0..0x1b5b7e0.
// 0x0600043e .ctor arm64 0x1b63b94..0x1b63bc0; x86_64 0x1b5b4c0..0x1b5b4e0.
// 0x0600043f System.IDisposable.Dispose arm64 0x1b63f10..0x1b63f14; x86_64 0x1b5b7e0..0x1b5b7f0.
// 0x06000440 MoveNext arm64 0x1b63f14..0x1b644b4; x86_64 0x1b5b7f0..0x1b5be80.
// 0x06000441 System.Collections.Generic.IEnumerator<System.Object>.get_Current arm64 0x1b644b4..0x1b644bc; x86_64 0x1b5be80..0x1b5be90.
// 0x06000442 System.Collections.IEnumerator.Reset arm64 0x1b644bc..0x1b644fc; x86_64 0x1b5be90..0x1b5bed0.
// 0x06000443 System.Collections.IEnumerator.get_Current arm64 0x1b644fc..0x1b64504; x86_64 0x1b5bed0..0x1b5bee0.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace Hardlight
{
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.NullChecks, false)]
    public class HLButtonHighlighter : MonoBehaviour
    {
        [Serializable]
        public class ButtonHighlightChangeTargets
        {
            public ModifiedOnUISelection[] UISelectionModifications;

            public void Deactivate(HLButtonHighlightCustomisations.ButtonHighlightChangeSource source)
            {
                for (int i = 0; i < UISelectionModifications.Length; ++i)
                    UISelectionModifications[i].OnDeactivate(source);
            }

            public void Activate(HLButtonHighlightCustomisations.ButtonHighlightChangeSource source)
            {
                for (int i = 0; i < UISelectionModifications.Length; ++i)
                    UISelectionModifications[i].OnActivate(source);
            }

            public void OnHighlightBegin(HLButtonHighlightCustomisations.ButtonHighlightChangeSource previousSource, HLButtonHighlightCustomisations.ButtonHighlightChangeSource currentSource)
            {
                for (int i = 0; i < UISelectionModifications.Length; ++i)
                    UISelectionModifications[i].OnHighlightBegin(previousSource, currentSource);
            }

            public void OnHighlightUpdated(HLButtonHighlightCustomisations.ButtonHighlightChangeSource previousSource, HLButtonHighlightCustomisations.ButtonHighlightChangeSource currentSource, float lerpValue)
            {
                for (int i = 0; i < UISelectionModifications.Length; ++i)
                    UISelectionModifications[i].OnHighlightUpdated(previousSource, currentSource, lerpValue);
            }

            public void OnHighlightEnd(HLButtonHighlightCustomisations.ButtonHighlightChangeSource previousSource, HLButtonHighlightCustomisations.ButtonHighlightChangeSource currentSource)
            {
                for (int i = 0; i < UISelectionModifications.Length; ++i)
                    UISelectionModifications[i].OnHighlightEnd(previousSource, currentSource);
            }
        }

        [SerializeField] private Canvas m_canvas;
        [SerializeField] private HLButtonHighlightCustomisations m_defaultCustomisation;
        [SerializeField] private List<ButtonHighlightChangeTargets> m_customisableLayers;
        [SerializeField] private Animation m_customisableAnimator;
        [SerializeField] private List<Behaviour> m_requiredComponents = new List<Behaviour>();
        private List<HLButtonHighlightCustomisations.ButtonHighlightChangeSource> m_currentChangeSource = new List<HLButtonHighlightCustomisations.ButtonHighlightChangeSource>();
        private List<HLButtonHighlightCustomisations.ButtonHighlightChangeSource> m_cachedChangeSource = new List<HLButtonHighlightCustomisations.ButtonHighlightChangeSource>();
        private HLButtonHighlightCustomisations m_cachedCustomisation;
        private Coroutine m_coroutine;

        private void Awake()
        {
        }

        public void SetButtonState(HLButtonHighlightCustomisations state, bool setInstantTransition)
        {
            ResetAnchorIfNeeded();
            if (state == null)
                state = m_defaultCustomisation;
            m_cachedCustomisation = state;
            m_cachedChangeSource.Clear();
            m_cachedChangeSource.AddRange(m_currentChangeSource);
            m_currentChangeSource.Clear();
            m_currentChangeSource.AddRange(m_cachedCustomisation.m_buttonHighlightLayerData);
            if (gameObject.activeSelf && gameObject.activeInHierarchy)
            {
                if (m_coroutine != null)
                    this.SafeStopCoroutine(ref m_coroutine);
                m_coroutine = StartCoroutine(ApplyCurrentCustomisation(setInstantTransition));
            }
        }

        public void SetButtonCanvasLayer(int sortingLayer, int sortingOrder = 0)
        {
            m_canvas.overrideSorting = true;
            m_canvas.sortingLayerID = sortingLayer;
            m_canvas.sortingOrder = sortingOrder;
        }

        public void SetButtonCanvasLayer(string sortingLayer, int sortingOrder = 0)
        {
            m_canvas.overrideSorting = true;
            m_canvas.sortingLayerName = sortingLayer;
            m_canvas.sortingOrder = sortingOrder;
        }

        public void ResetCanvasLayer()
        {
            m_canvas.overrideSorting = false;
        }

        private IEnumerator ApplyCurrentCustomisation(bool setInstantTransition)
        {
            m_customisableAnimator.Stop();
            for (int i = 0; i < m_cachedChangeSource.Count; ++i)
            {
                if (i >= m_currentChangeSource.Count)
                    m_customisableLayers[i].Deactivate(m_currentChangeSource[i]);
            }
            for (int i = 0; i < m_currentChangeSource.Count; ++i)
            {
                if (i >= m_cachedChangeSource.Count)
                {
                    m_cachedChangeSource.Add(new HLButtonHighlightCustomisations.ButtonHighlightChangeSource());
                    m_customisableLayers[i].Activate(m_currentChangeSource[i]);
                }
            }
            for (int i = 0; i < m_currentChangeSource.Count; ++i)
                m_customisableLayers[i].OnHighlightBegin(m_cachedChangeSource[i], m_currentChangeSource[i]);
            for (int i = 0; i < m_currentChangeSource.Count; ++i)
                m_customisableLayers[i].OnHighlightUpdated(m_cachedChangeSource[i], m_currentChangeSource[i], 1f);
            for (int i = 0; i < m_currentChangeSource.Count; ++i)
                m_customisableLayers[i].OnHighlightEnd(m_cachedChangeSource[i], m_currentChangeSource[i]);
            if (m_cachedCustomisation.m_animationClip != null)
            {
                m_customisableAnimator.AddClip(m_cachedCustomisation.m_animationClip, m_cachedCustomisation.name);
                m_customisableAnimator.wrapMode = m_cachedCustomisation.m_animationWrap;
                m_customisableAnimator.Play(m_cachedCustomisation.name);
            }
            yield break;
        }

        public void ReactivateBehaviours()
        {
            for (int i = 0; i < m_requiredComponents.Count; ++i)
                m_requiredComponents[i].enabled = true;
        }

        private void ResetAnchorIfNeeded()
        {
            RectTransform rect = transform as RectTransform;
            if (rect.anchorMin != Vector2.zero)
                rect.anchorMin = Vector2.zero;
            if (rect.anchorMax != Vector2.one)
                rect.anchorMax = Vector2.one;
            if (rect.pivot != Vector2.zero)
                rect.pivot = Vector2.zero;
        }
    }
}
