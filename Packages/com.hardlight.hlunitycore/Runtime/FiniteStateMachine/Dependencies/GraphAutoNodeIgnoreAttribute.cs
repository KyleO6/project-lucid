using System;

namespace Hardlight
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	// Original HLUnityCore.Runtime 020001c8; bodies translated from the 1.10.1 x86-64 binary.
	public sealed class GraphAutoNodeIgnoreAttribute : Attribute
	{
		// 06000c3a (1.10.1 RVA 0x1B067B0)
		public GraphAutoNodeIgnoreAttribute()
		{
		}
	}
}
