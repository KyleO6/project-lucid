using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000559; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "GameplayIslandDefinitionGroup", menuName = "HardlightProject/DefinitionData/Groups/GameplayIslandDefinitionGroup")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class GameplayIslandDefinitionGroup : DefinitionDataType<string, GameplayIslandDefinition>
	{
		// 06001d05 (1.10.1 RVA 0x54D480)
		protected override string GetElementKey(GameplayIslandDefinition data)
		{
			return data.name;
		}

		// 06001d06 (1.10.1 RVA 0x54D490)
		protected override IEqualityComparer<string> GetKeyComparer()
		{
			return null;
		}

		// 06001d07 (1.10.1 RVA 0x54D4A0)
		public GameplayIslandDefinitionGroup()
		{
			
		}
	}
}
