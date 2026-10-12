using UnityEngine;
using System;

namespace Hardlight
{
	// Original HLUnityCore.Runtime 02000281; bodies translated from the 1.10.1 x86-64 binary.
	public class PrefixAttribute : PropertyAttribute
	{
		public readonly string Prefix;

		// 06000fa0 (1.10.1 RVA 0x1B1FB30)
		public PrefixAttribute(string prefix)
		{
			Prefix = prefix;
		}
	}
}
