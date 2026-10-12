using Unity.IL2CPP.CompilerServices;
using System.Text;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020002e2; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public abstract class CharacterAbility_MovementRailBoostBase<T> : CharacterAbility_MovementRail<T> where T : CharacterAbilityDefinition_MovementRailBoostBase
	{
		private bool m_active;

		private bool m_wasActive;

		private bool m_queuedBoost;

		private float m_timeActivated;

		public float TimeElapsed
		{
			get
			{
				return m_character.GetTotalFixedTime() - m_timeActivated;
			}
		}

		protected abstract bool CanTrigger();

		// 0600119a (1.10.1 RVA 0xE64F90)
		public void QueueBoost()
		{
			m_queuedBoost = !IsTriggered;
		}

		// 0600119b (1.10.1 RVA 0xE64FB0)
		public override void SetActive(bool active)
		{
			m_wasActive = m_active;
			m_active = active;
		}

		// 0600119c (1.10.1 RVA 0xE64FD0)
		protected override void DoOnUpdate(CharacterBrain brain, float deltaTime)
		{
			base.DoOnUpdate(brain, deltaTime);
			if (!IsTriggered && brain.Boost && CanTrigger() && !m_character.BoostStamina.UpdatedFromAbility)
				QueueBoost();
			if (m_queuedBoost)
			{
				m_queuedBoost = false;
				if (!m_character.BoostStamina.UpdatedFromAbility)
					m_character.BoostStamina.UpdateFromAbility(Definition.AbilityType, Definition.BoostDefinition);
				IsTriggered = m_character.BoostStamina.CanTrigger();
				if (IsTriggered)
				{
					m_timeActivated = brain.GetActionTimestamp(GameAction.CharacterBoost);
					m_character.BoostStamina.ActivationQueued = true;
					m_character.Storage.SetValue(ActorFSMKeys.StateRemainTimer, Definition.StateTimeMin);
				}
			}
			else if (IsTriggered)
			{
				IsTriggered = m_character.BoostStamina.IsTriggered;
				m_character.BoostStamina.UpdateFromAbility(Definition.AbilityType, Definition.BoostDefinition);
			}
			m_wasActive = m_wasActive && m_character.BoostStamina.Stamina < 1f;
		}

		// 0600119d (1.10.1 RVA 0xE65400)
		protected override void DoOnLeave()
		{
			base.DoOnLeave();
			m_character.BoostStamina.Clear();
		}

		// 0600119e (1.10.1 RVA 0xE65430)
		public override bool GetUIText(StringBuilder stringInfoBuilder)
		{
			base.GetUIText(stringInfoBuilder);
			if (m_active || m_wasActive)
			{
				return m_character.BoostStamina.GetUIText(stringInfoBuilder);
			}
			return false;
		}

		// 0600119f (1.10.1 RVA 0xE65480)
		protected override void GetDebugExtraInfo(StringBuilder extraInfo)
		{
			base.GetDebugExtraInfo(extraInfo);
			m_character.BoostStamina.GetDebugExtraInfo(extraInfo);
		}

		// 060011a0 (1.10.1 RVA 0xE654B0)
		protected CharacterAbility_MovementRailBoostBase()
		{
		}
	}
}
