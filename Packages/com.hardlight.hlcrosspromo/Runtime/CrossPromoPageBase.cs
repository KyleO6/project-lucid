using Unity.IL2CPP.CompilerServices;
using System;
using UnityEngine;

namespace Hardlight
{
	// Original HLCrossPromo.Runtime 02000020; bodies translated from the 1.10.1 x86-64 binary.
	[Serializable]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public abstract class CrossPromoPageBase : MonoBehaviour
	{
		public abstract void SetSprite(Sprite sprite);

		public abstract Sprite GetSprite();

		public abstract void SetText(string text);

		public abstract void SetReadyState(bool isReady);

		public abstract bool GetReadyState();

		// 06000102 (1.10.1 RVA 0x1A18940)
		protected CrossPromoPageBase()
		{
		}
	}
}
