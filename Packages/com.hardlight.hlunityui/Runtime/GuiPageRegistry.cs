// Original HLUnityUI.Runtime Hardlight.GuiPageRegistry (0x02000035), with
// both nested callback delegate owners. Recovered from the supplied release;
// no offline replacement or invented readiness behavior is present here.
//
// Loading IDs are appended before requesting a scene. Pending GUI callbacks
// are removed before invocation; pending subpage callbacks are invoked before
// removal. Exceptions and reentrant callback changes therefore remain distinct.
// UnregisterSubPage writes a null dictionary value without clearing unloading.
// UnloadAllPages clears page dictionaries/loading only, retaining unloading
// IDs and both pending callback lists. Unity Object comparisons remain intact.
//
// Four generic methods retain T : GuiPage. Registered OBJECT-shared native
// bodies corroborate their algorithms, but open/closed/shared identities remain
// separate evidence roles. Ordinary delegate declarations and field initializers
// express source intent; exact regenerated delegate/layout/native emission is held.
// The readonly singleton field preserves the original BeforeFieldInit owner.
// Genuine SubPage and Hardlight.Generics.Pair<T,U> implementations are required.

using System.Collections.Generic;
using Hardlight.Generics;
using Unity.IL2CPP.CompilerServices;

namespace Hardlight
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public sealed class GuiPageRegistry
    {
        public delegate void OnGuiPageLoadComplete(GuiPage page);
        public delegate void OnSubPageLoadComplete(SubPage page);

        public static readonly GuiPageRegistry Instance = new GuiPageRegistry();
        private List<int> s_loadingScenes = new List<int>();
        private List<int> s_unloadingScenes = new List<int>();
        private Dictionary<int, GuiPage> s_menuPages = new Dictionary<int, GuiPage>();
        private Dictionary<int, SubPage> s_subPages = new Dictionary<int, SubPage>();
        private List<Pair<int, OnGuiPageLoadComplete>> m_pendingGuiPageDelegates = new List<Pair<int, OnGuiPageLoadComplete>>();
        private List<Pair<int, OnSubPageLoadComplete>> m_pendingSubPageDelegates = new List<Pair<int, OnSubPageLoadComplete>>();

        public static int IdentifierToCRC(string identifier)
        {
            return HLCRC32.GenerateInt(identifier);
        }

        public void PrimePage(int identifier)
        {
            if (identifier == 0)
                return;

            if (IsUnloading(identifier) || (!IsLoaded(identifier) && !IsLoading(identifier)))
            {
                s_loadingScenes.Add(identifier);
                GuiPageLoader.LoadSceneFromValueAsync(identifier);
            }
        }

        public void UnloadAllPages()
        {
            for (int i = 0; i < s_loadingScenes.Count; ++i)
                GuiPageLoader.UnloadSceneFromValue(s_loadingScenes[i]);

            foreach (KeyValuePair<int, GuiPage> page in s_menuPages)
            {
                if (page.Value != null)
                    GuiPageLoader.UnloadSceneFromValue(page.Key);
            }

            foreach (KeyValuePair<int, SubPage> page in s_subPages)
            {
                if (page.Value != null)
                    GuiPageLoader.UnloadSceneFromValue(page.Key);
            }

            s_menuPages.Clear();
            s_subPages.Clear();
            s_loadingScenes.Clear();
        }

        public void LoadGuiPage<T>(int identifier, OnGuiPageLoadComplete onComplete, bool loadAsync = false) where T : GuiPage
        {
            if (s_menuPages.TryGetValue(identifier, out GuiPage page) && page != null && !IsUnloading(identifier))
            {
                if (onComplete != null)
                    onComplete(page);
                return;
            }

            if (onComplete != null)
                m_pendingGuiPageDelegates.Add(new Pair<int, OnGuiPageLoadComplete>(identifier, onComplete));

            if (!IsLoading(identifier))
            {
                s_loadingScenes.Add(identifier);
                if (loadAsync)
                    GuiPageLoader.LoadSceneFromValueAsync(identifier);
                else
                    GuiPageLoader.LoadSceneFromValue(identifier);
            }
        }

        public void LoadSubPage(int identifier, OnSubPageLoadComplete onComplete)
        {
            if (s_subPages.TryGetValue(identifier, out SubPage page) && page != null)
            {
                if (onComplete != null)
                    onComplete(page);
                return;
            }

            if (onComplete != null)
                m_pendingSubPageDelegates.Add(new Pair<int, OnSubPageLoadComplete>(identifier, onComplete));

            if (!IsLoading(identifier))
            {
                s_loadingScenes.Add(identifier);
                GuiPageLoader.LoadSceneFromValueAsync(identifier);
            }
        }

        public void UnloadPage(int identifier)
        {
            if (GuiPageLoader.UnloadSceneFromValue(identifier))
                s_unloadingScenes.Add(identifier);
        }

        public GuiPage GetGuiPageIfLoaded(int identifier)
        {
            s_menuPages.TryGetValue(identifier, out GuiPage page);
            return page;
        }

        public SubPage GetSubPageIfLoaded(int identifier)
        {
            s_subPages.TryGetValue(identifier, out SubPage page);
            return page;
        }

        public bool IsLoaded(int identifier)
        {
            s_menuPages.TryGetValue(identifier, out GuiPage page);
            if (page != null)
                return true;

            s_subPages.TryGetValue(identifier, out SubPage subPage);
            return subPage != null;
        }

        public bool IsLoading(int identifier)
        {
            return s_loadingScenes.Contains(identifier);
        }

        public bool IsUnloading(int identifier)
        {
            return s_unloadingScenes.Contains(identifier);
        }

        public T GetGuiPageIfLoaded<T>(int identifier) where T : GuiPage
        {
            s_menuPages.TryGetValue(identifier, out GuiPage page);
            return page as T;
        }

        public T GetGuiPageIfLoaded<T>() where T : GuiPage
        {
            foreach (GuiPage page in s_menuPages.Values)
            {
                T typedPage = page as T;
                if (typedPage != null)
                    return typedPage;
            }
            return null;
        }

        public List<T> GetGuiPages<T>() where T : GuiPage
        {
            List<T> pages = new List<T>();
            foreach (GuiPage page in s_menuPages.Values)
            {
                T typedPage = page as T;
                if (typedPage != null)
                    pages.Add(typedPage);
            }
            return pages;
        }

        public void RegisterGuiPage(GuiPage page)
        {
            int identifier = IdentifierToCRC(page.Identifier);
            s_menuPages[identifier] = page;
            s_loadingScenes.Remove(identifier);
            for (int i = 0; i < m_pendingGuiPageDelegates.Count;)
            {
                if (m_pendingGuiPageDelegates[i].First == identifier)
                {
                    OnGuiPageLoadComplete onComplete = m_pendingGuiPageDelegates[i].Second;
                    m_pendingGuiPageDelegates.RemoveAt(i);
                    onComplete(page);
                }
                else
                    ++i;
            }
        }

        public void RegisterSubPage(SubPage page)
        {
            int identifier = IdentifierToCRC(page.Identifier);
            s_subPages[identifier] = page;
            s_loadingScenes.Remove(identifier);
            for (int i = 0; i < m_pendingSubPageDelegates.Count;)
            {
                if (m_pendingSubPageDelegates[i].First == identifier)
                {
                    m_pendingSubPageDelegates[i].Second(page);
                    m_pendingSubPageDelegates.RemoveAt(i);
                }
                else
                    ++i;
            }
        }

        public void UnregisterGuiPage(GuiPage page)
        {
            int identifier = IdentifierToCRC(page.Identifier);
            s_menuPages[identifier] = null;
            s_unloadingScenes.Remove(identifier);
        }

        public void UnregisterSubPage(SubPage page)
        {
            s_subPages[IdentifierToCRC(page.Identifier)] = null;
        }

        private GuiPageRegistry()
        {
        }
    }
}
