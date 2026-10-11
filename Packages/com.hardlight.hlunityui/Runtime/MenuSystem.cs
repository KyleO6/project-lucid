// Original sealed two-owner menu group; source-only, supplier/runtime holds remain.
// Static readonly initializer preserves BeforeFieldInit and stores only after constructor.
// Constructor subscribes after genuine PageStackSystem base; finalizer unsubscribes, with C# base-finalization finally.
// Move methods forward signed identifier bits/null data/callback/replace flag in native argument order.
// Four optional delegate defaults use metadata -1 sentinel; original closing callback ignores page and resets background unguarded.
// 0x06000247 .ctor ARM 0x1b52a08..0x1b52acc x86 0x1b4ae60..0x1b4af00
// 0x06000248 Finalize ARM 0x1b52acc..0x1b52bd4 x86 0x1b4af00..0x1b4b000
// 0x06000249 MoveToPage ARM 0x1b50f48..0x1b50fbc x86 0x1b494b0..0x1b49510
// 0x0600024a MoveToPage ARM 0x1b52bd4..0x1b52c4c x86 0x1b4b000..0x1b4b060
// 0x0600024b MoveToPageWithData ARM 0x1b52c4c..0x1b52cd0 x86 0x1b4b060..0x1b4b0d0
// 0x0600024c MoveToPreviousPage ARM 0x1b4eb9c..0x1b4ebf0 x86 0x1b47430..0x1b47470
// 0x0600024d CloseAllPages ARM 0x1b4ebf0..0x1b4ec4c x86 0x1b47470..0x1b474b0
// 0x0600024e CanLeaveCurrentPage ARM 0x1b52cd0..0x1b52d24 x86 0x1b4b0d0..0x1b4b110
// 0x0600024f AddPageBeforeCurrent ARM 0x1b52d24..0x1b52d98 x86 0x1b4b110..0x1b4b170
// 0x06000250 OnMenuPageClosing ARM 0x1b52d98..0x1b52e4c x86 0x1b4b170..0x1b4b200
// 0x06000251 .cctor ARM 0x1b52e4c..0x1b52eb4 x86 0x1b4b200..0x1b4b260
using Unity.IL2CPP.CompilerServices;

namespace Hardlight
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public sealed class MenuSystem : PageStackSystem<MenuPage>
    {
        public static readonly MenuSystem Instance = new MenuSystem();

        private MenuSystem()
        {
            OnGUIPageClosing += OnMenuPageClosing;
        }

        ~MenuSystem()
        {
            OnGUIPageClosing -= OnMenuPageClosing;
        }

        public void MoveToPage(MenuPageIdentifier identifier, OnPageLoaded onLoadedCallback = null)
        {
            AddPage((int)identifier, null, onLoadedCallback, false);
        }

        public void MoveToPage(MenuPageIdentifier identifier, bool replaceCurrent, OnPageLoaded onLoadedCallback = null)
        {
            AddPage((int)identifier, null, onLoadedCallback, replaceCurrent);
        }

        public void MoveToPageWithData(MenuPageIdentifier identifier, bool replaceCurrent, GuiPageParameters data, OnPageLoaded onLoadedCallback = null)
        {
            AddPage((int)identifier, data, onLoadedCallback, replaceCurrent);
        }

        public new void MoveToPreviousPage()
        {
            base.MoveToPreviousPage();
        }

        public new void CloseAllPages(UISystemClosureReason reasonForClosure)
        {
            base.CloseAllPages(reasonForClosure);
        }

        public new bool CanLeaveCurrentPage()
        {
            return base.CanLeaveCurrentPage();
        }

        public void AddPageBeforeCurrent(MenuPageIdentifier identifier, GuiPageParameters data, OnPageLoaded onLoadedCallback = null)
        {
            base.AddPageBeforeCurrent((int)identifier, data, onLoadedCallback);
        }

        private void OnMenuPageClosing(MenuPage page)
        {
            GuiBackgroundManager.Instance.SetUIBackgroundToDefault();
        }
    }
}
