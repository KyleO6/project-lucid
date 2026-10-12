using System;

namespace Hardlight
{
	// Original HLUnityCore.Runtime 020001da; bodies translated from the 1.10.1 x86-64 binary.
	public sealed class GraphMaxAttribute : GraphAttributeBase
	{
		public readonly float Maximum;

		// 06000c50 (1.10.1 RVA 0x1B06CC0)
		public GraphMaxAttribute(float maximum)
		{
			Maximum = maximum;
		}
	}
}
