using System;

namespace Hardlight
{
	// Original HLUnityCore.Runtime 020001d2; bodies translated from the 1.10.1 x86-64 binary.
	public sealed class GraphSliderAttribute : GraphAttributeBase
	{
		public readonly float Minimum;

		public readonly float Maximum;

		// 06000c48 (1.10.1 RVA 0x1B06B90)
		public GraphSliderAttribute(float minimum, float maximum)
		{
			Minimum = minimum;
			Maximum = maximum;
		}
	}
}
