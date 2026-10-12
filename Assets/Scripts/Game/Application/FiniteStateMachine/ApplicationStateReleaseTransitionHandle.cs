using Unity.IL2CPP.CompilerServices;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000117; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateReleaseTransitionHandle : FSMState_Trigger
	{
		// 0600073c (1.10.1 RVA 0x652DD0)
		private ApplicationStateReleaseTransitionHandle(FiniteStateMachine fsm, string stateName, TriggerJSONCtorArgs ctorArgs) : base(fsm, stateName, ctorArgs)
		{
			
		}

		// 0600073d (1.10.1 RVA 0x652E10)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			return new ApplicationStateReleaseTransitionHandle(fsm, (string)stateId, JsonUtility.FromJson<TriggerJSONCtorArgs>(jsonCtorArgs));
		}

		// 0600073e (1.10.1 RVA 0x652EC0)
		protected override void InvokeTriggerStart(IGraphUser user, FSMStateChangeAction action)
		{
			ProcessManager.GetSystem<App>().TryReleaseUIScreenTransition();
		}
	}
}
