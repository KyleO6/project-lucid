using UnityEngine;
using System;

namespace Hardlight
{
	// Original HLUnityCore.Runtime 02000280; bodies translated from the 1.10.1 x86-64 binary.
	public class PostfixAttribute : PropertyAttribute
	{
		public readonly string Postfix;

		// 06000f9f (1.10.1 RVA 0x1B1FB00)
		public PostfixAttribute(string postfix)
		{
			Postfix = postfix;
		}
	}
}
