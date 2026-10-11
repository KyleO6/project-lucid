// Shipping HLRTLTextMeshPro.Runtime RichTextFixer: complete metadata and dual native reconstruction.
// Inferred local names/C# spelling, optimizer faults, providers, current emission and runtime equivalence remain held.
using System;

namespace RTLTMPro
{
    public static class RichTextFixer
    {
        public enum TagType
        {
            None = 0,
            Opening = 1,
            Closing = 2,
            SelfContained = 3
        }

        public struct Tag
        {
            public int Start;
            public int End;
            public int HashCode;
            public TagType Type;

            public Tag(int start, int end, TagType type, int hashCode)
            {
                Start = start;
                End = end;
                Type = type;
                HashCode = hashCode;
            }
        }

        public static void Fix(FastStringBuilder text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                FindTag(text, i, out Tag tag);
                if (tag.Type == TagType.None)
                    break;
                text.Reverse(tag.Start, tag.End - tag.Start + 1);
                i = tag.End;
            }
        }

        public static void FindTag(FastStringBuilder str, int start, out Tag tag)
        {
            for (int i = start; i < str.Length; i++)
            {
                if (str.Get(i) != '<')
                    continue;
                tag.HashCode = 0;
                bool inName = true;
                for (int j = i + 1; j < str.Length; j++)
                {
                    int current = str.Get(j);
                    if (inName)
                    {
                        if (current <= 0xfffe && char.IsLetter((char)current))
                            tag.HashCode = tag.HashCode == 0 ? current.GetHashCode()
                                : unchecked(tag.HashCode * 397) ^ current.GetHashCode();
                        else
                            inName = tag.HashCode == 0;
                    }
                    if ((current == ' ' && j == i + 1) || current == '<')
                        break;
                    if (current == '>')
                    {
                        tag.Start = i;
                        tag.End = j;
                        tag.Type = str.Get(j - 1) == '/' ? TagType.SelfContained
                            : str.Get(i + 1) == '/' ? TagType.Closing : TagType.Opening;
                        return;
                    }
                }
            }
            tag = default(Tag);
        }
    }
}
