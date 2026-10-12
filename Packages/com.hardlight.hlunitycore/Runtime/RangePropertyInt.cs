using System;
using UnityEngine;

namespace Hardlight
{
	// Original HLUnityCore.Runtime 02000283; bodies translated from the 1.10.1 x86-64 binary.
	[Serializable]
	public class RangePropertyInt
	{
		[SerializeField]
		public int Min;

		[SerializeField]
		public int Max;

		public int NextValue
		{
			get
			{
				return UnityEngine.Random.Range(Min, Max + 1);
			}
		}

		// 06000fa3 (1.10.1 RVA 0x1B1FBC0)
		public RangePropertyInt(int min, int max)
		{
			Min = min;
			Max = max;
		}
	}
}
