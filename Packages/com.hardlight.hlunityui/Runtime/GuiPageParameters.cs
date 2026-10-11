// Reconstructed whole original HLUnityUI.Runtime GuiPageParameters.
// Native/source evidence preserves original behavior for research; C# spelling,
// compiler-generated layout, native fault parity and Unity runtime remain held.
using System;
using Unity.IL2CPP.CompilerServices;

namespace Hardlight
{
    [Serializable]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.NullChecks, false)]
    public class GuiPageParameters : ICloneable
    {
        public readonly HLSelectable Selectable;

        public object Clone() { return MemberwiseClone(); }
        public virtual bool Restore() { return false; }
        public override bool Equals(object obj)
        {
            return obj is GuiPageParameters && obj.GetType() == GetType();
        }
        public override int GetHashCode() { return base.GetHashCode(); }
        public GuiPageParameters(HLSelectable selectable) { Selectable = selectable; }
        public GuiPageParameters() { }
    }
}
