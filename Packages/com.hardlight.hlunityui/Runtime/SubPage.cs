// Original HLUnityUI.Runtime Hardlight.SubPage, 0x0200003e, six methods
// 0x060001cd..0x060001d2. Serialized names, former m_content name and ordered
// component requirements are preserved. Start/OnDestroy call the real registry.
// Relocation tests Unity content null semantics before the content flag. When
// requested, it resets the content RectTransform, then reads the genuine screen
// singleton: Landscape selects 25f bleed; every other value selects 300f.
// Root offsets are changed before SetParent(newParent, false). Missing roots or
// singleton providers are not replaced or given new null guards. Content and
// its transform are reread; return remains the current content reference.
// Editor load/save UnityEvents are initialized in field order before base ctor.
// Genuine SubPageContent/ScreenManager/UIOrientation supplier admission and
// authored Unity bindings/page startup remain held separately from this source.
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Hardlight
{
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasScaler))]
    public class SubPage : MonoBehaviour
    {
        [SerializeField] private string m_identifier;
        [FormerlySerializedAs("m_content")]
        [SerializeField] private SubPageContent m_contentScript;
        [SerializeField] private RectTransform m_subPageContentRoot;
        [SerializeField]
        [Header("Editor Only")]
        protected UnityEvent m_onEditorLoad = new UnityEvent();
        [SerializeField] protected UnityEvent m_onEditorSave = new UnityEvent();

        public string Identifier
        {
            get { return m_identifier; }
            set { m_identifier = value; }
        }

        public void Start()
        {
            GuiPageRegistry.Instance.RegisterSubPage(this);
        }

        public void OnDestroy()
        {
            GuiPageRegistry.Instance.UnregisterSubPage(this);
        }

        public SubPageContent RelocateToNewParent(Transform newParent)
        {
            if (m_contentScript != null)
            {
                if (m_contentScript.ForceAdjustBackgroundBleedSizeOnRelocate)
                {
                    RectTransform contentRect = m_contentScript.transform as RectTransform;
                    if (contentRect != null)
                    {
                        contentRect.anchorMin = Vector2.zero;
                        contentRect.anchorMax = Vector2.one;
                        contentRect.offsetMin = Vector2.zero;
                        contentRect.offsetMax = Vector2.zero;
                    }

                    float bleed = ScreenManager.Instance.Orientation == UIOrientation.Landscape ? 25f : 300f;
                    m_subPageContentRoot.anchorMin = Vector2.zero;
                    m_subPageContentRoot.anchorMax = Vector2.one;
                    m_subPageContentRoot.offsetMin = new Vector2(bleed, 0f);
                    m_subPageContentRoot.offsetMax = new Vector2(-bleed, 0f);
                }

                m_contentScript.transform.SetParent(newParent, false);
            }

            return m_contentScript;
        }

        public SubPage()
        {
        }
    }
}
