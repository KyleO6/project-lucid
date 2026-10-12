using System;
using UnityEngine;

namespace Hardlight
{
	// Original HLUnityCore.Runtime 02000282; bodies translated from the 1.10.1 x86-64 binary.
	[Serializable]
	public class RangeProperty
	{
		[SerializeField]
		public float Min;

		[SerializeField]
		public float Max;

		public float NextValue
		{
			get
			{
				return UnityEngine.Random.Range(Min, Max);
			}
		}

		// 06000fa1 (1.10.1 RVA 0x1B1FB60)
		public RangeProperty(float min, float max)
		{
			Min = min;
			Max = max;
		}
	}
}
