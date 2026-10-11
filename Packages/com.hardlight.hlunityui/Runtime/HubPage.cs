// Original HLUnityUI.Runtime HubPage: TypeDef 02000038 and natural callback 02000039.
// Method evidence: 06000196..060001b1, complete ARM64 and x86_64 instruction streams.
// VisibleSubPages exposes the original mutable set. Load bookkeeping deliberately uses
// Dictionary.Add after List.Add; duplicate identifiers preserve that partial mutation.
// The callback relocates, initialises, unloads the registry page, then reports completion.
// Completion notifies a shown page before publishing it or assigning its HubPage.
// Automatic visibility depends on a non-null completion event; lookup is repeated.
// SupPageCentered retains its shipping spelling. DragHasStarted uses integer keys
// 0..Count-1, while hiding snapshots the visible identifiers before invoking callbacks.
// Unity null comparisons, dictionary alias faults and ordinary callback exceptions remain.
// The lambda preserves the natural family in source; exact generated field/owner/token
// emission and target-specific fault shape require separate current qualification.
// Genuine HubPageManager/SubPageRelocator suppliers, authored assets and Engine acceptance
// remain separate. This source performs no offline policy replacement.
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.IL2CPP.CompilerServices;

namespace Hardlight
{
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.NullChecks, false)]
    public class HubPage : GuiPageContent
    {
        [SerializeField] protected SubPageRelocator[] m_subPageRelocators;
        [SerializeField] protected UnityEvent m_onAllSubPagesLoaded;
        [SerializeField] private SubPageIdentifier[] m_subpagesToEnableOnLoaded;
        protected bool m_showComplete;
        protected Dictionary<int, SubPageContent> m_subPages = new Dictionary<int, SubPageContent>();
        private HashSet<int> m_visibleSubPages = new HashSet<int>();
        private List<int> m_loadingSubPages = new List<int>();
        private Dictionary<int, SubPageIdentifier> m_pageNumberToSubPageIdentifierLookup = new Dictionary<int, SubPageIdentifier>();

        public HashSet<int> VisibleSubPages
        {
            get { return m_visibleSubPages; }
        }

        public bool IsLoadingSubPage()
        {
            return m_loadingSubPages.Count > 0;
        }

        public void LoadSubpage(SubPageIdentifier subPageToLoad, int pageNumber, Transform parentTransform)
        {
            m_loadingSubPages.Add(pageNumber);
            m_pageNumberToSubPageIdentifierLookup.Add(pageNumber, subPageToLoad);
            GuiPageRegistry.Instance.LoadSubPage((int)subPageToLoad, page =>
            {
                SubPageContent content = page.RelocateToNewParent(parentTransform);
                content.Initialise();
                GuiPageRegistry.Instance.UnloadPage((int)subPageToLoad);
                SubPageLoadComplete(pageNumber, content);
            });
        }

        public void HideAllSubpages(UISystemClosureReason reasonForClosure)
        {
            OnHideAllVisibleSubPages(reasonForClosure);
        }

        public override void OnShow(GuiPageParameters parameters)
        {
            base.OnShow(parameters);
            HubPageManager.Instance.ActiveHubPage = this;
            m_showComplete = false;
        }

        public override void ShowComplete(GuiPageParameters parameters)
        {
            base.ShowComplete(parameters);
            m_showComplete = true;
            foreach (KeyValuePair<int, SubPageContent> page in m_subPages)
            {
                if (page.Value != null)
                    page.Value.OnHubPageShowComplete();
            }
        }

        public override void OnHide(GuiPageParameters parameters, UISystemClosureReason reasonForClosure)
        {
            base.OnHide(parameters, reasonForClosure);
            OnHideAllVisibleSubPages(reasonForClosure);
            if (HubPageManager.Instance.ActiveHubPage == this)
                HubPageManager.Instance.ActiveHubPage = null;
            m_showComplete = false;
        }

        public virtual void SubPageLoadComplete(int pageNumber, SubPageContent pageContent)
        {
            if (pageContent != null)
            {
                if (m_showComplete)
                    pageContent.OnHubPageShowComplete();
                m_subPages[pageNumber] = pageContent;
                m_subPages[pageNumber].HubPage = this;
            }
            m_loadingSubPages.Remove(pageNumber);
            if (m_loadingSubPages.Count == 0 && m_onAllSubPagesLoaded != null)
            {
                m_onAllSubPagesLoaded.Invoke();
                if (m_subpagesToEnableOnLoaded != null && m_subPageRelocators != null)
                {
                    for (int i = 0; i < m_subpagesToEnableOnLoaded.Length; i++)
                    {
                        if (GetSubPageNumber(m_subpagesToEnableOnLoaded[i]) >= 0)
                            ShowSubPage(GetSubPageNumber(m_subpagesToEnableOnLoaded[i]));
                    }
                }
            }
        }

        public virtual void SupPageCentered(int pageNumber)
        {
            SubPageContent page;
            if (m_subPages.TryGetValue(pageNumber, out page))
                page.OnCenter(null);
            HubPageManager.Instance.PageToLoadOnEnable = GetSubPageID(pageNumber);
        }

        public virtual void MoveToSubPage(SubPageIdentifier pageID)
        {
            MoveToSubPage(GetSubPageNumber(pageID));
        }

        public virtual void MoveToSubPage(SubPageIdentifier pageID, GuiPageParameters optionalParams)
        {
            MoveToSubPage(GetSubPageNumber(pageID), optionalParams);
        }

        public void MoveToSubPage(int pageNumber)
        {
            if (pageNumber < 0 || m_visibleSubPages.Contains(pageNumber))
                return;
            OnHideAllVisibleSubPages(UISystemClosureReason.StandardClose);
            ShowSubPage(pageNumber);
        }

        public void MoveToSubPage(int pageNumber, GuiPageParameters optionalParams)
        {
            if (pageNumber < 0 || m_visibleSubPages.Contains(pageNumber))
                return;
            OnHideAllVisibleSubPages(UISystemClosureReason.StandardClose);
            ShowSubPage(pageNumber, optionalParams);
        }

        public void ShowSubPage(int pageNumber, GuiPageParameters optionalParams = null)
        {
            if (!IsSubPageLoaded(pageNumber) || m_visibleSubPages.Contains(pageNumber))
                return;
            m_visibleSubPages.Add(pageNumber);
            if (!m_subPages[pageNumber].gameObject.activeSelf)
                m_subPages[pageNumber].gameObject.SetActive(true);
            m_subPages[pageNumber].OnShow(optionalParams);
        }

        public void EnterSubPage(int pageNumber, GuiPageParameters optionalParams = null)
        {
            if (IsSubPageLoaded(pageNumber))
                m_subPages[pageNumber].OnEnter(optionalParams);
        }

        public void ExitSubPage(int pageNumber, GuiPageParameters optionalParams = null)
        {
            if (IsSubPageLoaded(pageNumber))
                m_subPages[pageNumber].OnExit(optionalParams);
        }

        public void HideSubPage(int pageNumber, UISystemClosureReason reasonForClosure)
        {
            if (IsSubPageLoaded(pageNumber))
            {
                m_subPages[pageNumber].OnHide(null, reasonForClosure);
                m_visibleSubPages.Remove(pageNumber);
            }
        }

        public void AlterSubPageOpacity(int pageNumber, float opacity)
        {
            if (IsSubPageLoaded(pageNumber) && m_visibleSubPages.Contains(pageNumber))
                m_subPages[pageNumber].SetOpacity(opacity);
        }

        public void DragHasStarted()
        {
            for (int i = 0; i < m_subPages.Count; i++)
                m_subPages[i].DragHasStarted();
        }

        protected bool IsSubPageLoaded(int subPage)
        {
            return m_subPages.ContainsKey(subPage);
        }

        protected int GetSubPageNumber(SubPageIdentifier pageID)
        {
            if (m_subPageRelocators != null)
            {
                foreach (SubPageRelocator relocator in m_subPageRelocators)
                {
                    if (relocator.SubPageToLoad == pageID)
                        return relocator.PageNumber;
                }
            }
            return -1;
        }

        public SubPageIdentifier GetSubPageID(int pageNumber)
        {
            if (m_subPageRelocators != null)
            {
                foreach (SubPageRelocator relocator in m_subPageRelocators)
                {
                    if (relocator.PageNumber == pageNumber)
                        return relocator.SubPageToLoad;
                }
            }
            return SubPageIdentifier.None;
        }

        private void OnHideAllVisibleSubPages(UISystemClosureReason reasonForClosure)
        {
            int[] pages = new int[m_visibleSubPages.Count];
            m_visibleSubPages.CopyTo(pages);
            for (int i = 0; i < pages.Length; i++)
                HideSubPage(pages[i], reasonForClosure);
        }

        private void OnDestroy()
        {
            foreach (KeyValuePair<int, SubPageContent> page in m_subPages)
            {
                if (page.Value != null)
                    page.Value.Uninitialise();
            }
        }

        public bool IsSubPageVisible(SubPageIdentifier subPageIdentifier)
        {
            foreach (int pageNumber in m_visibleSubPages)
            {
                if (m_pageNumberToSubPageIdentifierLookup[pageNumber] == subPageIdentifier)
                    return true;
            }
            return false;
        }
    }
}
