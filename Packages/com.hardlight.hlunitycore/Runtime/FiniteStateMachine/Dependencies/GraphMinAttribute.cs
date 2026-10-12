using System;

namespace Hardlight
{
	// Original HLUnityCore.Runtime 020001db; bodies translated from the 1.10.1 x86-64 binary.
	public sealed class GraphMinAttribute : GraphAttributeBase
	{
		public readonly float Minimum;

		// 06000c51 (1.10.1 RVA 0x1B06CF0)
		public GraphMinAttribute(float minimum)
		{
			Minimum = minimum;
		}
	}
}
