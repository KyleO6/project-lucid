using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020003d8; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeDefaultName("ControlsEnabled")]
	[GraphNodeMenuFormat("Character/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterTransition_GameplayControlsEnabled : FSMTransition
	{
		// 06001725 (1.10.1 RVA 0x790580)
		private CharacterTransition_GameplayControlsEnabled(FiniteStateMachine fsm, FSMIdentifier transitionId) : base(fsm, transitionId)
		{
		}

		// 06001726 (1.10.1 RVA 0x790590)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new CharacterTransition_GameplayControlsEnabled(fsm, transitionId);
		}

		// 06001727 (1.10.1 RVA 0x7905F0)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext updateContext)
		{
			return base.DoUpdate(user, updateContext) || user.GetAs<Character>().AreControlsEnabled();
		}
	}
}
