using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020002df; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterAbility_MovementJumpOneShot : CharacterAbility_MovementJumpBase<CharacterAbilityDefinition_JumpOneShot>, IAbilityOneShot
	{
		// 0600117c (1.10.1 RVA 0x74A390)
		protected override void DoOnUpdate(CharacterBrain brain, float deltaTime)
		{
			base.DoOnUpdate(brain, deltaTime);
			if (Jump)
			{
				return;
			}
			if (!ActorAbilityUtilities.TryGetAbilityInUse(m_character, out ActorAbilityType inUse, out float timeInUse)
				|| !ActorAbilityUtilities.TryGetAbilityLastUsed(m_character, out ActorAbilityType lastUsed, out float _))
			{
				return;
			}
			// A jump can chain out of the current ability only inside the trigger window.
			bool canTrigger = false;
			if (timeInUse < Definition.TriggerTimeAfter && Definition.CanTrigger(lastUsed, inUse))
			{
				canTrigger = !brain.Jump || m_character.GetTotalFixedTime() - brain.GetActionTimestamp(GameAction.CharacterJump) < Definition.TriggerTimeAfter;
			}
			m_character.Storage.SetValue(ActorFSMKeys.OneShotCanTriggerJump, canTrigger);
		}

		// 0600117d (1.10.1 RVA 0x74A690)
		protected override void DoOnLeave()
		{
			base.DoOnLeave();
			m_character.Storage.RemoveValue<bool>(ActorFSMKeys.OneShotCanTriggerJump);
		}

		// 0600117e (1.10.1 RVA 0x74A7B0)
		public bool CanTriggerBefore(float timeElapsed)
		{
			return timeElapsed < Definition.TriggerTimeBefore;
		}

		// 0600117f (1.10.1 RVA 0x74A630)
		public bool CanTriggerAfter(float timeElapsed)
		{
			return timeElapsed < Definition.TriggerTimeAfter;
		}

		// 06001180 (1.10.1 RVA 0x74A810)
		protected override bool CanTrigger()
		{
			return m_character.TryGetValue(ActorFSMKeys.OneShotCanTriggerJump);
		}

		// 06001181 (1.10.1 RVA 0x74A880)
		public CharacterAbility_MovementJumpOneShot()
		{
		}
	}
}
