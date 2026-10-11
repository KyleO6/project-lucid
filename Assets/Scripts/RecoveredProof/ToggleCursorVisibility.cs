// Reconstructed original Game.Runtime.dll ToggleCursorVisibility0200083f and natural owners02000840/841.
// Complete 17 MethodDefs; both static native architectures inform this source, without executing them.
// Constructor acquires App then UIRuntimeConfiguration references before the MonoBehaviour base constructor.
// Start waits only for configuration validity, then dereferences EventSystem.current while waiting for its input module.
// It subscribes static input changes, applies the current input type, then subscribes App gameplay and force-release callbacks.
// There is no App-readiness wait, rollback, cancellation, cursor restoration, or force-release guard in the gameplay callback.
// Missing EventSystem faults are retained. Only a non-null genuine HLInputModule changes hardware and Unity cursor visibility.
// Lambda/iterator syntax preserves natural roles; current compiler identities, native fault parity and Engine lifecycle remain separate gates.
// 0x06002fd9 HardlightProject.ToggleCursorVisibility+<>c..cctor; arm64 [0x5e3960,0x5e39cc); x86_64 [0x609f00,0x609f70).
// 0x06002fda HardlightProject.ToggleCursorVisibility+<>c..ctor; arm64 [0x5e39cc,0x5e39d4); x86_64 [0x609f70,0x609f80).
// 0x06002fdb HardlightProject.ToggleCursorVisibility+<>c.<Start>b__3_1; arm64 [0x5e39d4,0x5e3a98); x86_64 [0x609f80,0x60a030).
// 0x06002fdc HardlightProject.ToggleCursorVisibility+<Start>d__3..ctor; arm64 [0x5e3328,0x5e3354); x86_64 [0x609950,0x609970).
// 0x06002fdd HardlightProject.ToggleCursorVisibility+<Start>d__3.System.IDisposable.Dispose; arm64 [0x5e3a98,0x5e3a9c); x86_64 [0x60a030,0x60a040).
// 0x06002fde HardlightProject.ToggleCursorVisibility+<Start>d__3.MoveNext; arm64 [0x5e3a9c,0x5e3eb0); x86_64 [0x60a040,0x60a460).
// 0x06002fdf HardlightProject.ToggleCursorVisibility+<Start>d__3.System.Collections.Generic.IEnumerator<System.Object>.get_Current; arm64 [0x5e3eb0,0x5e3eb8); x86_64 [0x60a460,0x60a470).
// 0x06002fe0 HardlightProject.ToggleCursorVisibility+<Start>d__3.System.Collections.IEnumerator.Reset; arm64 [0x5e3eb8,0x5e3ef8); x86_64 [0x60a470,0x60a4b0).
// 0x06002fe1 HardlightProject.ToggleCursorVisibility+<Start>d__3.System.Collections.IEnumerator.get_Current; arm64 [0x5e3ef8,0x5e3f00); x86_64 [0x60a4b0,0x60a4c0).
// 0x06002fd1 HardlightProject.ToggleCursorVisibility.Start; arm64 [0x5e32b0,0x5e3328); x86_64 [0x6098e0,0x609950).
// 0x06002fd2 HardlightProject.ToggleCursorVisibility.OnDestroy; arm64 [0x5e3354,0x5e3564); x86_64 [0x609970,0x609b70).
// 0x06002fd3 HardlightProject.ToggleCursorVisibility.OnInputTypeChanged; arm64 [0x5e3564,0x5e36d8); x86_64 [0x609b70,0x609cd0).
// 0x06002fd4 HardlightProject.ToggleCursorVisibility.ForceReleaseCursor; arm64 [0x5e3830,0x5e3838); x86_64 [0x609dd0,0x609de0).
// 0x06002fd5 HardlightProject.ToggleCursorVisibility.OnGameplayControlActive; arm64 [0x5e3838,0x5e3840); x86_64 [0x609de0,0x609e00).
// 0x06002fd6 HardlightProject.ToggleCursorVisibility.SetCursorVisibility; arm64 [0x5e36d8,0x5e3830); x86_64 [0x609cd0,0x609dd0).
// 0x06002fd7 HardlightProject.ToggleCursorVisibility..ctor; arm64 [0x5e3840,0x5e390c); x86_64 [0x609e00,0x609ec0).
// 0x06002fd8 HardlightProject.ToggleCursorVisibility.<Start>b__3_0; arm64 [0x5e390c,0x5e3960); x86_64 [0x609ec0,0x609f00).

using System.Collections;
using Hardlight;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HardlightProject
{
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.NullChecks, false)]
    public class ToggleCursorVisibility : MonoBehaviour
    {
        private readonly SystemRef<App> m_appRef = ProcessManager.GetSystemRef<App>();
        private readonly SystemRef<UIRuntimeConfiguration> m_uiRuntimeConfigurationRef = ProcessManager.GetSystemRef<UIRuntimeConfiguration>();
        private bool m_forceReleaseCursor;

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => m_uiRuntimeConfigurationRef.IsValid());
            yield return new WaitUntil(() => EventSystem.current.currentInputModule != null);
            InputBridge.OnUpdateLastInputType += OnInputTypeChanged;
            OnInputTypeChanged(m_uiRuntimeConfigurationRef.Get().InputBridge.GetLastInputType());
            App app = m_appRef.Get();
            app.OnGameplayControlActive += OnGameplayControlActive;
            app.OnForceReleaseCursor += ForceReleaseCursor;
        }

        private void OnDestroy()
        {
            InputBridge.OnUpdateLastInputType -= OnInputTypeChanged;
            if (m_appRef.TryGet(out App app))
            {
                app.OnGameplayControlActive -= OnGameplayControlActive;
                app.OnForceReleaseCursor -= ForceReleaseCursor;
            }
        }

        private void OnInputTypeChanged(UIInputType inputType)
        {
            if (m_forceReleaseCursor) return;
            if (inputType == UIInputType.Mouse || inputType == UIInputType.Keyboard)
                SetCursorVisibility(!m_appRef.Get().Storage.GetValue(AppFSMKeys.GameplayControlActive, false, true));
            else
                SetCursorVisibility(false);
        }

        private void ForceReleaseCursor(bool release)
        {
            m_forceReleaseCursor = release;
            SetCursorVisibility(release);
        }

        private void OnGameplayControlActive(bool on)
        {
            SetCursorVisibility(!on);
        }

        private void SetCursorVisibility(bool on)
        {
            HLInputModule inputModule = EventSystem.current.currentInputModule as HLInputModule;
            if (inputModule == null) return;
            inputModule.SetHardwareCursorVisibility(on);
            Cursor.visible = on;
        }

        public ToggleCursorVisibility() { }
    }
}
