using Unity.IL2CPP.CompilerServices;

namespace HardlightProject
{
	// Original Game.Runtime 020004f2; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public abstract class CharacterAbilityDefinition_MovementRailBoostBase : CharacterAbilityDefinition_MovementRail
	{
		public CharacterStamina.Definition BoostDefinition;

		// 06001b66 (1.10.1 RVA 0x544840)
		public override bool TryGetBoostDefinition(out CharacterStamina.Definition boostDefinition)
		{
			boostDefinition = BoostDefinition;
			return true;
		}

		// 06001b67 (1.10.1 RVA 0x5447F0)
		protected CharacterAbilityDefinition_MovementRailBoostBase()
		{
		}
	}
}
