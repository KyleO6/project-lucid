using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020005c4; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "RewardTrackDefinitionGroup", menuName = "HardlightProject/DefinitionData/Groups/RewardTrackDefinitionGroup")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class RewardTrackDefinitionGroup : DefinitionDataType<RewardTrackType, RewardTrackDefinition>
	{
		// 06001f49 (1.10.1 RVA 0x55A1F0)
		protected override RewardTrackType GetElementKey(RewardTrackDefinition data)
		{
			return data.Type;
		}

		// 06001f4a (1.10.1 RVA 0x55A200)
		protected override IEqualityComparer<RewardTrackType> GetKeyComparer()
		{
			return HardlightProject.HardlightEnumComparers.RewardTrackTypeComparer;
		}

		// 06001f4b (1.10.1 RVA 0x55A260)
		public RewardTrackDefinitionGroup()
		{
			
		}
	}
}
