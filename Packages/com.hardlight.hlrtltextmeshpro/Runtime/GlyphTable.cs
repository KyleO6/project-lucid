// Original HLRTLTextMeshPro.Runtime.dll RTLTMPro.GlyphTable, token 02000007.
// .cctor 06000030: ARM64 0x1a576fc..0x1a579a0; x86_64 0x1a50230..0x1a504c0.
// Convert 06000031: ARM64 0x1a56760..0x1a5682c; x86_64 0x1a4f000..0x1a4f0a0.
// Initialisation obtains isolated enum names, publishes the capacity-sized map, then
// parses general and isolated values by each name. Boxed Int32 values truncate to
// UTF-16 chars. Convert uses TryGetValue and returns the original char on a miss.
// Names, local spellings and foreach syntax are reconstructed; native dictionary
// receiver checks and unboxing fault order are recorded separately, not asserted
// equivalent to managed evaluation. BCL enum order and algorithms remain held.
using System;
using System.Collections.Generic;

namespace RTLTMPro
{
    public static class GlyphTable
    {
        private static readonly Dictionary<char, char> MapList;

        static GlyphTable()
        {
            string[] names = Enum.GetNames(typeof(ArabicIsolatedLetters));
            MapList = new Dictionary<char, char>(names.Length);
            foreach (string name in names)
            {
                Dictionary<char, char> map = MapList;
                object general = Enum.Parse(typeof(ArabicGeneralLetters), name);
                object isolated = Enum.Parse(typeof(ArabicIsolatedLetters), name);
                map.Add(unchecked((char)(int)general), unchecked((char)(int)isolated));
            }
        }

        public static char Convert(char toBeConverted)
        {
            char result = '\0';
            return MapList.TryGetValue(toBeConverted, out result) ? result : toBeConverted;
        }
    }
}
