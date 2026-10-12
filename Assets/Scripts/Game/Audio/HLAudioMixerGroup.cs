using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 0200047e; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "HLAudioMixerGroup", menuName = "HardlightProject/DefinitionData/Groups/HLAudioMixerGroup")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class HLAudioMixerGroup : DefinitionDataType<HLAudioMixerIdentifier, HLAudioMixerDefinition>
	{
		// 06001a11 (1.10.1 RVA 0x53E540)
		protected override HLAudioMixerIdentifier GetElementKey(HLAudioMixerDefinition data)
		{
			return data.Identifier;
		}

		// 06001a12 (1.10.1 RVA 0x53E550)
		protected override IEqualityComparer<HLAudioMixerIdentifier> GetKeyComparer()
		{
			return HardlightProject.HardlightEnumComparers.HLAudioMixerIdentifierComparer;
		}

		// 06001a13 (1.10.1 RVA 0x53E5B0)
		public HLAudioMixerGroup()
		{
			
		}
	}
}
