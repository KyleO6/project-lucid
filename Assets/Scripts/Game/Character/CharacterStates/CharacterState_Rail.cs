using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 02000388; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Character/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public abstract class CharacterState_Rail<TAbility> : CharacterState_Ability<TAbility> where TAbility : CharacterAbility
	{
		// 060015d3 (1.10.1 RVA 0xE78910)
		protected CharacterState_Rail(FiniteStateMachine fsm, FSMIdentifier stateId, AbilityJSONCtorArgs ctorArgs) : base(fsm, stateId, ctorArgs)
		{
			
		}

		// 060015d4 (1.10.1 RVA 0xE78930)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			user.GetAs<Character>().Storage.SetValue(ActorFSMKeys.RailActive, true);
		}

		// 060015d5 (1.10.1 RVA 0xE78A80)
		protected override void DoOnLeave(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnLeave(user, action);
			Character character = user.GetAs<Character>();
			character.Storage.SetValue(ActorFSMKeys.RailActive, false);
			character.Storage.SetValue(ActorFSMKeys.RailActiveLastTime, character.GetTotalFixedTime());
		}
	}
}
