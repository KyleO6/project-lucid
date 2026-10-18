using System;
using Unity.IL2CPP.CompilerServices;

namespace Hardlight
{
    // Original HLInput.Runtime 0x0200007b: complete constructor-only owner;
    // its generic base and callbacks are the maintained BaseInputVectorisedGameInput.
    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class InputVectorised : BaseInputVectorisedGameInput<GameInputBinding, BindingData>
    {
        public InputVectorised() { } // 0x06000255
    }
}
