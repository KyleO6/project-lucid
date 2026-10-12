using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000591; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "ParticleEffectDefinitionGroup", menuName = "HardlightProject/DefinitionData/Groups/ParticleEffectDefinitionGroup")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ParticleEffectDefinitionGroup : DefinitionDataType<ParticleEffectType, ParticleEffectDefinition>
	{
		// 06001e94 (1.10.1 RVA 0x554C30)
		protected override ParticleEffectType GetElementKey(ParticleEffectDefinition data)
		{
			if (data == null)
			{
				return default(ParticleEffectType);
			}
			return data.PFXType;
		}

		// 06001e95 (1.10.1 RVA 0x554C90)
		protected override IEqualityComparer<ParticleEffectType> GetKeyComparer()
		{
			return HardlightProject.HardlightEnumComparers.ParticleEffectTypeComparer;
		}

		// 06001e96 (1.10.1 RVA 0x554CF0)
		public ParticleEffectDefinitionGroup()
		{
			
		}
	}
}
