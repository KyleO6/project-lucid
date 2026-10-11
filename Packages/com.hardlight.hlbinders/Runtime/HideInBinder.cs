// Original shipping preservation: HLBinders.Runtime.dll / Hardlight.UI.Binding.HideInBinder 0x02000006.
// 0x06000005 .ctor: arm64 [0x19f6fa0,0x19f6fa8); x86_64 [0x19f2fb0,0x19f2fc0).
// Marker attribute used by the original binder member filter; no fields or additional constructor work.

using System;

namespace Hardlight.UI.Binding
{
    public class HideInBinder : Attribute
    {
        public HideInBinder() { }
    }
}
