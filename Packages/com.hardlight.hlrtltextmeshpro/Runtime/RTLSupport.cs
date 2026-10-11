// Original HLRTLTextMeshPro.Runtime RTLTMPro.RTLSupport 0200000c: two complete methods.
// Explicit static construction preserves the original absence of BeforeFieldInit.
// Shared buffers, provider order, optional true/true/false and normal-end-only Clear are retained.
// Source spelling, provider static initialization/fault order, concurrency and current runtime remain held.
namespace RTLTMPro
{
    public static class RTLSupport
    {
        public const int DefaultBufferSize = 2048;
        private static FastStringBuilder inputBuilder;
        private static FastStringBuilder glyphFixerOutput;

        static RTLSupport()
        {
            inputBuilder = new FastStringBuilder(DefaultBufferSize);
            glyphFixerOutput = new FastStringBuilder(DefaultBufferSize);
        }

        public static void FixRTL(string input, FastStringBuilder output, bool farsi = true, bool fixTextTags = true, bool preserveNumbers = false)
        {
            inputBuilder.SetValue(input);
            TashkeelFixer.RemoveTashkeel(inputBuilder);
            GlyphFixer.Fix(inputBuilder, glyphFixerOutput, preserveNumbers, farsi, fixTextTags);
            TashkeelFixer.RestoreTashkeel(glyphFixerOutput);
            TashkeelFixer.FixShaddaCombinations(glyphFixerOutput);
            LigatureFixer.Fix(glyphFixerOutput, output, farsi, fixTextTags, preserveNumbers);
            if (fixTextTags)
                RichTextFixer.Fix(output);
            inputBuilder.Clear();
        }
    }
}
