using UnityEngine;

namespace Hardlight
{
	// Original HLUnityCore.Runtime 02000275; bodies translated from the 1.10.1 x86-64 binary.
	public class HorizontalLineAttribute : PropertyAttribute
	{
		public readonly int Height;

		// 06000f7e (1.10.1 RVA 0x1B1F1C0)
		public HorizontalLineAttribute(int height = 4)
		{
			Height = height;
		}
	}
}
