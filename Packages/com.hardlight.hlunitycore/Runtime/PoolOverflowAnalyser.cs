using System;

namespace Hardlight.Pooling
{
	// Original HLUnityCore.Runtime 02000292; bodies translated from the 1.10.1 x86-64 binary.
	public static class PoolOverflowAnalyser
	{
		public const string SnapshotHeadMarker = "[P]";

		public const string PoolElementMarker = "(P)";

		public const string PoolOverflowMarker = "(P>";

		public const char IDSeparator = '|';
	}
}
