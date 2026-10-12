using Unity.IL2CPP.CompilerServices;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000441; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "GlobalSignal", menuName = "HardlightProject/Signals/GlobalSignal")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class GlobalSignal : ScriptableObjectWithGuid
	{
		// 06001936 (1.10.1 RVA 0x7A4CA0)
		public GlobalSignal()
		{
		}
	}
}
