using Unity.IL2CPP.CompilerServices;
using Hardlight.Enums;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020005fa; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class LanguageDebug : MonoBehaviour
	{
		[SerializeField]
		private Languages[] m_projectLanguages;

		// 0600201b (1.10.1 RVA 0x5604B0)
		public LanguageDebug()
		{
		}
	}
}
