// Original whole MenuPage; source-only, genuine GuiPage/Dialog/PageStack/Fade/Bloom suppliers held.
// Serialized fade flag defaults true before GuiPage base. Original restart reason skips only bloom, after base hide and event removals.
// Dialog subscriptions occur before base ShowComplete; removals follow base Hide. No idempotence/null repair.
// SetInteractable calls are a source reconstruction of inlined original GuiPage behavior: null array skip and field/length rereads.
// Default transition is FadeTransition.Instance, independent of the separate fade-to-white hide effect.
// 0x060001b8 Close ARM 0x1b4ea70..0x1b4eb9c x86 0x1b472f0..0x1b47430
// 0x060001b9 Show ARM 0x1b4ec4c..0x1b4ed48 x86 0x1b474b0..0x1b47580
// 0x060001ba Hide ARM 0x1b4ed48..0x1b4ef44 x86 0x1b47580..0x1b47750
// 0x060001bb ShowComplete ARM 0x1b4ef44..0x1b4f120 x86 0x1b47750..0x1b47910
// 0x060001bc GetTransition ARM 0x1b4f120..0x1b4f16c x86 0x1b47910..0x1b47950
// 0x060001bd DialogSystemOpenedHandler ARM 0x1b4f16c..0x1b4f1cc x86 0x1b47950..0x1b479b0
// 0x060001be DialogSystemClosedHandler ARM 0x1b4f1cc..0x1b4f22c x86 0x1b479b0..0x1b47a10
// 0x060001bf .ctor ARM 0x1b4f22c..0x1b4f238 x86 0x1b47a10..0x1b47a30
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace Hardlight
{
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.NullChecks, false)]
    public class MenuPage : GuiPage
    {
        [Tooltip("Whether this page should trigger a fade-to-white effect on hide")]
        [SerializeField]
        protected bool m_fadeTransition = true;

        public override void Close()
        {
            if (MenuSystem.Instance.CanMoveBack)
                MenuSystem.Instance.MoveToPreviousPage();
            else
                MenuSystem.Instance.CloseAllPages(UISystemClosureReason.StandardClose);
        }

        public override void Show(GuiPageParameters parameters)
        {
            base.Show(parameters);
            if (m_immediatelyInteractable && !DialogSystem.Instance.Empty)
                SetInteractable(false);
        }

        public override void Hide(UISystemClosureReason reasonForClosure)
        {
            base.Hide(reasonForClosure);
            DialogSystem.Instance.OnSystemOpened -= DialogSystemOpenedHandler;
            DialogSystem.Instance.OnSystemClosed -= DialogSystemClosedHandler;
            if (reasonForClosure != UISystemClosureReason.AppRestartClose && m_fadeTransition && MenuBloomTransition.NotNull())
                MenuBloomTransition.Instance.StartTransition(m_pageTransitionDelay, false);
        }

        protected override void ShowComplete()
        {
            DialogSystem.Instance.OnSystemOpened += DialogSystemOpenedHandler;
            DialogSystem.Instance.OnSystemClosed += DialogSystemClosedHandler;
            base.ShowComplete();
            if (!m_immediatelyInteractable)
                SetInteractable(DialogSystem.Instance.Empty);
        }

        protected override GuiPageTransition GetTransition()
        {
            return FadeTransition.Instance;
        }

        private void DialogSystemOpenedHandler()
        {
            SetInteractable(false);
        }

        private void DialogSystemClosedHandler()
        {
            SetInteractable(true);
        }

        public MenuPage()
        {
        }
    }
}
