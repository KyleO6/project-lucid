using System;

namespace Hardlight
{
	// Original HLUnityCore.Runtime 020001dc; bodies translated from the 1.10.1 x86-64 binary.
	public sealed class GraphShowIfAttribute : GraphAttributeBase
	{
		public readonly string PropertyToCheck;

		// 06000c52 (1.10.1 RVA 0x1B06D20)
		public GraphShowIfAttribute(string boolPropertyName)
		{
			PropertyToCheck = boolPropertyName;
		}
	}
}
