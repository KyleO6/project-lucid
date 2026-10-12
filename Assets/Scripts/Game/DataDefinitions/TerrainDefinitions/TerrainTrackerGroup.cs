using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020005d0; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "TerrainTrackerGroup", menuName = "HardlightProject/DefinitionData/Groups/TerrainTrackerGroup")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class TerrainTrackerGroup : DefinitionDataType<TerrainTrackerType, TerrainTrackerDefinition>
	{
		// 06001f6d (1.10.1 RVA 0x55AE40)
		protected override TerrainTrackerType GetElementKey(TerrainTrackerDefinition data)
		{
			return data.MetadataType;
		}

		// 06001f6e (1.10.1 RVA 0x55AE50)
		protected override IEqualityComparer<TerrainTrackerType> GetKeyComparer()
		{
			return HardlightProject.HardlightEnumComparers.TerrainTrackerTypeComparer;
		}

		// 06001f6f (1.10.1 RVA 0x55AEB0)
		public TerrainTrackerGroup()
		{
			
		}
	}
}
