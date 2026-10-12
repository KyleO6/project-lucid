using Unity.IL2CPP.CompilerServices;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000382; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeDefaultName("Knockback")]
	[GraphNodeMenuFormat("Character/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterState_Knockback : CharacterState_Ability<CharacterAbility_Knockback>
	{
		// 060015ba (1.10.1 RVA 0x77AF60)
		protected CharacterState_Knockback(FiniteStateMachine fsm, FSMIdentifier stateId, AbilityJSONCtorArgs ctorArgs) : base(fsm, stateId, ctorArgs)
		{
			
		}

		// 060015bb (1.10.1 RVA 0x77AFD0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new CharacterState_Knockback(fsm, transitionId, JsonUtility.FromJson<AbilityJSONCtorArgs>(jsonCtorArgs));
		}

		// 060015bc (1.10.1 RVA 0x77B090)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			Character character = user.GetAs<Character>();
			character.SetWorldRotation(Quaternion.LookRotation(-character.WorldVelocity, -character.Gravity));
			character.SetLocalVelocity(character.LocalVelocity + GetAbility(character).Definition.KnockbackAdditiveVelocity);
		}
	}
}
