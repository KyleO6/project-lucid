using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000524; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "CharacterStickyControlsDefinitionGroup", menuName = "HardlightProject/DefinitionData/Groups/CharacterStickyControlsDefinitionGroup")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterStickyControlsDefinitionGroup : DefinitionDataType<string, CharacterStickyControlsDefinition>
	{
		// 06001c0b (1.10.1 RVA 0x547820)
		protected override string GetElementKey(CharacterStickyControlsDefinition data)
		{
			return data.name;
		}

		// 06001c0c (1.10.1 RVA 0x547830)
		protected override IEqualityComparer<string> GetKeyComparer()
		{
			return null;
		}

		// 06001c0d (1.10.1 RVA 0x547840)
		public CharacterStickyControlsDefinitionGroup()
		{
			
		}
	}
}
