// Original HLUnityUI.Runtime SubPageRelocator02000040 plus natural <>c02000041.
// Start is genuinely public and uses the serialized hub after Unity null checking.
// Receiver/identifier/page-number evaluation precedes normal transform lookup.
// OnSubPageLoaded remains a mutable public Action field, initially an empty delegate.
// Its real natural singleton/cctor/empty callback are retained by ordinary lambda source.
// No callback invocation or readiness guarantee is added here. Original HashEnum
// encodes actual SubPageIdentifier type28422; exact ordered attributes are preserved.
// Natural parameter spellings <p0>/<p1> and generated names/layout/tokens require
// separate current-emission qualification; no fabricated explicit owner replaces them.
using System;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Hardlight.Utils;

namespace Hardlight
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SubPageRelocator : MonoBehaviour
    {
        public static Action<int, GuiPageContent> OnSubPageLoaded = (p0, p1) => { };

        [SerializeField, HashEnum(typeof(SubPageIdentifier))]
        private SubPageIdentifier m_subPageToLoad;
        [SerializeField] private int m_pageNumber;
        [SerializeField] private HubPage m_hubPage;

        public SubPageIdentifier SubPageToLoad
        {
            get { return m_subPageToLoad; }
        }

        public int PageNumber
        {
            get { return m_pageNumber; }
        }

        public void Start()
        {
            if (m_hubPage == null)
                return;
            m_hubPage.LoadSubpage(m_subPageToLoad, m_pageNumber, transform);
        }
    }
}
