using Unity.IL2CPP.CompilerServices;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Hardlight
{
	// Original HLUnityCore.Runtime 02000287; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class SerializableDictionaryAttribute : PropertyAttribute
	{
		public float KeyValueWidthRatio { get; private set; }

		public string KeyProperty { get; private set; }

		public string ValueProperty { get; private set; }

		// 06000fb7 (1.10.1 RVA 0x1B1FC90)
		public SerializableDictionaryAttribute()
			: this("m_value")
		{
		}

		// 06000fb8 (1.10.1 RVA 0x1B1FD20)
		public SerializableDictionaryAttribute(float keyValueWidthRatio)
			: this("m_value", keyValueWidthRatio)
		{
		}

		// 06000fb9 (1.10.1 RVA 0x1B1FE40)
		public SerializableDictionaryAttribute(string valueProperty)
			: this(valueProperty, 0.3f)
		{
		}

		// 06000fba (1.10.1 RVA 0x1B1FDC0)
		public SerializableDictionaryAttribute(string valueProperty, float keyValueWidthRatio)
		{
			KeyProperty = "m_key";
			ValueProperty = valueProperty;
			KeyValueWidthRatio = keyValueWidthRatio;
		}
	}
}
