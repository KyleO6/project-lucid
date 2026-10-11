// Reconstructed from complete shipping metadata and both native slices.
// HandleBackButton intentionally has an empty setter. Background/Menu alone subscribe.
// Public Content/IsShowing/SetInteractable source forms infer inlined genuine GuiPage bodies.
// LayersForTargets is a field initializer, preserving the original BeforeFieldInit role.
// Exact original C# spelling, mutation timing, generic dispatch and fault equivalence are held.
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace Hardlight
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class OverlayPage : GuiPage
    {
        public enum OverlayTarget
        {
            Background = 0,
            Menu = 1,
            Dialog = 2,
            Foreground = 3
        }

        [SerializeField]
        protected OverlayTarget m_overlayTarget;
        [SerializeField]
        private bool m_useFadeTransition = true;
        [SerializeField]
        private bool m_handleBackButton;
        public static readonly string[] LayersForTargets =
            new string[] { "BackgroundUI", "MenuUI", "DialogUI", "ForegroundUI" };

        public OverlayTarget Target
        {
            get { return m_overlayTarget; }
            set { m_overlayTarget = value; }
        }

        public bool HandleBackButton
        {
            get { return m_handleBackButton; }
            set { }
        }

        public override void Show(GuiPageParameters parameters)
        {
            if (IsShowing)
            {
                for (int i = 0; i < Content.Length; ++i)
                    Content[i].OnShow(parameters);
                return;
            }
            base.Show(parameters);
        }

        protected override void ShowComplete()
        {
            if (m_overlayTarget == OverlayTarget.Background || m_overlayTarget == OverlayTarget.Menu)
            {
                DialogSystem.Instance.OnSystemOpened += DialogSystemOpenedHandler;
                DialogSystem.Instance.OnSystemClosed += DialogSystemClosedHandler;
            }
            base.ShowComplete();
        }

        public override void Hide(UISystemClosureReason reasonForClosure)
        {
            if (m_overlayTarget == OverlayTarget.Background || m_overlayTarget == OverlayTarget.Menu)
            {
                DialogSystem.Instance.OnSystemOpened -= DialogSystemOpenedHandler;
                DialogSystem.Instance.OnSystemClosed -= DialogSystemClosedHandler;
            }
            base.Hide(reasonForClosure);
        }

        public override void Close()
        {
            OverlaySystem.Instance.HidePage(this);
        }

        private void DialogSystemOpenedHandler()
        {
            SetInteractable(false);
        }

        private void DialogSystemClosedHandler()
        {
            SetInteractable(true);
        }

        protected override GuiPageTransition GetTransition()
        {
            return m_useFadeTransition ? FadeTransition.Instance : null;
        }

        public OverlayPage()
        {
        }
    }
}
