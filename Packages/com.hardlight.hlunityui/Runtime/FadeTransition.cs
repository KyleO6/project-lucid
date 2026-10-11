// Original HLUnityUI.Runtime FadeTransition and both natural coroutine owners, reconstructed from whole dual native ranges.
// Ordered-negative duration selects Dialog/Overlay/menu defaults; NaN overrides are returned unchanged. Menu field defaults retain original single bits 0x3eaaa64c.
// In clears alpha then yields once before interpolation. Out interpolates before its first yield. The continuation null/empty predicates differ in both original slices; their source parentheses and optimizer/fault equivalence remain inferred and held.
// Natural coroutine names/layout, inline property receiver reuse and Clamp01 spelling await distinct source/current emission qualification. No extra null guards, clamping of durations or service behavior added.
using System.Collections;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Hardlight
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FadeTransition : GuiPageTransition
    {
        public static FadeTransition Instance { get; private set; }

        [SerializeField]
        [Range(0f, 1f)]
        private float m_transitionInTimeMenu = 0.3333f;
        private const float TransitionInTimeDialog = 0.2f;
        private const float TransitionInTimeOverlay = 0.1f;
        [Range(0f, 1f)]
        [SerializeField]
        private float m_transitionOutTimeMenu = 0.3333f;
        private const float TransitionOutTimeDialog = 0.2f;
        private const float TransitionOutTimeOverlay = 0.1f;

        protected void Awake() { Instance = this; }

        private float GetTransitionInTime(GuiPage page)
        {
            if (page.ShowDuration < 0f)
            {
                if (page as DialogPage != null) return TransitionInTimeDialog;
                if (page as OverlayPage != null) return TransitionInTimeOverlay;
                return m_transitionInTimeMenu;
            }
            return page.ShowDuration;
        }

        private float GetTransitionOutTime(GuiPage page)
        {
            if (page.HideDuration < 0f)
            {
                if (page as DialogPage != null) return TransitionOutTimeDialog;
                if (page as OverlayPage != null) return TransitionOutTimeOverlay;
                return m_transitionOutTimeMenu;
            }
            return page.HideDuration;
        }

        protected override IEnumerator ProcessTransitionOut(GuiPage page)
        {
            if (page == null) yield break;
            if (page.PageCanvasGroups == null || page.PageCanvasGroups.Length == 0) yield break;
            CanvasGroup[] canvasGroups = page.PageCanvasGroups;
            float transitionTime = 0f;
            float transitionOutTimeRecip = 1f / GetTransitionOutTime(page);
            while (transitionTime < 1f)
            {
                if ((page == null && canvasGroups == null) || canvasGroups.Length == 0) yield break;
                transitionTime += Time.deltaTime * transitionOutTimeRecip;
                for (int i = 0; i < canvasGroups.Length; ++i)
                    canvasGroups[i].alpha = 1f - Mathf.Clamp01(transitionTime);
                yield return null;
            }
            if (page != null && canvasGroups != null && canvasGroups.Length != 0)
                for (int i = 0; i < canvasGroups.Length; ++i) canvasGroups[i].alpha = 0f;
        }

        protected override IEnumerator ProcessTransitionIn(GuiPage page)
        {
            if (page == null) yield break;
            if (page.PageCanvasGroups == null || page.PageCanvasGroups.Length == 0) yield break;
            CanvasGroup[] canvasGroups = page.PageCanvasGroups;
            float transitionTime = 0f;
            float transitionInTimeRecip = 1f / GetTransitionInTime(page);
            for (int i = 0; i < canvasGroups.Length; ++i) canvasGroups[i].alpha = 0f;
            yield return null;
            while (transitionTime < 1f)
            {
                if (page == null && (canvasGroups == null || canvasGroups.Length == 0)) yield break;
                transitionTime += Time.deltaTime * transitionInTimeRecip;
                for (int i = 0; i < canvasGroups.Length; ++i)
                    canvasGroups[i].alpha = Mathf.Clamp01(transitionTime);
                yield return null;
            }
            if (page != null && canvasGroups != null && canvasGroups.Length != 0)
                for (int i = 0; i < canvasGroups.Length; ++i) canvasGroups[i].alpha = 1f;
        }

        public FadeTransition() { }
    }
}
