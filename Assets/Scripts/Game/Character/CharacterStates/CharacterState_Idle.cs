using Unity.IL2CPP.CompilerServices;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 0200037b; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeDefaultName("Idle")]
	[GraphNodeMenuFormat("Character/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterState_Idle : CharacterState_Ability<CharacterAbility_Idle>
	{
		// 060015a8 (1.10.1 RVA 0x77A7C0)
		protected CharacterState_Idle(FiniteStateMachine fsm, FSMIdentifier stateId, AbilityJSONCtorArgs ctorArgs) : base(fsm, stateId, ctorArgs)
		{
			
		}

		// 060015a9 (1.10.1 RVA 0x77A830)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new CharacterState_Idle(fsm, transitionId, JsonUtility.FromJson<AbilityJSONCtorArgs>(jsonCtorArgs));
		}

		// 060015aa (1.10.1 RVA 0x77A8F0)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			Character character = user.GetAs<Character>();
			character.Storage.SetValue(ActorFSMKeys.IsIdle, true);
			character.HomingPool.ClearTarget();
		}

		// 060015ab (1.10.1 RVA 0x77AA50)
		protected override void DoUpdate(IGraphUser user, FSMUpdateContext updateContext)
		{
			base.DoUpdate(user, updateContext);
			Character character = user.GetAs<Character>();
			character.SetWorldVelocity(Vector3.zero);
			character.ActiveTracking.OrientateToGround();
		}

		// 060015ac (1.10.1 RVA 0x77AB30)
		protected override void DoOnLeave(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnLeave(user, action);
			user.GetAs<Character>().Storage.SetValue(ActorFSMKeys.IsIdle, false);
		}
	}
}
