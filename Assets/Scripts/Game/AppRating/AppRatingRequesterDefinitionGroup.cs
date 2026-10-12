using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 0200047a; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "AppRatingRequesterDefinitionGroup", menuName = "HardlightProject/DefinitionData/Groups/AppRatingRequesterDefinitionGroup")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class AppRatingRequesterDefinitionGroup : DefinitionDataType<string, AppRatingRequesterBase>
	{
		// 060019f9 (1.10.1 RVA 0x53DC80)
		protected override string GetElementKey(AppRatingRequesterBase data)
		{
			return data.name;
		}

		// 060019fa (1.10.1 RVA 0x53DC90)
		protected override IEqualityComparer<string> GetKeyComparer()
		{
			return null;
		}

		// 060019fb (1.10.1 RVA 0x53DCA0)
		public AppRatingRequesterDefinitionGroup()
		{
			
		}
	}
}
