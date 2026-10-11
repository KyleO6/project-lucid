// Original HLUnityUI.Runtime GuiInitializer, 02000045, plus natural iterator
// <OpenGuiPagesCoroutine>d__9, 02000046. Reconstructed from complete ARM64 and
// x86_64 native evidence, not supplied original C# spelling. All 12 methods are
// retained through six ordinary methods (including the ordinary implicit ctor)
// and the natural iterator. No helper, provider substitute or offline policy.
// OpenGuiPages reads Instance twice; the first owns StartCoroutine, the second
// owns the iterator. OpenAppLoadingPage captures MenuSystem before reading the
// initializer identifier and uses the genuine optional-null MoveToPage contract.
// Start retains original Conditional BUILD_DEVELOPMENT and only initializes
// GuiPageLoader then adjusts drag threshold. Unity dispatch/build effects held.
// The iterator dispatches all permanent loads in menu/dialog/overlay order,
// rereading each field after each call. Null initial arrays are skipped; provider
// mutation can fault later. It yields null once then invokes onComplete without
// a null guard. This is not evidence that pages have completed loading.
// DPI is acquired before EventSystem's Unity null comparison. EventSystem.current
// is reread for the assignment receiver; single division precedes multiplication.
// Preserve Mathf.RoundToInt and all nonfinite, overflow and zero-DPI behavior;
// original C# operand spelling, optimized implicit faults and cross-runtime
// conversions remain underdetermined. The ordinary ctor stores 8f/100f before
// its genuine MonoSingleton base call. Compilation/Engine/provider gates held.
using System;
using System.Collections;
using System.Diagnostics;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Hardlight
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class GuiInitializer : MonoSingleton<GuiInitializer>
    {
        [SerializeField]
        private MenuPageIdentifier m_appLoadingPage;

        [SerializeField]
        private MenuPageIdentifier[] m_alwaysLoadedMenuPages;

        [SerializeField]
        private DialogPageIdentifier[] m_alwaysLoadedDialogs;

        [SerializeField]
        private OverlayPageIdentifier[] m_alwaysLoadedOverlays;

        [SerializeField]
        private float defaultPixelDragThreshold = 8f;

        [SerializeField]
        private float defaultPixelDragDPI = 100f;

        public static void OpenGuiPages(Action onComplete)
        {
            Instance.StartCoroutine(Instance.OpenGuiPagesCoroutine(onComplete));
        }

        public static void OpenAppLoadingPage()
        {
            MenuSystem.Instance.MoveToPage(Instance.m_appLoadingPage);
        }

        [Conditional("BUILD_DEVELOPMENT")]
        protected void Start()
        {
            GuiPageLoader.InitialiseGUIPageLoader();
            FixPixelDragThreshold();
        }

        private IEnumerator OpenGuiPagesCoroutine(Action onComplete)
        {
            if (m_alwaysLoadedMenuPages != null)
            {
                for (int i = 0; i < m_alwaysLoadedMenuPages.Length; i++)
                    GuiPageLoader.LoadPermanently((int)m_alwaysLoadedMenuPages[i]);
            }

            if (m_alwaysLoadedDialogs != null)
            {
                for (int i = 0; i < m_alwaysLoadedDialogs.Length; i++)
                    GuiPageLoader.LoadPermanently((int)m_alwaysLoadedDialogs[i]);
            }

            if (m_alwaysLoadedOverlays != null)
            {
                for (int i = 0; i < m_alwaysLoadedOverlays.Length; i++)
                    GuiPageLoader.LoadPermanently((int)m_alwaysLoadedOverlays[i]);
            }

            yield return null;
            onComplete();
        }

        private void FixPixelDragThreshold()
        {
            float dpi = ScreenExtensions.DPI();
            if (EventSystem.current != null)
                EventSystem.current.pixelDragThreshold = Mathf.RoundToInt((dpi / defaultPixelDragDPI) * defaultPixelDragThreshold);
        }
    }
}
