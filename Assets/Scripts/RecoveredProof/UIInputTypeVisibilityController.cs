// Reconstructed original Game.Runtime.dll UIInputTypeVisibilityController; static source/native evidence only.
// Original C# spelling, compiler-generated identities, null/fault timing and other-platform source branches remain underdetermined.
// 0x06002ff3 HardlightProject.UIInputTypeVisibilityController.OnEnable: arm64 [0x5e4aec,0x5e4bdc), x86_64 [0x60b000,0x60b0e0).
// 0x06002ff4 HardlightProject.UIInputTypeVisibilityController.OnUIRuntimeConfigurationValid: arm64 [0x5e4bdc,0x5e4d20), x86_64 [0x60b0e0,0x60b200).
// 0x06002ff5 HardlightProject.UIInputTypeVisibilityController.OnDisable: arm64 [0x5e4d28,0x5e4e08), x86_64 [0x60b210,0x60b2e0).
// 0x06002ff6 HardlightProject.UIInputTypeVisibilityController.OnUpdateLastInputType: arm64 [0x5e4e08,0x5e4e74), x86_64 [0x60b2e0,0x60b340).
// 0x06002ff7 HardlightProject.UIInputTypeVisibilityController..ctor: arm64 [0x5e4e74,0x5e4e7c), x86_64 [0x60b340,0x60b350).
// 0x06002ff8 HardlightProject.UIInputTypeVisibilityController+<>c__DisplayClass4_0..ctor: arm64 [0x5e4d20,0x5e4d28), x86_64 [0x60b200,0x60b210).
// 0x06002ff9 HardlightProject.UIInputTypeVisibilityController+<>c__DisplayClass4_0.<OnUIRuntimeConfigurationValid>b__0: arm64 [0x5e4e7c,0x5e4fe4), x86_64 [0x60b350,0x60b4c0).
// Configuration validity precedes UIManager validity. The closure assigns the bridge, updates overrides, then subscribes.
// Original OnDisable only subtracts the static input callback; pending validity callbacks can subscribe later.
// No override reset, bridge clear, disabled-object guard, exception cleanup or fake readiness is added.
// The lambda preserves the two captured fields and original callback role; emitted closure ordinal/order is a separate current gate.
using Hardlight;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace HardlightProject
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class UIInputTypeVisibilityController : MonoBehaviour
    {
        [SerializeField] private UIVisibilityGroupOverrider m_pointerUIOverrides;
        [SerializeField] private UIVisibilityGroupOverrider m_controllerUIOverrides;
        private InputBridge m_inputBridge;

        private void OnEnable()
        {
            ProcessManager.GetSystemRef<UIRuntimeConfiguration>().InvokeOnValid(OnUIRuntimeConfigurationValid);
        }

        private void OnUIRuntimeConfigurationValid(UIRuntimeConfiguration uiRuntimeConfiguration)
        {
            ProcessManager.GetSystemRef<UIManager>().InvokeOnValid(_ =>
            {
                m_inputBridge = uiRuntimeConfiguration.InputBridge;
                OnUpdateLastInputType(m_inputBridge.GetLastInputType());
                InputBridge.OnUpdateLastInputType += OnUpdateLastInputType;
            });
        }

        private void OnDisable()
        {
            InputBridge.OnUpdateLastInputType -= OnUpdateLastInputType;
        }

        private void OnUpdateLastInputType(UIInputType lastInputType)
        {
            if (m_inputBridge.DoesInputTypeRequireSelection(lastInputType))
            {
                m_pointerUIOverrides.DeactivateOverrides();
                m_controllerUIOverrides.ActivateOverrides();
            }
            else
            {
                m_controllerUIOverrides.DeactivateOverrides();
                m_pointerUIOverrides.ActivateOverrides();
            }
        }

        public UIInputTypeVisibilityController() { }
    }
}
