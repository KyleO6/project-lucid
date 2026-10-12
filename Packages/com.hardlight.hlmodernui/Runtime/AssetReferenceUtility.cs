using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Hardlight.Utilities
{
	// Original HLModernUI.Runtime 0200004c; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public static class AssetReferenceUtility
	{
		// 060001df (1.10.1 RVA 0x1A4A070)
		public static bool SetAtlasReference(Object unityObject, AssetReferenceAtlasedSprite atlasSpriteField, AssetReference nonAtlasReference)
		{
			return false;
		}
	}
}
