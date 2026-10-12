using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020003d5; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeDefaultName("ToFall")]
	[GraphNodeMenuFormat("Character/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterTransition_Fall : FSMTransition
	{
		// 0600171e (1.10.1 RVA 0x7902E0)
		protected CharacterTransition_Fall(FiniteStateMachine fsm, FSMIdentifier transitionId) : base(fsm, transitionId)
		{
			
		}

		// 0600171f (1.10.1 RVA 0x7902F0)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new CharacterTransition_Fall(fsm, transitionId);
		}

		// 06001720 (1.10.1 RVA 0x790350)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext updateContext)
		{
			if (base.DoUpdate(user, updateContext))
				return true;
			Character character = user.GetAs<Character>();
			return !character.ActiveTracking.IsOnGround() && character.Collider.GroundHitLastFrame.Hit && !character.ActiveTracking.WasOnGround();
		}
	}
}
