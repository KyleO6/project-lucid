using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000455; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "ActorAbilityDefinitionGroup", menuName = "HardlightProject/DefinitionData/Groups/ActorAbilityDefinitionGroup")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ActorAbilityDefinitionGroup : DefinitionDataType<string, AbilityDefinition>
	{
		// 0600197d (1.10.1 RVA 0x7A7110)
		protected override string GetElementKey(AbilityDefinition data)
		{
			return data.name;
		}

		// 0600197e (1.10.1 RVA 0x7A7120)
		protected override IEqualityComparer<string> GetKeyComparer()
		{
			return null;
		}

		// 0600197f (1.10.1 RVA 0x7A7130)
		public ActorAbilityDefinitionGroup()
		{
			
		}
	}
}
