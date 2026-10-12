using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000437; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "CutsceneDefinitionGroup", menuName = "HardlightProject/DefinitionData/Groups/CutsceneDefinitionGroup")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CutsceneDefinitionGroup : DefinitionDataType<HLCutsceneIdentifier, CutsceneDefinition>
	{
		// 060018ce (1.10.1 RVA 0x79F530)
		protected override HLCutsceneIdentifier GetElementKey(CutsceneDefinition data)
		{
			return data.Identifier;
		}

		// 060018cf (1.10.1 RVA 0x79F540)
		protected override IEqualityComparer<HLCutsceneIdentifier> GetKeyComparer()
		{
			return HardlightProject.HardlightEnumComparers.HLCutsceneIdentifierComparer;
		}

		// 060018d0 (1.10.1 RVA 0x79F5A0)
		public CutsceneDefinitionGroup()
		{
			
		}
	}
}
