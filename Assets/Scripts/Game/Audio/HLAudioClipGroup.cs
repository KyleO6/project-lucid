using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 0200047c; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "HLAudioClipGroup", menuName = "HardlightProject/DefinitionData/Groups/HLAudioClipGroup")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class HLAudioClipGroup : DefinitionDataType<HLAudioClipIdentifier, HLAudioClipDefinition>
	{
		// 060019ff (1.10.1 RVA 0x53DD10)
		protected override HLAudioClipIdentifier GetElementKey(HLAudioClipDefinition data)
		{
			return data.Identifier;
		}

		// 06001a00 (1.10.1 RVA 0x53DD20)
		protected override IEqualityComparer<HLAudioClipIdentifier> GetKeyComparer()
		{
			return HardlightProject.HardlightEnumComparers.HLAudioClipIdentifierComparer;
		}

		// 06001a01 (1.10.1 RVA 0x53DD80)
		public HLAudioClipGroup()
		{
			
		}
	}
}
