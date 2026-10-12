using Unity.IL2CPP.CompilerServices;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020003cf; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Character/{0}")]
	[GraphNodeDefaultName("ClimbExitDirectionModifier")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterTransition_ClimbExitDirectionModifier : FSMTransition
	{
		// 0600170f (1.10.1 RVA 0x78F9F0)
		protected CharacterTransition_ClimbExitDirectionModifier(FiniteStateMachine fsm, FSMIdentifier transitionId) : base(fsm, transitionId)
		{
		}

		// 06001710 (1.10.1 RVA 0x78FA00)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new CharacterTransition_ClimbExitDirectionModifier(fsm, transitionId);
		}

		// 06001711 (1.10.1 RVA 0x78FA60)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext updateContext)
		{
			if (base.DoUpdate(user, updateContext))
			{
				return true;
			}
			return user.GetAs<Character>().IsDirectionOverrideActive();
		}

		// 06001712 (1.10.1 RVA 0x78FAE0)
		protected override void DoOnLeave(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnLeave(user, action);
			if (action != null && action.TransitionThatFired == this)
			{
				Character character = user.GetAs<Character>();
				character.TryGetDirectionOverride(out Vector3 lookDirection);
				character.OrientateToPlaneWithLookDirection(character.UpDirection, lookDirection);
				character.Storage.SetValue(ActorFSMKeys.ClimbingEnterWhenDirectionOverrideEnds, true);
			}
		}
	}
}
