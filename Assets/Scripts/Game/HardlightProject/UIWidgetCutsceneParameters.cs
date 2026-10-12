using Unity.IL2CPP.CompilerServices;
using System;
using System.Collections.Generic;
using Hardlight;
using Hardlight.Enums;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HardlightProject
{
	// Original Game.Runtime 020009b1; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public sealed class UIWidgetCutsceneParameters : UIWidgetButtonParameters
	{
		public readonly bool Locked;

		public readonly AssetReferenceT<Texture> ImageAssetReference;

		public readonly bool IsNew;

		// 060037fd (1.10.1 RVA 0x69DB60)
		public UIWidgetCutsceneParameters(string label, Action callback, bool locked, AssetReferenceT<Texture> imageAssetReference, bool isNew) : base(label, callback)
		{
			Locked = locked;
			ImageAssetReference = imageAssetReference;
			IsNew = isNew;
		}

		// 060037fe (1.10.1 RVA 0x69DBB0)
		public UIWidgetCutsceneParameters(Strings label, Action callback, bool locked, AssetReferenceT<Texture> imageAssetReference, bool isNew) : base(label, callback)
		{
			Locked = locked;
			ImageAssetReference = imageAssetReference;
			IsNew = isNew;
		}
	}
}
