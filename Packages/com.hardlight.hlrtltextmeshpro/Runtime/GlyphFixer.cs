// Original RTLTMPro.GlyphFixer, whole shipping owner; source spelling is inferred from both native slices.
// Public mutable dictionaries use ordered indexer assignments and retain BeforeFieldInit; no explicit static ctor.
// Lam mappings are shipping 0622->FEF3, 0623->FEF5, 0625->FEF7, 0627->FEF9; then writes FFFF.
// Fix mutates Yah in input before copying full FastStringBuilder capacity; number passes occur after shaping.
// Predicates eagerly acquire neighboring cells, retain signed <= FFFE and char truncation, and use literal shipping sets.
// Null/index faults, partial writes, mutable map failures, class-init timing and optimizer fault order remain research holds.
// GlyphTable/TextUtils/FastStringBuilder/BCL algorithms and future generated metadata require separate qualification.
using System.Collections.Generic;

namespace RTLTMPro
{
    public static class GlyphFixer
    {
        public static Dictionary<char, char> EnglishToFarsiNumberMap = new Dictionary<char, char>
        {
            ['0'] = '\u06f0',
            ['1'] = '\u06f1',
            ['2'] = '\u06f2',
            ['3'] = '\u06f3',
            ['4'] = '\u06f4',
            ['5'] = '\u06f5',
            ['6'] = '\u06f6',
            ['7'] = '\u06f7',
            ['8'] = '\u06f8',
            ['9'] = '\u06f9'
        };

        public static Dictionary<char, char> EnglishToHinduNumberMap = new Dictionary<char, char>
        {
            ['0'] = '\u0660',
            ['1'] = '\u0661',
            ['2'] = '\u0662',
            ['3'] = '\u0663',
            ['4'] = '\u0664',
            ['5'] = '\u0665',
            ['6'] = '\u0666',
            ['7'] = '\u0667',
            ['8'] = '\u0668',
            ['9'] = '\u0669'
        };

        public static void Fix(FastStringBuilder input, FastStringBuilder output, bool preserveNumbers, bool farsi, bool fixTextTags)
        {
            FixYah(input, farsi);
            output.SetValue(input);
            for (int i = 0; i < input.Length; i++)
            {
                int letter = input.Get(i);
                bool handledLam = false;
                if (letter == '\u0644' && i < input.Length - 1 && HandleSpecialLam(input, output, i))
                {
                    letter = output.Get(i);
                    handledLam = true;
                }
                if (letter == '\u0640' || letter == '\u200c')
                    continue;
                if (letter <= 0xfffe && TextUtils.IsGlyphFixedArabicCharacter((char)letter))
                {
                    letter = GlyphTable.Convert((char)letter);
                    if (IsMiddleLetter(input, i))
                        output.Set(i, (char)(letter + 3));
                    else if (IsFinishingLetter(input, i))
                        output.Set(i, (char)(letter + 1));
                    else if (IsLeadingLetter(input, i))
                        output.Set(i, (char)(letter + 2));
                    else
                        output.Set(i, (char)letter);
                }
                if (handledLam)
                    i++;
            }
            if (!preserveNumbers)
            {
                if (fixTextTags)
                    FixNumbersOutsideOfTags(output, farsi);
                else
                    FixNumbers(output, farsi);
            }
        }

        public static void FixYah(FastStringBuilder text, bool farsi)
        {
            if (farsi)
                text.Replace('\u064a', '\u06cc');
            else
                text.Replace('\u06cc', '\u064a');
        }

        private static bool HandleSpecialLam(FastStringBuilder input, FastStringBuilder output, int i)
        {
            char replacement;
            switch (input.Get(i + 1))
            {
                case '\u0622': replacement = '\ufef3'; break;
                case '\u0623': replacement = '\ufef5'; break;
                case '\u0625': replacement = '\ufef7'; break;
                case '\u0627': replacement = '\ufef9'; break;
                default: return false;
            }
            output.Set(i, replacement);
            output.Set(i + 1, '\uffff');
            return true;
        }

        public static void FixNumbers(FastStringBuilder text, bool farsi)
        {
            text.Replace('0', farsi ? '\u06f0' : '\u0660');
            text.Replace('1', farsi ? '\u06f1' : '\u0661');
            text.Replace('2', farsi ? '\u06f2' : '\u0662');
            text.Replace('3', farsi ? '\u06f3' : '\u0663');
            text.Replace('4', farsi ? '\u06f4' : '\u0664');
            text.Replace('5', farsi ? '\u06f5' : '\u0665');
            text.Replace('6', farsi ? '\u06f6' : '\u0666');
            text.Replace('7', farsi ? '\u06f7' : '\u0667');
            text.Replace('8', farsi ? '\u06f8' : '\u0668');
            text.Replace('9', farsi ? '\u06f9' : '\u0669');
        }

        public static void FixNumbersOutsideOfTags(FastStringBuilder text, bool farsi)
        {
            HashSet<char> numbers = new HashSet<char>(EnglishToFarsiNumberMap.Keys);
            for (int i = 0; i < text.Length; i++)
            {
                int letter = text.Get(i);
                if (letter == '<')
                {
                    bool foundTag = false;
                    for (int j = i + 1; j < text.Length; j++)
                    {
                        int next = text.Get(j);
                        if ((next == ' ' && j == i + 1) || next == '<')
                            break;
                        if (next == '>')
                        {
                            i = j;
                            foundTag = true;
                            break;
                        }
                    }
                    if (foundTag)
                        continue;
                }
                if (numbers.Contains((char)letter))
                    text.Set(i, farsi ? EnglishToFarsiNumberMap[(char)letter] : EnglishToHinduNumberMap[(char)letter]);
            }
        }

