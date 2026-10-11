// Reconstructed original Game.Runtime.dll UIPlatformVisibilityController; static source/native evidence only.
// Original C# spelling, compiler-generated identities, null/fault timing and other-platform source branches remain underdetermined.
// 0x06002ffe HardlightProject.UIPlatformVisibilityController.Start: arm64 [0x5e5184,0x5e5190), x86_64 [0x60b650,0x60b660).
// 0x06002fff HardlightProject.UIPlatformVisibilityController..ctor: arm64 [0x5e5190,0x5e5198), x86_64 [0x60b660,0x60b670).
// The supplied shipping macOS bodies activate m_macOsUiOverrides unconditionally. No runtime platform predicate is present.
// Any original preprocessor selection for unobserved targets is a source-form hold; this preserves the observed original method.
using Hardlight;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace HardlightProject
{
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.NullChecks, false)]
    public class UIPlatformVisibilityController : MonoBehaviour
    {
        [SerializeField] private UIVisibilityGroupOverrider m_macOsUiOverrides;

        private void Start()
        {
            m_macOsUiOverrides.ActivateOverrides();
        }

        public UIPlatformVisibilityController() { }
    }
}
