using System;
using System.Collections.Generic;
using Unity.IL2CPP.CompilerServices;

namespace Hardlight
{
    // HLInput.Runtime 0x02000016: complete original local API is three fields
    // and zero methods. No constructor exists in shipped native metadata.
    // StrippedBindingConstructionCompatibility.cs adds a private constructor
    // calling base(modifiers) so this source compiles; it has no original
    // credit, and asset/JSON construction routes still need engine tests.
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public partial class VectorisedBinding : BaseBinding
    {
        public List<GameInput> XComponents;
        public List<GameInput> YComponents;
        public List<GameInput> ZComponents;
    }
}