        private static bool IsLeadingLetter(FastStringBuilder letters, int index)
        {
            int previous = index == 0 ? 0 : letters.Get(index - 1);
            int current = letters.Get(index);
            int next = index < letters.Length - 1 ? letters.Get(index + 1) : 0;
            bool previousDisconnected = index == 0 ||
                (previous <= 0xfffe && !TextUtils.IsGlyphFixedArabicCharacter((char)previous)) ||
                previous == '\u0621' ||
                previous == '\u0622' ||
                previous == '\u0623' ||
                previous == '\u0624' ||
                previous == '\u0625' ||
                previous == '\u0627' ||
                previous == '\u062f' ||
                previous == '\u0630' ||
                previous == '\u0631' ||
                previous == '\u0632' ||
                previous == '\u0648' ||
                previous == '\u0698' ||
                previous == '\ufe80' ||
                previous == '\ufe81' ||
                previous == '\ufe83' ||
                previous == '\ufe85' ||
                previous == '\ufe87' ||
                previous == '\ufe8d' ||
                previous == '\ufea9' ||
                previous == '\ufeab' ||
                previous == '\ufead' ||
                previous == '\ufeaf' ||
                previous == '\ufeed' ||
                previous == '\ufb8a' ||
                previous == '\u200c';
            bool currentConnects = current != '\u0621' &&
                current != '\u0623' &&
                current != '\u0625' &&
                current != '\u0020' &&
                current != '\u0622' &&
                current != '\u0624' &&
                current != '\u0627' &&
                current != '\u062f' &&
                current != '\u0630' &&
                current != '\u0631' &&
                current != '\u0632' &&
                current != '\u0648' &&
                current != '\u0698' &&
                current != '\u200c';
            bool nextConnects = index < letters.Length - 1 && next <= 0xfffe &&
                TextUtils.IsGlyphFixedArabicCharacter((char)next) && next != '\u0621' && next != '\u200c';
            return previousDisconnected & currentConnects & nextConnects;
        }

        private static bool IsFinishingLetter(FastStringBuilder letters, int index)
        {
            int previous = index == 0 ? 0 : letters.Get(index - 1);
            int current = letters.Get(index);
            bool previousConnects = previous != '\u0621' &&
                previous != '\u0622' &&
                previous != '\u0623' &&
                previous != '\u0625' &&
                previous != '\u0624' &&
                previous != '\u0627' &&
                previous != '\u062f' &&
                previous != '\u0630' &&
                previous != '\u0631' &&
                previous != '\u0632' &&
                previous != '\u0698' &&
                previous != '\u0648' &&
                previous != '\ufe80' &&
                previous != '\ufe81' &&
                previous != '\ufe83' &&
                previous != '\ufe87' &&
                previous != '\ufead' &&
                previous != '\ufeaf' &&
                previous != '\ufb8a' &&
                previous != '\ufeed' &&
                previous != '\ufe85' &&
                previous != '\ufe8d' &&
                previous != '\ufea9' &&
                previous != '\ufeab' &&
                previous != '\u200c' &&
                previous != '\u0020' &&
                previous <= 0xfffe && index != 0 && TextUtils.IsGlyphFixedArabicCharacter((char)previous);
            bool currentConnects = current != ' ' && current != '\u200c' && current != '\u0621';
            return previousConnects & currentConnects;
        }

        private static bool IsMiddleLetter(FastStringBuilder letters, int index)
        {
            int previous = index == 0 ? 0 : letters.Get(index - 1);
            int current = letters.Get(index);
            int next = index < letters.Length - 1 ? letters.Get(index + 1) : 0;
            bool previousConnects = index != 0 &&
                previous != '\u0623' &&
                previous != '\u0625' &&
                previous != '\u0624' &&
                previous != '\u0621' &&
                previous != '\u0632' &&
                previous != '\u0698' &&
                previous != '\u0648' &&
                previous != '\u0622' &&
                previous != '\u0627' &&
                previous != '\u062f' &&
                previous != '\u0630' &&
                previous != '\u0631' &&
                previous != '\ufe81' &&
                previous != '\ufe83' &&
                previous != '\ufe87' &&
                previous != '\ufe85' &&
                previous != '\ufeaf' &&
                previous != '\ufb8a' &&
                previous != '\ufeed' &&
                previous != '\u200c' &&
                previous != '\ufe8d' &&
                previous != '\ufea9' &&
                previous != '\ufeab' &&
                previous != '\ufead' &&
                previous != '\ufe80' &&
                previous <= 0xfffe && TextUtils.IsGlyphFixedArabicCharacter((char)previous);
            bool nextConnects = index < letters.Length - 1 && next <= 0xfffe &&
                TextUtils.IsGlyphFixedArabicCharacter((char)next) && next != '\u0621' && next != '\u200c' && next != '\ufe80';
            bool currentConnects = current != '\u200c' &&
                current != '\u0621' &&
                current != '\u0622' &&
                current != '\u0623' &&
                current != '\u0625' &&
                current != '\u0624' &&
                current != '\u0627' &&
                current != '\u062f' &&
                current != '\u0630' &&
                current != '\u0631' &&
                current != '\u0632' &&
                current != '\u0698' &&
                current != '\u0648';
            return index != 0 & previousConnects & nextConnects & currentConnects;
        }
    }
}
