// Original HLRTLTextMeshPro.Runtime RTLTMPro.TextUtils 02000011: eleven complete methods.
// IsPunctuation genuinely throws; IsRTLInput returns classification of the first letter outside markup.
// Glyph cases derive from both complete native tables; C# switch/range/local spelling is inferred.
// Unicode BCL semantics, optimized source form, native faults and current emission/runtime remain held.
using System;

namespace RTLTMPro
{
    public static class TextUtils
    {
        private const char LowerCaseA = 'a';
        private const char UpperCaseA = 'A';
        private const char LowerCaseZ = 'z';
        private const char UpperCaseZ = 'Z';
        private const char HebrewLow = '\u0591';
        private const char HebrewHigh = '\u05f4';
        private const char ArabicBaseBlockLow = '\u0600';
        private const char ArabicBaseBlockHigh = '\u06ff';
        private const char ArabicExtendedABlockLow = '\u08a0';
        private const char ArabicExtendedABlockHigh = '\u08ff';
        private const char ArabicExtendedBBlockLow = '\u0870';
        private const char ArabicExtendedBBlockHigh = '\u089f';
        private const char ArabicPresentationFormsABlockLow = '\ufb50';
        private const char ArabicPresentationFormsABlockHigh = '\ufdff';
        private const char ArabicPresentationFormsBBlockLow = '\ufe70';
        private const char ArabicPresentationFormsBBlockHigh = '\ufeff';

        public static bool IsPunctuation(char ch)
        {
            throw new NotImplementedException();
        }

        public static bool IsNumber(char ch, bool preserveNumbers, bool farsi)
        {
            if (preserveNumbers)
                return IsEnglishNumber(ch);
            return farsi ? IsFarsiNumber(ch) : IsHinduNumber(ch);
        }

        public static bool IsEnglishNumber(char ch)
        {
            return ch >= '0' && ch <= '9';
        }

        public static bool IsFarsiNumber(char ch)
        {
            return ch >= '\u06f0' && ch <= '\u06f9';
        }

        public static bool IsHinduNumber(char ch)
        {
            return ch >= '\u0660' && ch <= '\u0669';
        }

        public static bool IsEnglishLetter(char ch)
        {
            return (ch >= LowerCaseA && ch <= LowerCaseZ) || (ch >= UpperCaseA && ch <= UpperCaseZ);
        }

        public static bool IsHebrewCharacter(char ch)
        {
            return ch >= HebrewLow && ch <= HebrewHigh;
        }

        public static bool IsArabicCharacter(char ch)
        {
            return (ch >= ArabicBaseBlockLow && ch <= ArabicBaseBlockHigh)
                || (ch >= ArabicExtendedABlockLow && ch <= ArabicExtendedABlockHigh)
                || (ch >= ArabicExtendedBBlockLow && ch <= ArabicExtendedBBlockHigh)
                || (ch >= ArabicPresentationFormsABlockLow && ch <= ArabicPresentationFormsABlockHigh)
                || (ch >= ArabicPresentationFormsBBlockLow && ch <= ArabicPresentationFormsBBlockHigh);
        }

        public static bool IsRTLCharacter(char ch)
        {
            return IsHebrewCharacter(ch) || IsArabicCharacter(ch);
        }

        public static bool IsGlyphFixedArabicCharacter(char ch)
        {
            if (ch >= '\ufe80' && ch <= '\ufef4')
                return true;
            switch (ch)
            {
                case '\ufb56':
                case '\ufb57':
                case '\ufb58':
                case '\ufb59':
                case '\ufb7a':
                case '\ufb7b':
                case '\ufb7c':
                case '\ufb7d':
                case '\ufb8a':
                case '\ufb8b':
                case '\ufb8e':
                case '\ufb8f':
                case '\ufb90':
                case '\ufb91':
                case '\ufb92':
                case '\ufb93':
                case '\ufb94':
                case '\ufb95':
                case '\ufbfc':
                case '\ufbfd':
                case '\ufbfe':
                case '\ufbff':
                case '\ufef5':
                case '\ufef7':
                case '\ufef9':
                    return true;
            }
            return (ch >= '\u0621' && ch <= '\u063a')
                || (ch >= '\u0640' && ch <= '\u064a')
                || ch == '\u067e' || ch == '\u0686' || ch == '\u0698'
                || ch == '\u06a9' || ch == '\u06af' || ch == '\u06cc' || ch == '\u200c';
        }

        public static bool IsRTLInput(string input)
        {
            bool insideTag = false;
            for (int i = 0; i < input.Length; i++)
            {
                char ch = input[i];
                if (ch == '<')
                    insideTag = true;
                else if (ch == '>')
                    insideTag = false;
                else if (!insideTag && char.IsLetter(ch))
                    return IsRTLCharacter(ch);
            }
            return false;
        }
    }
}
