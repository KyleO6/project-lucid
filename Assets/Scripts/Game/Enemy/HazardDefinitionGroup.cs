using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000564; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "HazardDefinitionGroup", menuName = "HardlightProject/DefinitionData/Groups/HazardDefinitionGroup")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class HazardDefinitionGroup : DefinitionDataType<HazardType, HazardDefinition>
	{
		// 06001d2a (1.10.1 RVA 0x54DA20)
		protected override HazardType GetElementKey(HazardDefinition data)
		{
			return data.Type;
		}

		// 06001d2b (1.10.1 RVA 0x54DA30)
		protected override IEqualityComparer<HazardType> GetKeyComparer()
		{
			return HardlightProject.HardlightEnumComparers.HazardTypeComparer;
		}

		// 06001d2c (1.10.1 RVA 0x54DA90)
		public HazardDefinitionGroup()
		{
			
		}
	}
}
