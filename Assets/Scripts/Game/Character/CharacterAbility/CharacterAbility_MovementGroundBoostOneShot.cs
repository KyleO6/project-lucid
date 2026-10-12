using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020002db; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterAbility_MovementGroundBoostOneShot : CharacterAbility_MovementGroundBoostBase<CharacterAbilityDefinition_MovementGroundBoostOneShot>, IAbilityOneShot
	{
		// 0600115c (1.10.1 RVA 0x7491B0)
		protected override void DoOnUpdate(CharacterBrain brain, float deltaTime)
		{
			base.DoOnUpdate(brain, deltaTime);
			if (!ActorAbilityUtilities.TryGetAbilityInUse(m_character, out ActorAbilityType abilityInUse, out float timeSinceInUse))
			{
				return;
			}
			if (!ActorAbilityUtilities.TryGetAbilityLastUsed(m_character, out ActorAbilityType abilityLastUsed, out float _))
			{
				return;
			}
			bool canTrigger = false;
			if (!IsTriggered && timeSinceInUse < Definition.TriggerTimeAfter && Definition.CanTrigger(abilityLastUsed, abilityInUse))
			{
				canTrigger = true;
				if (brain.Boost)
				{
					canTrigger = m_character.GetTotalFixedTime() - brain.GetActionTimestamp(GameAction.CharacterBoost) < Definition.TriggerTimeAfter;
				}
			}
			m_character.Storage.SetValue(ActorFSMKeys.OneShotCanTriggerBoost, canTrigger);
		}

		// 0600115d (1.10.1 RVA 0x7494B0)
		protected override void DoOnLeave()
		{
			base.DoOnLeave();
			m_character.Storage.RemoveValue<bool>(ActorFSMKeys.OneShotCanTriggerBoost);
		}

		// 0600115e (1.10.1 RVA 0x7495D0)
		public bool CanTriggerBefore(float timeElapsed)
		{
			return timeElapsed < Definition.TriggerTimeBefore;
		}

		// 0600115f (1.10.1 RVA 0x749450)
		public bool CanTriggerAfter(float timeElapsed)
		{
			return timeElapsed < Definition.TriggerTimeAfter;
		}

		// 06001160 (1.10.1 RVA 0x749630)
		protected override bool CanTrigger()
		{
			return m_character.TryGetValue(ActorFSMKeys.OneShotCanTriggerBoost);
		}

		// 06001161 (1.10.1 RVA 0x7496A0)
		public CharacterAbility_MovementGroundBoostOneShot()
		{
		}
	}
}
