using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020003c7; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Character/{0}")]
	[GraphNodeDefaultName("AirAbilityEnter")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterTransition_AirAbilityEnter : FSMTransition
	{
		// 060016fb (1.10.1 RVA 0x78EF60)
		protected CharacterTransition_AirAbilityEnter(FiniteStateMachine fsm, FSMIdentifier transitionId) : base(fsm, transitionId)
		{
			
		}

		// 060016fc (1.10.1 RVA 0x78EF70)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new CharacterTransition_AirAbilityEnter(fsm, transitionId);
		}

		// 060016fd (1.10.1 RVA 0x78EFD0)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext updateContext)
		{
			if (base.DoUpdate(user, updateContext))
				return true;
			Character character = user.GetAs<Character>();
			if (character.Collider.GroundHit.HitProjected || character.HasAnyDirectionModifier() || character.HomingPool.IsTargetActive() || character.Storage.GetValue(ActorFSMKeys.JumpOnRailTracker, false, true))
				return false;
			CharacterAbility_Air ability = character.FindAbility<CharacterAbility_Air>(air => air.AirActive);
			character.Storage.SetValue(ActorFSMKeys.AirAbilityActive, ability);
			return ability != null;
		}
	}
}
