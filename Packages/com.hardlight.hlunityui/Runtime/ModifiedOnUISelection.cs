// Original shipping ModifiedOnUISelection preservation; source form reconstructed from metadata and both native slices.
// No compiler, Unity, provider or original native body executed; current emission and runtime parity held.
// Four concrete virtual callbacks are RET-only; Updated remains abstract with zero native pointers.
// The protected constructor retains genuine MonoBehaviour base construction. No fake override/provider.
// 0x06000481 OnHighlightBegin: empty callback/base ctor.
// 0x06000482 OnHighlightUpdated: abstract; no body.
// 0x06000483 OnHighlightEnd: empty callback/base ctor.
// 0x06000484 OnActivate: empty callback/base ctor.
// 0x06000485 OnDeactivate: empty callback/base ctor.
// 0x06000486 .ctor: empty callback/base ctor.
using UnityEngine;

namespace Hardlight
{
    public abstract class ModifiedOnUISelection : MonoBehaviour
    {
        public virtual void OnHighlightBegin(HLButtonHighlightCustomisations.ButtonHighlightChangeSource previousSource, HLButtonHighlightCustomisations.ButtonHighlightChangeSource currentSource)
        {
        }

        public abstract void OnHighlightUpdated(HLButtonHighlightCustomisations.ButtonHighlightChangeSource previousSource, HLButtonHighlightCustomisations.ButtonHighlightChangeSource currentSource, float lerpValue);

        public virtual void OnHighlightEnd(HLButtonHighlightCustomisations.ButtonHighlightChangeSource previousSource, HLButtonHighlightCustomisations.ButtonHighlightChangeSource currentSource)
        {
        }

        public virtual void OnActivate(HLButtonHighlightCustomisations.ButtonHighlightChangeSource source)
        {
        }

        public virtual void OnDeactivate(HLButtonHighlightCustomisations.ButtonHighlightChangeSource source)
        {
        }

        protected ModifiedOnUISelection()
        {
        }

    }
}
