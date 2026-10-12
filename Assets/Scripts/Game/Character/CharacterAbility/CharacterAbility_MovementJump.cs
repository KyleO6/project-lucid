using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020002dd; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterAbility_MovementJump : CharacterAbility_MovementJumpBase<CharacterAbilityDefinition_Jump>
	{
		// 06001170 (1.10.1 RVA 0x74A2E0)
		protected override bool CanTrigger()
		{
			return !m_character.TryGetValue(ActorFSMKeys.OneShotCanTriggerJump);
		}

		// 06001171 (1.10.1 RVA 0x74A350)
		public CharacterAbility_MovementJump()
		{
		}
	}
}
