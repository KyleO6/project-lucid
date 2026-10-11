// Shipping HLRTLTextMeshPro.Runtime TashkeelFixer: complete metadata and dual native reconstruction.
// Inferred local names/C# spelling, optimizer faults, providers, current emission and runtime equivalence remain held.
using System;
using System.Collections.Generic;

namespace RTLTMPro
{
    public static class TashkeelFixer
    {
        private static readonly List<TashkeelLocation> TashkeelLocations = new List<TashkeelLocation>(100);
        private static readonly string ShaddaDammatan = new string(new[] { '\u0651', '\u064c' });
        private static readonly string ShaddaKasratan = new string(new[] { '\u0651', '\u064d' });
        private static readonly string ShaddaSuperscriptAlef = new string(new[] { '\u0651', '\u0670' });
        private static readonly string ShaddaFatha = new string(new[] { '\u0651', '\u064e' });
        private static readonly string ShaddaDamma = new string(new[] { '\u0651', '\u064f' });
        private static readonly string ShaddaKasra = new string(new[] { '\u0651', '\u0650' });
        private static readonly string ShaddaWithFathaIsolatedForm = '\ufc60'.ToString();
        private static readonly string ShaddaWithDammaIsolatedForm = '\ufc61'.ToString();
        private static readonly string ShaddaWithKasraIsolatedForm = '\ufc62'.ToString();
        private static readonly string ShaddaWithDammatanIsolatedForm = '\ufc5e'.ToString();
        private static readonly string ShaddaWithKasratanIsolatedForm = '\ufc5f'.ToString();
        private static readonly string ShaddaWithSuperscriptAlefIsolatedForm = '\ufc63'.ToString();
        private static readonly HashSet<char> TashkeelCharactersSet = new HashSet<char>
        {
            '\u064b', '\u064c', '\u064d', '\u064e', '\u064f', '\u0650', '\u0651', '\u0652',
            '\u0653', '\u0670', '\ufc5e', '\ufc5f', '\ufc60', '\ufc61', '\ufc62', '\ufc63'
        };
        private static readonly Dictionary<char, char> ShaddaCombinationMap = new Dictionary<char, char>
        {
            ['\u064c'] = '\ufc5e', ['\u064d'] = '\ufc5f', ['\u064e'] = '\ufc60',
            ['\u064f'] = '\ufc61', ['\u0650'] = '\ufc62', ['\u0670'] = '\ufc63'
        };

        public static void RemoveTashkeel(FastStringBuilder input)
        {
            TashkeelLocations.Clear();
            int written = 0;
            for (int i = 0; i < input.Length; i++)
            {
                int current = input.Get(i);
                if (current <= 0xfffe && TashkeelCharactersSet.Contains((char)current))
                    TashkeelLocations.Add(new TashkeelLocation((TashkeelCharacters)(char)current, i));
                else
                    input.Set(written++, current);
            }
            input.Length = written;
        }

        public static void RestoreTashkeel(FastStringBuilder letters)
        {
            foreach (TashkeelLocation location in TashkeelLocations)
                letters.Insert(location.Position, location.Tashkeel);
        }

        public static void FixShaddaCombinations(FastStringBuilder input)
        {
            int written = 0;
            for (int i = 0; i < input.Length; i++)
            {
                int current = input.Get(i);
                int next = i < input.Length - 1 ? input.Get(i + 1) : 0;
                if (current == '\u0651' && ShaddaCombinationMap.ContainsKey((char)next))
                {
                    input.Set(written++, ShaddaCombinationMap[(char)next]);
                    i++;
                }
                else
                    input.Set(written++, current);
            }
            input.Length = written;
        }
    }
}
