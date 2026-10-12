using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020003fa; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeDefaultName("RailExit")]
	[GraphNodeMenuFormat("Character/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterTransition_RailExit : FSMTransition
	{
		// 0600177d (1.10.1 RVA 0x793420)
		protected CharacterTransition_RailExit(FiniteStateMachine fsm, FSMIdentifier transitionId) : base(fsm, transitionId)
		{
		}

		// 0600177e (1.10.1 RVA 0x793430)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new CharacterTransition_RailExit(fsm, transitionId);
		}

		// 0600177f (1.10.1 RVA 0x793490)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext updateContext)
		{
			if (base.DoUpdate(user, updateContext)) return true;
			Character character = user.GetAs<Character>();
			return character.Storage.GetValue(ActorFSMKeys.RailTargetCancelled, false, true) || !character.Storage.GetValue(ActorFSMKeys.RailActive, false, true);
		}
	}
}
