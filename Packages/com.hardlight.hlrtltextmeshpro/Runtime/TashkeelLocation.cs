// Shipping HLRTLTextMeshPro.Runtime TashkeelLocation: complete metadata and dual native reconstruction.
// Inferred local names/C# spelling, optimizer faults, providers, current emission and runtime equivalence remain held.
namespace RTLTMPro
{
    public struct TashkeelLocation
    {
        public char Tashkeel { get; set; }
        public int Position { get; set; }

        public TashkeelLocation(TashkeelCharacters tashkeel, int position) : this()
        {
            Tashkeel = (char)tashkeel;
            Position = position;
        }
    }
}
