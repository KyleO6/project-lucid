using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020005e7; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "UIContainerIdentifierGroup", menuName = "HardlightProject/DefinitionData/Groups/UIContainerIdentifierGroup")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class UIContainerIdentifierGroup : DefinitionDataType<string, UIContainerIdentifier>
	{
		// 06001fb4 (1.10.1 RVA 0x55BA20)
		protected override string GetElementKey(UIContainerIdentifier data)
		{
			return data.Name;
		}

		// 06001fb5 (1.10.1 RVA 0x55BA30)
		protected override IEqualityComparer<string> GetKeyComparer()
		{
			return null;
		}

		// 06001fb6 (1.10.1 RVA 0x55BA40)
		public UIContainerIdentifierGroup()
		{
			
		}
	}
}
