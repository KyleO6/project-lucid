using Unity.IL2CPP.CompilerServices;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 0200006f; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeDefaultName("ZeroVelocity")]
	[GraphNodeMenuFormat("Actor/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ActorState_ZeroVelocity : ActorState_Effect
	{
		// 06000453 (1.10.1 RVA 0x5294F0)
		protected ActorState_ZeroVelocity(FiniteStateMachine fsm, FSMIdentifier stateId, BaseJSONCtorArgs ctorArgs) : base(fsm, stateId, ctorArgs)
		{;}

		// 06000454 (1.10.1 RVA 0x529500)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new ActorState_ZeroVelocity(fsm, transitionId, JsonUtility.FromJson<BaseJSONCtorArgs>(jsonCtorArgs));
		}

		// 06000455 (1.10.1 RVA 0x529590)
		protected override void DoUpdate(IGraphUser user, FSMUpdateContext updateContext)
		{
			base.DoUpdate(user, updateContext);
			GetActor(user).SetLocalVelocity(Vector3.zero);
		}
	}
}
