// Shipping HLRTLTextMeshPro.Runtime LigatureFixer: complete metadata and dual native reconstruction.
// Inferred local names/C# spelling, optimizer faults, providers, current emission and runtime equivalence remain held.
using System;
using System.Collections.Generic;

namespace RTLTMPro
{
    public static class LigatureFixer
    {
        private static readonly List<int> LtrTextHolder = new List<int>(512);
        private static readonly List<int> TagTextHolder = new List<int>(512);
        private static readonly Dictionary<char, char> MirroredCharsMap = new Dictionary<char, char>
        {
            ['('] = ')', [')'] = '(', ['»'] = '«', ['«'] = '»'
        };
        private static readonly HashSet<char> MirroredCharsSet = new HashSet<char>(MirroredCharsMap.Keys);

        private static void FlushBufferToOutput(List<int> buffer, FastStringBuilder output)
        {
            for (int i = 0; i < buffer.Count; i++)
                output.Append(buffer[buffer.Count - 1 - i]);
            buffer.Clear();
        }

        public static void Fix(FastStringBuilder input, FastStringBuilder output, bool farsi, bool fixTextTags, bool preserveNumbers)
        {
            LtrTextHolder.Clear();
            TagTextHolder.Clear();
            for (int i = input.Length - 1; i >= 0; i--)
            {
                int current = input.Get(i);
                int next = i < input.Length - 1 ? input.Get(i + 1) : 0;
                int previous = i > 0 ? input.Get(i - 1) : 0;
                bool edge = i == 0 || i >= input.Length - 1;
                if (fixTextTags && current == '>')
                {
                    TagTextHolder.Add(current);
                    int j;
                    for (j = i - 1; j >= 0; j--)
                    {
                        int tagCharacter = input.Get(j);
                        TagTextHolder.Add(tagCharacter);
                        if (tagCharacter == '<')
                            break;
                    }
                    if (j >= 0 && input.Get(j + 1) != ' ')
                    {
                        FlushBufferToOutput(LtrTextHolder, output);
                        FlushBufferToOutput(TagTextHolder, output);
                        i = j;
                        continue;
                    }
                    TagTextHolder.Clear();
                }
                if ((current <= 0xfffe && char.IsPunctuation((char)current))
                    || (current <= 0xfffe && char.IsSymbol((char)current)))
                {
                    if (MirroredCharsSet.Contains((char)current)
                        && ((previous <= 0xfffe && TextUtils.IsRTLCharacter((char)previous))
                            || (next <= 0xfffe && TextUtils.IsRTLCharacter((char)next))))
                        current = MirroredCharsMap[(char)current];
                    if (edge)
                    {
                        if (i == input.Length - 1)
                            LtrTextHolder.Add(current);
                        else if (i == 0)
                            output.Append(current);
                        continue;
                    }
                    bool previousRTL = previous <= 0xfffe && TextUtils.IsRTLCharacter((char)previous);
                    bool nextRTL = next <= 0xfffe && TextUtils.IsRTLCharacter((char)next);
                    bool nextWhitespace = next <= 0xfffe && char.IsWhiteSpace((char)next);
                    bool previousWhitespace = previous <= 0xfffe && char.IsWhiteSpace((char)previous);
                    if ((previousRTL && nextRTL)
                        || (previousWhitespace && (current == '.' || current == '\u060c' || current == '\u061b'))
                        || (previousRTL && nextWhitespace)
                        || (nextRTL && previousWhitespace)
                        || (current == '_' && (previousRTL || nextRTL)))
                    {
                        FlushBufferToOutput(LtrTextHolder, output);
                        output.Append(current);
                    }
                    else
                        LtrTextHolder.Add(current);
                    continue;
                }
                if (!edge)
                {
                    bool previousSymbol = previous <= 0xfffe && char.IsSymbol((char)previous);
                    bool nextSymbol = next <= 0xfffe && char.IsSymbol((char)next);
                    if (current == ' '
                        && ((previous <= 0xfffe && (TextUtils.IsEnglishLetter((char)previous)
                            || TextUtils.IsNumber((char)previous, preserveNumbers, farsi))) || previousSymbol)
                        && ((next <= 0xfffe && (TextUtils.IsEnglishLetter((char)next)
                            || TextUtils.IsNumber((char)next, preserveNumbers, farsi))) || nextSymbol))
                    {
                        LtrTextHolder.Add(current);
                        continue;
                    }
                }
                if (current <= 0xfffe && (TextUtils.IsEnglishLetter((char)current)
                    || TextUtils.IsNumber((char)current, preserveNumbers, farsi)))
                {
                    LtrTextHolder.Add(current);
                    continue;
                }
                if ((current >= 0xd800 && current <= 0xdbff) || (current >= 0xdc00 && current <= 0xdfff))
                {
                    LtrTextHolder.Add(current);
                    continue;
                }
                FlushBufferToOutput(LtrTextHolder, output);
                if (current != '\u200c' && current != 0xffff)
                    output.Append(current);
            }
            FlushBufferToOutput(LtrTextHolder, output);
        }
    }
}
