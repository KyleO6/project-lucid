using System;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 0200041c; bodies translated from the 1.10.1 x86-64 binary.
	[Serializable]
	public struct CollectableCount
	{
		public CollectableType Type;

		public int Total;

		[InspectorReadOnly]
		public string[] Guids;
	}
}
