using System;

namespace Hardlight.UI.Binding
{
	// Original HLBinders.Runtime 02000009; bodies translated from the 1.10.1 x86-64 binary.
	public class RestrictType : Attribute
	{
		public readonly Type TypeRestriction;

		// 06000008 (1.10.1 RVA 0x19F2FE0)
		public RestrictType(Type typeRestriction)
        {
            TypeRestriction = typeRestriction;
        }
	}
}
