using Unity.IL2CPP.CompilerServices;
using System;
using System.Collections.Generic;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020002e3; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterAbility_MovementRailBoostOneShot : CharacterAbility_MovementRailBoostBase<CharacterAbilityDefinition_MovementRailBoostOneShot>, IAbilityOneShot
	{
		// 060011a1 (1.10.1 RVA 0x74A970)
		protected override void DoOnUpdate(CharacterBrain brain, float deltaTime)
		{
			base.DoOnUpdate(brain, deltaTime);
			if (!ActorAbilityUtilities.TryGetAbilityInUse(m_character, out ActorAbilityType abilityInUse, out float timeSinceInUse))
				return;
			if (!ActorAbilityUtilities.TryGetAbilityLastUsed(m_character, out ActorAbilityType abilityLastUsed, out float _))
				return;
			bool canTrigger = false;
			if (!IsTriggered && Definition.TriggerAbilitiesBefore.Contains(abilityLastUsed) && Definition.TriggerAbilities.Contains(abilityInUse) && timeSinceInUse < Definition.TriggerTimeAfter)
			{
				canTrigger = m_character.GetTotalFixedTime() - brain.GetActionTimestamp(GameAction.CharacterBoost) < timeSinceInUse;
			}
			m_character.Storage.SetValue(ActorFSMKeys.OneShotCanTriggerBoost, canTrigger);
		}

		// 060011a2 (1.10.1 RVA 0x74AC60)
		protected override void DoOnLeave()
		{
			base.DoOnLeave();
			m_character.Storage.RemoveValue<bool>(ActorFSMKeys.OneShotCanTriggerBoost);
		}

		// 060011a3 (1.10.1 RVA 0x74AD80)
		public bool CanTriggerBefore(float timeElapsed)
		{
			return timeElapsed < Definition.TriggerTimeBefore;
		}

		// 060011a4 (1.10.1 RVA 0x74AC00)
		public bool CanTriggerAfter(float timeElapsed)
		{
			return timeElapsed < Definition.TriggerTimeAfter;
		}

		// 060011a5 (1.10.1 RVA 0x74ADE0)
		protected override bool CanTrigger()
		{
			return m_character.TryGetValue(ActorFSMKeys.OneShotCanTriggerBoost);
		}

		// 060011a6 (1.10.1 RVA 0x74AE50)
		public CharacterAbility_MovementRailBoostOneShot()
		{
			
		}
	}
}
