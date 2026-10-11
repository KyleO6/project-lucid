// Original Game.Runtime.dll HardlightProject.FontAssetAndFallbacks; complete shipping declaration and dual native evidence.
// The C# reconstruction preserves the original algorithm; source spelling, compiler-generated layout and native fault equivalence remain underdetermined.
// Original method evidence below uses shipping MethodDef tokens and half-open native address ranges (ARM64; x86_64).
// 0x060035a6 HardlightProject.FontAssetAndFallbacks.OnBeforeSerialize: ARM64 0x65fa80..0x65fbec; x86_64 0x684d90..0x684f10.
// 0x060035a7 HardlightProject.FontAssetAndFallbacks.OnAfterDeserialize: ARM64 0x65fbec..0x65fbf0; x86_64 0x684f10..0x684f20.
// 0x060035a8 HardlightProject.FontAssetAndFallbacks..ctor: ARM64 0x65fbf0..0x65fbf8; x86_64 0x684f20..0x684f30.
// 0x060035a9 HardlightProject.FontAssetAndFallbacks+FontAssetFallbackPairing..ctor: ARM64 0x65fbf8..0x65fc00; x86_64 0x684f30..0x684f40.
// OnBeforeSerialize assigns each pairing Name from the original language enum ToString; it supplies no null guards. OnAfterDeserialize is a literal no-op.
// The nested pairing preserves the original Serializable TypeDef bit and ordered IL2CPP options; neither original constructor creates serialized collection defaults.
// Font assets, Addressable references, serialization lifecycle and actual Engine/resource readiness remain separately held.
using System;
using System.Collections.Generic;
using Hardlight.Enums;
using TMPro;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HardlightProject
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class FontAssetAndFallbacks : ScriptableObject, ISerializationCallbackReceiver
    {
        [Serializable]
        [Il2CppSetOption(Option.NullChecks, false)]
        [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
        public class FontAssetFallbackPairing
        {
            [HideInInspector]
            public string Name;
            public Languages m_language;
            public List<AssetReferenceT<TMP_FontAsset>> m_fontsForLanguage;
        }

        public TMP_FontAsset m_fontAsset;
        public List<FontAssetFallbackPairing> m_fallbackCollection;

        public void OnBeforeSerialize()
        {
            foreach (var pairing in m_fallbackCollection)
                pairing.Name = pairing.m_language.ToString();
        }

        public void OnAfterDeserialize()
        {
        }
    }
}
