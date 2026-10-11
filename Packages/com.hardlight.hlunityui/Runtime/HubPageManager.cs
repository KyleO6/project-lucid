// Original HLUnityUI.Runtime HubPageManager0200003a: all six methods and two fields.
// Plain requested-identifier storage and automatic ActiveHubPage storage stay original.
// IsSubPageVisible uses Unity null semantics, rereads the active property, then calls
// the genuine HubPage method; it does not load, authenticate or signal readiness.
// The ordinary implicit constructor delegates to real MonoSingleton<HubPageManager>.
// Exact newly emitted metadata/generic behavior and Engine/assets remain separate.
using Unity.IL2CPP.CompilerServices;

namespace Hardlight
{
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.NullChecks, false)]
    public class HubPageManager : MonoSingleton<HubPageManager>
    {
        public SubPageIdentifier PageToLoadOnEnable
        {
            get { return m_pageToLoadOnEnable; }
            set { m_pageToLoadOnEnable = value; }
        }

        public HubPage ActiveHubPage { get; set; }
        private SubPageIdentifier m_pageToLoadOnEnable;

        public bool IsSubPageVisible(SubPageIdentifier subPageIdentifier)
        {
            return ActiveHubPage != null && ActiveHubPage.IsSubPageVisible(subPageIdentifier);
        }
    }
}
