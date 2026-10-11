// Original HLUnityUI.Runtime GuiBackgroundManager/Active. Reconstructed from whole metadata and both native slices.
// Unknown Active values select None; NoChange returns. Dark canvas updates precede Default. Unity equality and ordinary MonoSingleton constructor remain genuine.
// Source spelling, receiver mutation/fault equivalence, provider algorithms, current emission and Engine/bootstrap acceptance remain held.
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Hardlight
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class GuiBackgroundManager : MonoSingleton<GuiBackgroundManager>
    {
        [SerializeField] private Canvas m_defaultBackground;
        [SerializeField] private Canvas m_darkBackground;

        public enum Active
        {
            None = 0,
            Default = 1,
            Dark = 2,
            NoChange = 3
        }

        public void SetBackgroundActive(Active active)
        {
            switch (active)
            {
                case Active.NoChange: return;
                case Active.Dark: SetUIBackgroundToDark(); return;
                case Active.Default: SetUIBackgroundToDefault(); return;
                default: SetUIBackgroundToNone(); return;
            }
        }

        public void SetUIBackgroundToDefault()
        {
            DarkBackgroundActive = false;
            DefaultBackgroundActive = true;
        }

        public void SetUIBackgroundToDark()
        {
            DarkBackgroundActive = true;
            DefaultBackgroundActive = false;
        }

        public void SetUIBackgroundToNone()
        {
            DarkBackgroundActive = false;
            DefaultBackgroundActive = false;
        }

        private bool DarkBackgroundActive
        {
            set { if (m_darkBackground != null) m_darkBackground.enabled = value; }
        }

        private bool DefaultBackgroundActive
        {
            set { if (m_defaultBackground != null) m_defaultBackground.enabled = value; }
        }

        public GuiBackgroundManager() { }
    }
}
