using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020005e8; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "UIModernEventDefinitionGroup", menuName = "HardlightProject/DefinitionData/Groups/UIModernEventDefinitionGroup")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class UIModernEventDefinitionGroup : DefinitionDataType<string, UIModernEvent>
	{
		// 06001fb7 (1.10.1 RVA 0x55BA80)
		protected override string GetElementKey(UIModernEvent data)
		{
			return data.name;
		}

		// 06001fb8 (1.10.1 RVA 0x55BA90)
		protected override IEqualityComparer<string> GetKeyComparer()
		{
			return null;
		}

		// 06001fb9 (1.10.1 RVA 0x55BAA0)
		public UIModernEventDefinitionGroup()
		{
			
		}
	}
}
