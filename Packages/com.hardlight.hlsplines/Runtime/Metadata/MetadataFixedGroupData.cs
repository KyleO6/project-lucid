using Unity.IL2CPP.CompilerServices;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hardlight
{
	// Original HLSplines.Runtime 02000008; bodies translated from the 1.10.1 x86-64 binary.
	[Serializable]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class MetadataFixedGroupData
	{
		[SerializeField]
		private MetadataFixedGroupDefinition m_definition;

		[SerializeField]
		private List<MetadataFixed> m_metadata = new List<MetadataFixed>();

		public IReadOnlyList<Metadata> Metadata
		{
			get
{ return m_metadata; }
		}

		// 06000014 (1.10.1 RVA 0x1A53A30)
		public MetadataFixedGroupData()
		{
		}
	}
}
