using Unity.IL2CPP.CompilerServices;
using System;
using System.Collections.Generic;

namespace HardlightProject
{
	// Original Game.Runtime 02000501; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public abstract class CharacterAbilityDefinition_Modifier : AbilityDefinition
	{
		public List<ActorAbilityType> ModifyAbilities = new List<ActorAbilityType>();

		// 06001b93 (1.10.1 RVA 0x545330)
		protected CharacterAbilityDefinition_Modifier()
		{
		}
	}
}
