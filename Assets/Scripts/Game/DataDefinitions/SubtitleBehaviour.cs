using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.Playables;

namespace HardlightProject
{
	// Original Game.Runtime 02000449; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class SubtitleBehaviour : PlayableBehaviour
	{
		public string SubtitleText;

		public string SubtitleTextEditor;

		public Color BackgroundColour;

		// 06001952 (1.10.1 RVA 0x7A5E40)
		public SubtitleBehaviour()
		{
			BackgroundColour.a = 0.7f;
		}
	}
}
