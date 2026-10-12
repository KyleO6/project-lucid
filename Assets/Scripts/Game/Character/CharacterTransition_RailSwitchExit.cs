using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020003fc; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Character/{0}")]
	[GraphNodeDefaultName("RailSwitchExit")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterTransition_RailSwitchExit : FSMTransition
	{
		// 06001783 (1.10.1 RVA 0x7937D0)
		protected CharacterTransition_RailSwitchExit(FiniteStateMachine fsm, FSMIdentifier transitionId) : base(fsm, transitionId)
		{
		}

		// 06001784 (1.10.1 RVA 0x7937E0)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new CharacterTransition_RailSwitchExit(fsm, transitionId);
		}

		// 06001785 (1.10.1 RVA 0x793840)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext updateContext)
		{
			return base.DoUpdate(user, updateContext) || user.GetAs<Character>().Storage.GetValue<ISurface>(ActorFSMKeys.RailSwitchNeighbour, null, true) == null;
		}
	}
}
