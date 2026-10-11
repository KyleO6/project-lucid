// Original HLRTLTextMeshPro.Runtime FastStringBuilder, reconstructed from complete metadata
// and all 56 shipping ARM64/x86_64 ranges. Original C# spelling and optimizer fault order
// remain inferred; no current compilation or runtime equivalence is claimed.
// Preserve full-capacity copying, shrink-only Length (including negative values), unchecked
// growth, scalar conversion faults, replacement shift quirks and the extra match increment.
// Local names and delegation/inlining spelling are inferred. No upstream replacement.
using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace RTLTMPro
{
    public class FastStringBuilder
    {
        private int length;
        private int[] array;
        private int capacity;

        public int Length
        {
            get { return length; }
            set
            {
                if (value <= length)
                    length = value;
            }
        }

        public FastStringBuilder(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("capacity");
            this.capacity = capacity;
            array = new int[capacity];
        }

        public FastStringBuilder(string text) : this(text.Length)
        {
            SetValue(text);
        }

        public FastStringBuilder(string text, int capacity) : this(capacity)
        {
            SetValue(text);
        }

        public static implicit operator string(FastStringBuilder x)
        {
            return x.ToString();
        }

        public static implicit operator FastStringBuilder(string x)
        {
            return new FastStringBuilder(x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Get(int index)
        {
            return array[index];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(int index, int ch)
        {
            array[index] = ch;
        }

        public void SetValue(string text)
        {
            length = text.Length;
            EnsureCapacity(length, false);
            int written = 0;
            for (int i = 0; i < text.Length; i++)
            {
                int codePoint = char.ConvertToUtf32(text, i);
                array[written] = codePoint;
                if (codePoint >= 0x10000)
                    i++;
                written++;
            }
            length = written;
        }

        public void SetValue(FastStringBuilder other)
        {
            EnsureCapacity(other.length, false);
            Copy(other.array, array);
            length = other.length;
        }

        public void Append(int ch)
        {
            length = unchecked(length + 1);
            EnsureCapacity(length, true);
            array[unchecked(length - 1)] = ch;
        }

        public void Append(char ch)
        {
            length = unchecked(length + 1);
            EnsureCapacity(length, true);
            array[unchecked(length - 1)] = ch;
        }

        public void Insert(int pos, FastStringBuilder str, int offset, int count)
        {
            if (str == this)
                throw new InvalidOperationException("You cannot pass the same string builder to insert");
            if (count == 0)
                return;
            unchecked
            {
                length += count;
                EnsureCapacity(length, true);
                for (int i = length - count - 1; i >= pos; i--)
                    array[i + count] = array[i];
                for (int i = 0; i < count; i++)
                    array[pos + i] = str.array[offset + i];
            }
        }

        public void Insert(int pos, FastStringBuilder str)
        {
            Insert(pos, str, 0, str.length);
        }

        public void Insert(int pos, int ch)
        {
            unchecked
            {
                length++;
                EnsureCapacity(length, true);
                for (int i = length - 2; i >= pos; i--)
                    array[i + 1] = array[i];
                array[pos] = ch;
            }
        }

        public void RemoveAll(int character)
        {
            int written = 0;
            for (int i = 0; i < length; i++)
            {
                if (array[i] != character)
                    array[written++] = array[i];
            }
            length = written;
        }

        public void Remove(int start, int length)
        {
            unchecked
            {
                for (int i = start; i < this.length - length; i++)
                    array[i] = array[i + length];
                this.length -= length;
            }
        }

        public void Reverse(int startIndex, int length)
        {
            unchecked
            {
                for (int i = 0; i < length / 2; i++)
                {
                    int value = array[startIndex + i];
                    array[startIndex + i] = array[startIndex + length - 1 - i];
                    array[startIndex + length - 1 - i] = value;
                }
            }
        }

        public void Reverse()
        {
            Reverse(0, length);
        }

        public void Substring(FastStringBuilder output, int start, int length)
        {
            output.Clear();
            for (int i = 0; i < length; i++)
                output.Append(array[unchecked(start + i)]);
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < length; i++)
                result.Append(char.ConvertFromUtf32(array[i]));
            return result.ToString();
        }

        public string ToDebugString()
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                result.Append("\\");
                result.Append(array[i].ToString("X"));
            }
            return result.ToString();
        }

        public void Replace(int oldChar, int newChar)
        {
            for (int i = 0; i < length; i++)
            {
                if (array[i] == oldChar)
                    array[i] = newChar;
            }
        }

        public void Replace(FastStringBuilder oldStr, FastStringBuilder newStr)
        {
            unchecked
            {
                for (int i = 0; i < length; i++)
                {
                    bool matches = true;
                    for (int j = 0; j < oldStr.length; j++)
                    {
                        if (array[i + j] != oldStr.array[j])
                        {
                            matches = false;
                            break;
                        }
                    }
                    if (!matches)
                        continue;
                    if (oldStr.length == newStr.length)
                    {
                        for (int j = 0; j < oldStr.length; j++)
                            array[i + j] = newStr.array[j];
                    }
                    else if (oldStr.length < newStr.length)
                    {
                        int difference = newStr.length - oldStr.length;
                        length += difference;
                        EnsureCapacity(length, true);
                        for (int j = length - difference - 1; j >= i + oldStr.length; j--)
                            array[j + difference] = array[j];
                        for (int j = 0; j < newStr.length; j++)
                            array[i + j] = newStr.array[j];
                    }
                    else
                    {
                        int difference = oldStr.length - newStr.length;
                        for (int j = i + difference; j < length - difference; j++)
                            array[j] = array[j + difference];
                        for (int j = 0; j < newStr.length; j++)
                            array[i + j] = newStr.array[j];
                        length -= difference;
                    }
                    i += newStr.length;
                }
            }
        }

        public void Clear()
        {
            length = 0;
        }

        private void EnsureCapacity(int cap, bool keepValues)
        {
            if (capacity >= cap)
                return;
            if (capacity == 0)
                capacity = 1;
            while (capacity < cap)
                capacity = unchecked(capacity * 2);
            int[] resized = new int[capacity];
            if (keepValues)
                Copy(array, resized);
            array = resized;
        }

        private static void Copy(int[] src, int[] dst)
        {
            for (int i = 0; i < src.Length; i++)
                dst[i] = src[i];
        }
    }
}
