using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 0200056a; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "LevelStartPositionDefinitionGroup", menuName = "HardlightProject/DefinitionData/Groups/LevelStartPositionDefinitionGroup")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class LevelStartPositionDefinitionGroup : DefinitionDataType<string, LevelStartPositionDefinition>
	{
		// 06001d40 (1.10.1 RVA 0x54DD00)
		protected override string GetElementKey(LevelStartPositionDefinition data)
		{
			return data.name;
		}

		// 06001d41 (1.10.1 RVA 0x54DD10)
		protected override IEqualityComparer<string> GetKeyComparer()
		{
			return null;
		}

		// 06001d42 (1.10.1 RVA 0x54DD20)
		public LevelStartPositionDefinitionGroup()
		{
			
		}
	}
}
