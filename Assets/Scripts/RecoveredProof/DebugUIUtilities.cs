// Original Game.Runtime HardlightProject.DebugUIUtilities 02000954: complete one-method owner.
// 060035a1 .ctor: ARM64 [0x65f89c,0x65f8a4), x86_64 [0x684bb0,0x684bc0).
// Both CPUs clear the hidden method argument and tail-call MonoBehaviour's constructor.
// No original fields, nested owners, debugging callbacks or other methods are declared.
// Preserve ordered NullChecks=false then ArrayBoundsChecks=false options. This source
// establishes no scene binding, constructor execution, debug service or startup readiness.
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace HardlightProject
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class DebugUIUtilities : MonoBehaviour
    {
        public DebugUIUtilities() { }
    }
}
