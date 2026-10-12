using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020003fd; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeDefaultName("RespawnComplete")]
	[GraphNodeMenuFormat("Character/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterTransition_RespawnComplete : FSMTransition
	{
		// 06001786 (1.10.1 RVA 0x793980)
		protected CharacterTransition_RespawnComplete(FiniteStateMachine fsm, FSMIdentifier transitionId) : base(fsm, transitionId)
		{
		}

		// 06001787 (1.10.1 RVA 0x793990)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new CharacterTransition_RespawnComplete(fsm, transitionId);
		}

		// 06001788 (1.10.1 RVA 0x7939F0)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext updateContext)
		{
			return base.DoUpdate(user, updateContext) || user.Storage.GetValue(ActorFSMKeys.RespawnEffectsComplete, false, true);
		}

		// 06001789 (1.10.1 RVA 0x793B90)
		protected override void DoOnLeave(IGraphUser user, FSMStateChangeAction action)
		{
			user.Storage.RemoveValue<bool>(ActorFSMKeys.RespawnEffectsComplete);
			user.Storage.SetValue(ActorFSMKeys.IsOutOfBounds, false);
			user.Storage.SetValue(ActorFSMKeys.InHardFailureState, false);
			base.DoOnLeave(user, action);
		}
	}
}
