// Original delegates retain the literal OnTranstion spelling. Both wrappers
// start the private iterator, which yields the virtual Process coroutine, then
// checks callback before Unity page inequality. No page validity check is added
// before virtual processing, and no fault or callback recovery is synthesized.
// Two abstract slots have zero native definition pointers; natural iterator
// and delegate implementation layouts remain compiler/source-form holds.
using System.Collections;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Hardlight
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public abstract class GuiPageTransition : MonoBehaviour
    {
        public delegate void OnTranstionOutFinished();
        public delegate void OnTranstionInFinished();

        public void StartTransitionOut(GuiPage page, OnTranstionOutFinished callback)
        {
            StartCoroutine(StartTransitionOutCoroutine(page, callback));
        }

        public void StartTransitionIn(GuiPage page, OnTranstionInFinished callback)
        {
            StartCoroutine(StartTransitionInCoroutine(page, callback));
        }

        private IEnumerator StartTransitionOutCoroutine(GuiPage page, OnTranstionOutFinished callback)
        {
            yield return StartCoroutine(ProcessTransitionOut(page));
            if (callback != null && page != null)
            {
                callback();
            }
        }

        private IEnumerator StartTransitionInCoroutine(GuiPage page, OnTranstionInFinished callback)
        {
            yield return StartCoroutine(ProcessTransitionIn(page));
            if (callback != null && page != null)
            {
                callback();
            }
        }

        protected abstract IEnumerator ProcessTransitionOut(GuiPage page);
        protected abstract IEnumerator ProcessTransitionIn(GuiPage page);

        protected GuiPageTransition()
        {
        }
    }
}
