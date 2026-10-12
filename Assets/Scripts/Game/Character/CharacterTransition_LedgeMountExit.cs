using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020003f5; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Character/{0}")]
	[GraphNodeDefaultName("LedgeMountExit")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterTransition_LedgeMountExit : FSMTransition
	{
		// 0600176d (1.10.1 RVA 0x792B20)
		protected CharacterTransition_LedgeMountExit(FiniteStateMachine fsm, FSMIdentifier transitionId) : base(fsm, transitionId)
		{
		}

		// 0600176e (1.10.1 RVA 0x792B30)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new CharacterTransition_LedgeMountExit(fsm, transitionId);
		}

		// 0600176f (1.10.1 RVA 0x792B90)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext updateContext)
		{
			return base.DoUpdate(user, updateContext) || user.Storage.GetValue(ActorFSMKeys.LedgeMountFinished, false, true);
		}
	}
}
