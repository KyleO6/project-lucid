// Original Char32Utils8; signed ch < 65535 deliberately permits negative aliases.
// Subsequent UTF16 truncation/modular ranges and genuine System.Char calls are retained.
// Native ARM64/x86_64 source spelling and BCL Unicode table/runtime parity remain held.
using System;

namespace RTLTMPro
{
    public static class Char32Utils
    {
        public static bool IsUnicode16Char(int ch)
        {
            return ch < 65535;
        }

        public static bool IsRTLCharacter(int ch)
        {
            if (!IsUnicode16Char(ch)) return false;
            char value = unchecked((char)ch);
            return (value >= 0x0591 && value <= 0x05f4)
                || (value >= 0xfb50 && value <= 0xfdff)
                || (value >= 0x0600 && value <= 0x06ff)
                || (value >= 0x0870 && value <= 0x08ff)
                || (value >= 0xfe70 && value <= 0xfeff);
        }

        public static bool IsEnglishLetter(int ch)
        {
            return IsUnicode16Char(ch)
                && unchecked((ushort)((ch & 0xffdf) - 0x41)) < 26;
        }

        public static bool IsNumber(int ch, bool preserveNumbers, bool farsi)
        {
            int first = preserveNumbers ? (int)EnglishNumbers.Zero
                : farsi ? (int)FarsiNumbers.Zero : (int)HinduNumbers.Zero;
            return IsUnicode16Char(ch) && unchecked((ushort)(ch - first)) < 10;
        }

        public static bool IsSymbol(int ch)
        {
            return IsUnicode16Char(ch) && Char.IsSymbol(unchecked((char)ch));
        }

        public static bool IsLetter(int ch)
        {
            return IsUnicode16Char(ch) && Char.IsLetter(unchecked((char)ch));
        }

        public static bool IsPunctuation(int ch)
        {
            return IsUnicode16Char(ch) && Char.IsPunctuation(unchecked((char)ch));
        }

        public static bool IsWhiteSpace(int ch)
        {
            return IsUnicode16Char(ch) && Char.IsWhiteSpace(unchecked((char)ch));
        }
    }
}
