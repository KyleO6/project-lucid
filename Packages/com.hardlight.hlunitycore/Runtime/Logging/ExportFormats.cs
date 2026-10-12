using System;

namespace Hardlight
{
	// Original HLUnityCore.Runtime 02000172; bodies translated from the 1.10.1 x86-64 binary.
	[Flags]
	public enum ExportFormats
	{
		Txt = 2,
		Json = 4,
		Markdown = 8,
		All = 0xE
	}
}
