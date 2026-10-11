// Original Game.Runtime.dll HardlightProject.StackedCamera, TypeDef 0x0200029b.
// Reset: 0x06000e68, arm64 0x702408, x86_64 0x726450.
// OnEnable: 0x06000e69, arm64 0x70249c, x86_64 0x7264d0.
// OnValidate: 0x06000e6a, arm64 0x7025b8, x86_64 0x7265d0.
// OnCameraStackManagerSystemStartup: 0x06000e6b, arm64 0x7025d8, x86_64 0x7265f0.
// OnDisable: 0x06000e6c, arm64 0x7025e8, x86_64 0x726610.
// .ctor: 0x06000e6d, arm64 0x702640, x86_64 0x726660.
// Reset publishes Camera before the AudioListener lookup. Validation results are ignored.
// Enable stores the real auto-registering system reference before subscribing the callback.
// Disable obtains its current safe system and unloads only when that CLR reference is nonnull.
// Original reference-null faults, deferred callbacks, ordering and lack of cancellation remain.
using Hardlight;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace HardlightProject
{
    [RequireComponent(typeof(Camera))]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.NullChecks, false)]
    public class StackedCamera : MonoBehaviour
    {
        [SerializeField] private Camera m_camera;
        [SerializeField] private AudioListener m_audioListener;
        private SystemRef<CameraStackManager> m_cameraStackManagerRef;

        private void Reset()
        {
            m_camera = GetComponent<Camera>();
            m_audioListener = GetComponent<AudioListener>();
        }

        private void OnEnable()
        {
            gameObject.CanValidate(false);
            m_cameraStackManagerRef = ProcessManager.GetSystemRef<CameraStackManager>(null, true);
            m_cameraStackManagerRef.InvokeOnValid(OnCameraStackManagerSystemStartup);
        }

        private void OnValidate()
        {
            gameObject.CanValidate(false);
        }

        private void OnCameraStackManagerSystemStartup(CameraStackManager cameraStackManager)
        {
            cameraStackManager.OnCameraLoaded(m_camera, m_audioListener);
        }

        private void OnDisable()
        {
            CameraStackManager cameraStackManager = m_cameraStackManagerRef.GetSafe();
            if (cameraStackManager != null)
                cameraStackManager.OnCameraUnloaded(m_camera);
        }

        public StackedCamera() { }
    }
}
