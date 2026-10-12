using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020003dd; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeDefaultName("ToHalfPipeTransfer")]
	[GraphNodeMenuFormat("Character/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterTransition_HalfPipeTransfer : FSMTransition
	{
		// 06001732 (1.10.1 RVA 0x790F80)
		private CharacterTransition_HalfPipeTransfer(FiniteStateMachine fsm, FSMIdentifier transitionId) : base(fsm, transitionId)
		{
		}

		// 06001733 (1.10.1 RVA 0x790F90)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new CharacterTransition_HalfPipeTransfer(fsm, transitionId);
		}

		// 06001734 (1.10.1 RVA 0x790FF0)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext updateContext)
		{
			if (base.DoUpdate(user, updateContext)) return true;
			Character character = user.GetAs<Character>();
			return !character.ActiveTracking.IsOnGround() && !character.IsDirectionOverrideActive() && character.Storage.GetValue(ActorFSMKeys.HalfPipeTransferActive, false, true);
		}
	}
}
