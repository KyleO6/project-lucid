using Unity.IL2CPP.CompilerServices;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000063; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeDefaultName("ResetAllAnimParameters")]
	[GraphNodeMenuFormat("Actor/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ActorState_ResetAllAnimParameters : FSMState_Trigger
	{
		// 06000435 (1.10.1 RVA 0x527E60)
		protected ActorState_ResetAllAnimParameters(FiniteStateMachine fsm, FSMIdentifier stateId, TriggerJSONCtorArgs ctorArgs) : base(fsm, stateId, ctorArgs)
		{
			
		}

		// 06000436 (1.10.1 RVA 0x527E70)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new ActorState_ResetAllAnimParameters(fsm, transitionId, JsonUtility.FromJson<TriggerJSONCtorArgs>(jsonCtorArgs));
		}

		// 06000437 (1.10.1 RVA 0x527F00)
		protected override void InvokeTriggerStart(IGraphUser user, FSMStateChangeAction action)
		{
			user.GetAs<Actor>().Animator.ResetAllParameters();
		}
	}
}
