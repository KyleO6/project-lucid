using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020005ca; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "TerrainDefinitionGroup", menuName = "HardlightProject/DefinitionData/Groups/TerrainDefinitionGroup")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class TerrainDefinitionGroup : DefinitionDataType<TerrainMetadataType, TerrainMetadataDefinition>
	{
		// 06001f60 (1.10.1 RVA 0x55ACC0)
		protected override TerrainMetadataType GetElementKey(TerrainMetadataDefinition data)
		{
			return data.MetadataType;
		}

		// 06001f61 (1.10.1 RVA 0x55ACD0)
		protected override IEqualityComparer<TerrainMetadataType> GetKeyComparer()
		{
			return HardlightProject.HardlightEnumComparers.TerrainMetadataTypeComparer;
		}

		// 06001f62 (1.10.1 RVA 0x55AD30)
		public TerrainDefinitionGroup()
		{
			
		}
	}
}
