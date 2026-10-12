using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000597; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "ProjectileDefinitionGroup", menuName = "HardlightProject/DefinitionData/Groups/ProjectileDefinitionGroup")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ProjectileDefinitionGroup : DefinitionDataType<ProjectileType, ProjectileDefinition>
	{
		// 06001ea7 (1.10.1 RVA 0x554F80)
		protected override ProjectileType GetElementKey(ProjectileDefinition data)
		{
			return data.ProjectileType;
		}

		// 06001ea8 (1.10.1 RVA 0x554F90)
		protected override IEqualityComparer<ProjectileType> GetKeyComparer()
		{
			return HardlightProject.HardlightEnumComparers.ProjectileTypeComparer;
		}

		// 06001ea9 (1.10.1 RVA 0x554FF0)
		public ProjectileDefinitionGroup()
		{
			
		}
	}
}
