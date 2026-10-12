using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000514; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "CharacterActionGlyphMapGroup", menuName = "HardlightProject/DefinitionData/Groups/CharacterActionGlyphMapGroup")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterActionGlyphMapGroup : DefinitionDataType<InputType, GameActionGlyphMap>
	{
		// 06001bc3 (1.10.1 RVA 0x5462E0)
		protected override InputType GetElementKey(GameActionGlyphMap data)
		{
			return data.InputType;
		}

		// 06001bc4 (1.10.1 RVA 0x5462F0)
		protected override IEqualityComparer<InputType> GetKeyComparer()
		{
			return HardlightInputEnumComparers.InputTypeComparer;
		}

		// 06001bc5 (1.10.1 RVA 0x546340)
		public CharacterActionGlyphMapGroup()
		{
			
		}
	}
}
