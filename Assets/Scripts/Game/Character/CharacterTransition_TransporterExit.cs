using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 02000408; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Character/{0}")]
	[GraphNodeDefaultName("TransporterExit")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterTransition_TransporterExit : FSMTransition
	{
		// 060017a5 (1.10.1 RVA 0x794A20)
		protected CharacterTransition_TransporterExit(FiniteStateMachine fsm, FSMIdentifier transitionId) : base(fsm, transitionId)
		{
			
		}

		// 060017a6 (1.10.1 RVA 0x794A30)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new CharacterTransition_TransporterExit(fsm, transitionId);
		}

		// 060017a7 (1.10.1 RVA 0x794A90)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext updateContext)
		{
			return base.DoUpdate(user, updateContext) || !user.GetAs<Character>().Storage.GetValue(ActorFSMKeys.TransporterActive, false, true);
		}
	}
}
