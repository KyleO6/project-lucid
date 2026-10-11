// Reconstructed whole original HLUnityUI.Runtime GuiPage.
// Native/source evidence preserves original behavior for research; C# spelling,
// compiler-generated layout, native fault parity and Unity runtime remain held.
using System.Collections;
using System.Collections.Generic;
using Hardlight.Utils;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Hardlight
{
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(CanvasScaler))]
    [Il2CppSetOption(Option.NullChecks, false)]
    public abstract class GuiPage : MonoBehaviour
    {
        [SerializeField] public UnityEvent m_onShow = new UnityEvent();
        [SerializeField] public UnityEvent m_onShowComplete = new UnityEvent();
        [SerializeField] public UnityEvent m_onHide = new UnityEvent();
        [SerializeField] public UnityEvent m_onHideComplete = new UnityEvent();
        [SerializeField] public List<GameObject> m_objectsToActivate = new List<GameObject>();
        [SerializeField] protected float m_showDuration;
        [Tooltip("The time after which it is safe to hide all group objects")]
        [SerializeField] protected float m_hideDuration;
        [Tooltip("Whether group elements should be interactable as soon as the show animation starts playing")]
        [SerializeField] protected bool m_immediatelyInteractable;
        [SerializeField] private CanvasGroup[] m_rootCanvasGroups;
        [SerializeField] private GuiPageParameters pageParameters;
        [SerializeField] protected bool m_enableDisableBaseCanvas;
        [SerializeField] protected bool m_shouldInterruptFTUE;
        [SerializeField] protected string m_identifier;
        [Tooltip("How long subsequent pages should wait before starting their show animations")]
        [SerializeField] protected float m_pageTransitionDelay;
        [SerializeField] protected UnityEvent m_onEditorLoad = new UnityEvent();
        [SerializeField] protected UnityEvent m_onEditorSave = new UnityEvent();
        [SerializeField] private Canvas[] m_rootCanvases;
        [SerializeField] private GuiPageContent[] m_content;
        [Tooltip("Restrict the selectable UI highlighter to only elements in this scene when this scene is open")]
        [SerializeField] private bool m_selectorIsExclusiveToScene;
        [SerializeField]
        [Tooltip("For pages which are in the background like 's_overlay_dialog_background'")]
        private bool m_doNotClearSelectableOnHide;
        private State m_state;
        private Coroutine m_findCameraCoroutine;
        private Coroutine m_hideCompleteCoroutine;
        private Coroutine m_showCompleteCoroutine;
        private const int EditorFindCameraTimeoutSeconds = 5;
        private GraphicRaycaster[] m_raycasters;

        public string Identifier { get => m_identifier; set => m_identifier = value; }
        public GuiPageParameters PageParameters => pageParameters;
        public CanvasGroup[] PageCanvasGroups => m_rootCanvasGroups;
        public GuiPageContent[] Content => m_content;
        public float PageTransitionDelay => m_pageTransitionDelay;
        public float ShowDuration => m_showDuration;
        public float HideDuration => m_hideDuration;
        public bool IsTranstioning => m_state == State.Opening || m_state == State.Closing;
        public bool IsShowing => m_state == State.Showing;
        public bool IsHidden => m_state == State.Hidden;
        public bool IsClosing => m_state == State.Closing;
        public bool CanLeavePage { get; set; }
        public bool ShouldInterruptFtue => m_shouldInterruptFTUE;
        public event OnPageTransitionDelayElapsedDelegate OnPageTransitionDelayElapsed = p => { };
        private SelectableControllerBase m_selectableController;
        private UIRuntimeConfiguration m_uiRuntimeConfiguration;

        private enum State { Hidden = 0, Opening = 1, Showing = 2, Closing = 3 }
        public delegate void OnPageTransitionDelayElapsedDelegate(GuiPage page);

        protected void Awake()
        {
            CanLeavePage = true;
            m_raycasters = GetComponentsInChildren<GraphicRaycaster>(true);
            SetRaycastersEnabled(false);
            if (m_rootCanvases != null && m_rootCanvases.Length > 0)
            {
                this.SafeStopCoroutine(ref m_findCameraCoroutine);
                m_findCameraCoroutine = StartCoroutine(FindUICameraForCanvas());
            }
        }

        private void SetRaycastersEnabled(bool value)
        {
            if (m_raycasters == null) return;
            for (int i = 0; i < m_raycasters.Length; ++i)
            {
                GraphicRaycaster raycaster = m_raycasters[i];
                if (raycaster != null) raycaster.enabled = value;
            }
        }

        protected void Start()
        {
            m_uiRuntimeConfiguration = ProcessManager.GetSystem<UIRuntimeConfiguration>(null, true);
            if (m_uiRuntimeConfiguration.ShouldHighlightSelection)
                m_selectableController = ProcessManager.GetSystemOfType<SelectableControllerBase>();
            GuiPageRegistry.Instance.RegisterGuiPage(this);
        }

        protected void OnDestroy()
        {
            pageParameters = null;
            GuiPageRegistry.Instance.UnregisterGuiPage(this);
        }

        public virtual void Show(GuiPageParameters parameters)
        {
            m_state = State.Opening;
            pageParameters = parameters;
            this.SafeStopCoroutine(ref m_hideCompleteCoroutine);
            if (m_immediatelyInteractable) SetInteractable(true);
            SetObjectsActive(true);
            if (m_uiRuntimeConfiguration.ShouldHighlightSelection && m_selectorIsExclusiveToScene && m_selectableController != null)
                m_selectableController.RegisterExclusiveScene(m_identifier);
            if (m_content != null)
                for (int i = 0; i < m_content.Length; ++i) m_content[i].OnShow(pageParameters);
            if (MonoSingleton<MenuBloomTransition>.NotNull())
                MonoSingleton<MenuBloomTransition>.Instance.FinishTransition();
            GuiPageTransition transition = GetTransition();
            if (transition != null)
                transition.StartTransitionIn(this, () => ShowComplete());
            else
            {
                this.SafeStopCoroutine(ref m_showCompleteCoroutine);
                m_showCompleteCoroutine = StartCoroutine(DoShowComplete());
            }
            SetRootCanvasEnabled(true);
            m_onShow.Invoke();
        }

        public virtual void Hide(UISystemClosureReason reasonForClosure)
        {
            m_state = State.Closing;
            if (reasonForClosure != UISystemClosureReason.AppRestartClose)
            {
                SetInteractable(false);
                SetRootCanvasEnabled(false);
            }
            SetRaycastersEnabled(false);
            this.SafeStopCoroutine(ref m_showCompleteCoroutine);
            if (GetTransition() != null && reasonForClosure != UISystemClosureReason.AppRestartClose)
                GetTransition().StartTransitionOut(this, () => HideComplete(reasonForClosure));
            else
            {
                this.SafeStopCoroutine(ref m_hideCompleteCoroutine);
                if (gameObject.activeInHierarchy)
                    m_hideCompleteCoroutine = StartCoroutine(DoHideComplete(reasonForClosure));
                else
                    HideComplete(reasonForClosure);
            }
            m_onHide.Invoke();
        }

        public void SetInteractable(bool interactable)
        {
            if (m_rootCanvasGroups == null) return;
            for (int i = 0; i < m_rootCanvasGroups.Length; ++i)
                m_rootCanvasGroups[i].interactable = interactable;
        }

        public virtual void Close() { Hide(UISystemClosureReason.StandardClose); }
        protected virtual GuiPageTransition GetTransition() { return null; }

        protected IEnumerator DoShowComplete()
        {
            if (m_showDuration > 0f)
                yield return StartCoroutine(CoroutineUtils.WaitForRealSeconds(m_showDuration));
            ShowComplete();
        }

        protected virtual void ShowComplete()
        {
            m_state = State.Showing;
            if (!m_immediatelyInteractable) SetInteractable(true);
            if (m_content != null)
                for (int i = 0; i < m_content.Length; ++i) m_content[i].ShowComplete(pageParameters);
            SetRaycastersEnabled(true);
            m_onShowComplete.Invoke();
        }

        protected IEnumerator DoHideComplete(UISystemClosureReason reasonForClosure)
        {
            if (m_hideDuration > 0f)
                yield return StartCoroutine(CoroutineUtils.WaitForRealSeconds(m_hideDuration));
            HideComplete(reasonForClosure);
        }

        protected virtual void HideComplete(UISystemClosureReason reasonForClosure)
        {
            m_state = State.Hidden;
            SetObjectsActive(false);
            HideContent(reasonForClosure);
            if (gameObject.activeInHierarchy) StartCoroutine(DoPageTransition());
            else OnPageTransitionDelayElapsed(this);
            m_onHideComplete.Invoke();
            if (m_uiRuntimeConfiguration.ShouldHighlightSelection && m_selectableController != null)
            {
                if (m_selectorIsExclusiveToScene) m_selectableController.DeregisterExclusiveScene(m_identifier);
                if (!m_doNotClearSelectableOnHide) m_selectableController.RemoveHighlighterFromScene();
            }
        }

        protected void HideContent(UISystemClosureReason reasonForClosure)
        {
            if (m_content != null)
                for (int i = 0; i < m_content.Length; ++i) m_content[i].OnHide(pageParameters, reasonForClosure);
        }

        protected void SetRootCanvasEnabled(bool enabled)
        {
            if (!m_enableDisableBaseCanvas || m_rootCanvases == null) return;
            for (int i = 0; i < m_rootCanvases.Length; ++i) m_rootCanvases[i].enabled = enabled;
        }

        protected void SetObjectsActive(bool active)
        {
            for (int i = 0; i < m_objectsToActivate.Count; ++i)
                if (m_objectsToActivate[i] != null) m_objectsToActivate[i].SetActive(active);
        }

        protected IEnumerator DoPageTransition()
        {
            if (m_pageTransitionDelay != 0f)
                yield return StartCoroutine(CoroutineUtils.WaitForRealSeconds(m_pageTransitionDelay));
            OnPageTransitionDelayElapsed(this);
        }

        protected void SetRootCanvasesCamera(Camera uiCamera)
        {
            if (m_rootCanvases == null) return;
            for (int i = 0; i < m_rootCanvases.Length; ++i)
            {
                m_rootCanvases[i].worldCamera = uiCamera;
                m_rootCanvases[i].enabled = true;
            }
        }

        protected IEnumerator FindUICameraForCanvas()
        {
            Camera camera = null;
            while (camera == null)
            {
                camera = MonoSingleton<GUICameraManager>.NotNull() ? MonoSingleton<GUICameraManager>.Instance.MenuCamera : null;
                if (camera != null) SetRootCanvasesCamera(camera);
                else yield return null;
            }
        }

        protected GuiPage() { }
    }
}
