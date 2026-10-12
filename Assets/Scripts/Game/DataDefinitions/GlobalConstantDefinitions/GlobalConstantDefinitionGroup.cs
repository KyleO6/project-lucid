using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 0200055c; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "GlobalConstantDefinitionGroup", menuName = "HardlightProject/DefinitionData/Groups/GlobalConstantDefinitionGroup")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class GlobalConstantDefinitionGroup : DefinitionDataType<GlobalConstantType, GlobalConstantDefinition>
	{
		// 06001d0f (1.10.1 RVA 0x54D570)
		protected override GlobalConstantType GetElementKey(GlobalConstantDefinition data)
		{
			return data.Type;
		}

		// 06001d10 (1.10.1 RVA 0x54D580)
		protected override IEqualityComparer<GlobalConstantType> GetKeyComparer()
		{
			return HardlightProject.HardlightEnumComparers.GlobalConstantTypeComparer;
		}

		// 06001d11 (1.10.1 RVA 0x54D5E0)
		public GlobalConstantDefinitionGroup()
		{
			
		}
	}
}
