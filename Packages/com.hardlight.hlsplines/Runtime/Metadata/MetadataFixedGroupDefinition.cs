using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using UnityEngine;

namespace Hardlight
{
	// Original HLSplines.Runtime 02000009; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "MetadataFixedGroupDefinition", menuName = "Hardlight/Metadata/MetadataFixedGroupDefinition", order = 0)]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class MetadataFixedGroupDefinition : ScriptableObjectWithGuid
	{
		[SerializeField]
		private List<MetadataKeyType> m_keys;

		public List<MetadataKeyType> Keys
		{
			get
{ return m_keys; }
		}

		// 06000016 (1.10.1 RVA 0x1A53AE0)
		public MetadataFixedGroupDefinition()
		{
		}
	}
}
