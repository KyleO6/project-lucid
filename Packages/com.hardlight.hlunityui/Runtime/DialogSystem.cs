// Original HLUnityUI DialogSystem reconstructed from complete shipping metadata and both native slices.
// Ordinary private construction and base-page-stack forwarding are preserved.
// Original C# spelling, generated cctor layout, provider behavior and rebuilt runtime equivalence remain held.
using Unity.IL2CPP.CompilerServices;

namespace Hardlight
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public sealed class DialogSystem : PageStackSystem<DialogPage>
    {
        public static readonly DialogSystem Instance = new DialogSystem();

        private DialogSystem()
        {
        }

        public void ShowPage(DialogPageIdentifier identifier, GuiPageParameters data = null, OnPageLoaded onLoadedCallback = null)
        {
            AddPage((int)identifier, data, onLoadedCallback, false);
        }

        public void EnqueuePage(DialogPageIdentifier identifier, GuiPageParameters data = null, OnPageLoaded onLoadedCallback = null)
        {
            AddPageAtBase((int)identifier, data, onLoadedCallback);
        }

        public void EnqueuePageAfterCurrent(DialogPageIdentifier identifier, GuiPageParameters data = null, OnPageLoaded onLoadedCallback = null)
        {
            AddPageBeforeCurrent((int)identifier, data, onLoadedCallback);
        }

        public void CloseCurrentPage()
        {
            CloseCurrent(true, UISystemClosureReason.StandardClose);
        }

        public new void CloseAllPages(UISystemClosureReason reasonForClosure)
        {
            base.CloseAllPages(reasonForClosure);
        }

        public void CloseAllPagesExcept(DialogPageIdentifier identifier)
        {
            base.CloseAllPagesExcept((int)identifier);
        }

        public bool IsPageOpen(DialogPageIdentifier identifier)
        {
            return IsPageInStack((int)identifier);
        }
    }
}
