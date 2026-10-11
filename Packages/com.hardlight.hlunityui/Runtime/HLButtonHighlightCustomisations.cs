// Original HLUnityUI.Runtime source preservation; source form, providers, native faults and Engine parity remain held.
// Global original owner; nested ChangeSource is Serializable. Wrap Once and four white Color components precede base construction; layer-data list remains unallocated.
// 0x0600000a .ctor arm64 0x1b40608..0x1b40618; x86_64 0x1b39370..0x1b39390.
// 0x0600000b .ctor arm64 0x1b40618..0x1b40628; x86_64 0x1b39390..0x1b393b0.

using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[CreateAssetMenu(menuName = "UI/Highlight Customisation Set")]
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
public sealed class HLButtonHighlightCustomisations : ScriptableObject
{
    [Serializable]
    public class ButtonHighlightChangeSource
    {
        public bool Enabled;
        public float Top;
        public float Right;
        public float Bottom;
        public float Left;
        public Sprite Sprite;
        public Material Material;
        public Color Color = Color.white;
        public float SkewX;
        public float SkewY;
    }

    public List<ButtonHighlightChangeSource> m_buttonHighlightLayerData;
    public AnimationClip m_animationClip;
    public WrapMode m_animationWrap = WrapMode.Once;
}
