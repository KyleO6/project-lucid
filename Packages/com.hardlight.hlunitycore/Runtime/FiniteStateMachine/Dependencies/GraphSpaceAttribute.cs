using System;

namespace Hardlight
{
	// Original HLUnityCore.Runtime 020001cd; bodies translated from the 1.10.1 x86-64 binary.
	public sealed class GraphSpaceAttribute : GraphAttributeBase
	{
		public readonly int Height;

		public readonly bool AfterProperty;

		// 06000c42 (1.10.1 RVA 0x1B069E0)
		public GraphSpaceAttribute(int height = 1, bool afterProperty = false)
		{
			Height = height;
			AfterProperty = afterProperty;
		}
	}
}
