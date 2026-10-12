using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020003e5; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeDefaultName("HomingAttackExit")]
	[GraphNodeMenuFormat("Character/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterTransition_HomingAttackExit : FSMTransition
	{
		// 06001745 (1.10.1 RVA 0x7917D0)
		protected CharacterTransition_HomingAttackExit(FiniteStateMachine fsm, FSMIdentifier transitionId) : base(fsm, transitionId)
		{
		}

		// 06001746 (1.10.1 RVA 0x7917E0)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new CharacterTransition_HomingAttackExit(fsm, transitionId);
		}

		// 06001747 (1.10.1 RVA 0x791840)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext updateContext)
		{
			if (base.DoUpdate(user, updateContext)) return true;
			Character character = user.GetAs<Character>();
			return !(character.IsDirectionOverrideActive() ? character.HomingPool.ActiveTargetOverridesTargeting() : character.HomingPool.IsTargetActive());
		}
	}
}
