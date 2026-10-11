// Original shipping preservation: HLBinders.Runtime.dll / Hardlight.UI.Binding.AddNodeMenu 0x02000004.
// Source-only reconstruction from original declarations and ARM64/x86_64 native evidence.
// No current compiler, binding, lifecycle, supplier or runtime equivalence is implied.
// 0x06000003 .ctor: arm64 [0x19f6f24,0x19f6f6c); x86_64 [0x19f2f40,0x19f2f80).

using System;

namespace Hardlight.UI.Binding
{
    public class AddNodeMenu : Attribute
    {
        public readonly string MenuPath;
        public readonly int Priority;

        public AddNodeMenu(string menuPath, int priority = int.MaxValue)
        {
            MenuPath = menuPath;
            Priority = priority;
        }
    }
}
