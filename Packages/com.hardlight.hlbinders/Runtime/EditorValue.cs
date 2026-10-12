using System;

namespace Hardlight.UI.Binding
{
	// Original HLBinders.Runtime 02000005; bodies translated from the 1.10.1 x86-64 binary.
	public class EditorValue : Attribute
	{
		public readonly object DefaultValue;

		// 06000004 (1.10.1 RVA 0x19F2F80)
		public EditorValue(object defaultValue)
        {
            DefaultValue = defaultValue;
        }
	}
}
