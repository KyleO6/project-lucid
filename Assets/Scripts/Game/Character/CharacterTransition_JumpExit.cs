using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020003ed; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeDefaultName("JumpExit")]
	[GraphNodeMenuFormat("Character/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterTransition_JumpExit : FSMTransition
	{
		// 0600175c (1.10.1 RVA 0x792160)
		protected CharacterTransition_JumpExit(FiniteStateMachine fsm, FSMIdentifier transitionId) : base(fsm, transitionId)
		{
		}

		// 0600175d (1.10.1 RVA 0x792170)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new CharacterTransition_JumpExit(fsm, transitionId);
		}

		// 0600175e (1.10.1 RVA 0x7921D0)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext updateContext)
		{
			if (base.DoUpdate(user, updateContext)) return true;
			Character character = user.GetAs<Character>();
			if (character.Storage.GetValue(ActorFSMKeys.JumpingExitedGround, false, true) && character.ActiveTracking.IsOnGround()) return true;
			return !character.Storage.GetValue(ActorFSMKeys.IsJumping, false, true);
		}
	}
}
