using Unity.IL2CPP.CompilerServices;
using System.Text;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020002d7; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterAbility_MovementClimbingBoost : CharacterAbility_Climbing<CharacterAbilityDefinition_MovementClimbingBoost>, IAbilityTrigger
	{
		public bool IsTriggered
		{
			get
			{
				return m_character.BoostStamina.IsTriggered;
			}
		}

		// 06001149 (1.10.1 RVA 0x748EC0)
		protected override void DoOnUpdate(CharacterBrain brain, float deltaTime)
		{
			base.DoOnUpdate(brain, deltaTime);
			m_character.BoostStamina.UpdateFromAbility(Definition.AbilityType, Definition.BoostDefinition);
		}

		// 0600114a (1.10.1 RVA 0x748F80)
		protected override void DoOnLeave()
		{
			base.DoOnLeave();
			m_character.BoostStamina.Clear();
		}

		// 0600114b (1.10.1 RVA 0x748FB0)
		public override bool GetUIText(StringBuilder stringInfoBuilder)
		{
			base.GetUIText(stringInfoBuilder);
			return m_character.BoostStamina.GetUIText(stringInfoBuilder);
		}

		// 0600114c (1.10.1 RVA 0x748FE0)
		protected override void GetDebugExtraInfo(StringBuilder extraInfo)
		{
			base.GetDebugExtraInfo(extraInfo);
			m_character.BoostStamina.GetDebugExtraInfo(extraInfo);
		}

		// 0600114d (1.10.1 RVA 0x749010)
		public CharacterAbility_MovementClimbingBoost()
		{
		}
	}
}
