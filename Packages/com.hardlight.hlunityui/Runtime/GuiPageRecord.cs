// Original HLUnityUI.Runtime Hardlight.GuiPageRecord<T>, TypeDef02000047.
// The genuine T constraint is GuiPage; the callback is PageStackSystem<T>.OnPageLoaded.
// Open MethodDef06000203 has zero ordinary pointers. VAR MethodSpec36750 has no function record; OBJECT shared role36751 has one body per CPU.
// 06000203 shared ARM64 0x1389a48..0x1389aa0; x86_64 0x1392310..0x1392370.
// Object construction precedes identifier, data and callback stores. References are retained without invocation or null repair.
// Generic open field offsets0 are distinct from shared-body stores at16/24/32; no current layout equivalence is claimed.
// No original attributes, defaults, events, queue or natural child owners exist. Engine/generic-provider/fault/source-form holds remain.
namespace Hardlight
{
    public class GuiPageRecord<T> where T : GuiPage
    {
        public int m_identifier;
        public GuiPageParameters m_data;
        public PageStackSystem<T>.OnPageLoaded m_onLoadedCallback;

        public GuiPageRecord(int identifier, GuiPageParameters data, PageStackSystem<T>.OnPageLoaded callback)
        {
            m_identifier = identifier;
            m_data = data;
            m_onLoadedCallback = callback;
        }
    }
}
