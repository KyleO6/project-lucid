// Reconstructed from complete shipping metadata and both native slices.
// Close uses the genuine DialogSystem wrapper; provider algorithms remain separate.
// Exact original C# spelling, inlining and native fault equivalence are unresolved.
using Unity.IL2CPP.CompilerServices;

namespace Hardlight
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class DialogPage : GuiPage
    {
        public override void Close()
        {
            DialogSystem.Instance.CloseCurrentPage();
        }

        protected override GuiPageTransition GetTransition()
        {
            return FadeTransition.Instance;
        }

        public DialogPage()
        {
        }
    }
}
