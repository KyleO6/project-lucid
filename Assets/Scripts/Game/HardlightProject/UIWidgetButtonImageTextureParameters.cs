using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine.AddressableAssets;

namespace HardlightProject
{
	// Original Game.Runtime 020009a2; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.NullChecks, false)]
	public class UIWidgetButtonImageTextureParameters : UIWidgetButtonParameters
	{
		public readonly AssetReferenceAtlasedSprite Image;

		// 060037c0 (1.10.1 RVA 0x69B250)
		public UIWidgetButtonImageTextureParameters(string label, Action callback, AssetReferenceAtlasedSprite image) : base(label, callback)
		{
			Image = image;
		}
	}
}
