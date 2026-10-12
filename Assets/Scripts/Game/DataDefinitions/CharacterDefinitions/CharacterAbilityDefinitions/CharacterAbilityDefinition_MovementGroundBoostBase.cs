using Unity.IL2CPP.CompilerServices;

namespace HardlightProject
{
	// Original Game.Runtime 020004ed; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public abstract class CharacterAbilityDefinition_MovementGroundBoostBase : CharacterAbilityDefinition_MovementGround
	{
		public CharacterStamina.Definition BoostDefinition;

		// 06001b56 (1.10.1 RVA 0x544350)
		public override bool TryGetBoostDefinition(out CharacterStamina.Definition boostDefinition)
		{
			boostDefinition = BoostDefinition;
			return true;
		}

		// 06001b57 (1.10.1 RVA 0x544370)
		public override void CalculateCachedValues()
		{
			base.CalculateCachedValues();
			BoostDefinition.CalculateCachedValues();
		}

		// 06001b58 (1.10.1 RVA 0x544320)
		protected CharacterAbilityDefinition_MovementGroundBoostBase()
		{
		}
	}
}
