using System;

namespace Hardlight
{
	// Original HLUnityCore.Runtime 020001dd; bodies translated from the 1.10.1 x86-64 binary.
	public sealed class GraphShowIfEqualAttribute : GraphAttributeBase
	{
		public readonly string PropertyToCheck;

		public readonly object VisibleValue;

		// 06000c53 (1.10.1 RVA 0x1B06D50)
		public GraphShowIfEqualAttribute(string propertyName, object visibleValue)
		{
			PropertyToCheck = propertyName;
			VisibleValue = visibleValue;
		}
	}
}
