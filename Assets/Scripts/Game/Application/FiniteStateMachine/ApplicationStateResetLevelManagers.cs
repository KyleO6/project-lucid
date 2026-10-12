using Unity.IL2CPP.CompilerServices;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 0200011d; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateResetLevelManagers : FSMState_Trigger
	{
		private readonly SystemRef<LevelManager> m_levelManagerRef = ProcessManager.GetSystemRef<LevelManager>();

		private readonly SystemRef<MessageManager> m_messageManagerRef = ProcessManager.GetSystemRef<MessageManager>();

		// 0600074a (1.10.1 RVA 0x6538C0)
		private ApplicationStateResetLevelManagers(FiniteStateMachine fsm, string stateName, TriggerJSONCtorArgs ctorArgs) : base(fsm, stateName, ctorArgs)
		{


		}

		// 0600074b (1.10.1 RVA 0x6539A0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			TriggerJSONCtorArgs args = JsonUtility.FromJson<TriggerJSONCtorArgs>(jsonCtorArgs);
			return new ApplicationStateResetLevelManagers(fsm, (string)stateId, args);
		}

		// 0600074c (1.10.1 RVA 0x653A40)
		protected override void InvokeTriggerStart(IGraphUser user, FSMStateChangeAction action)
		{
			m_levelManagerRef.Get().ManagedSystems.AddManagedSystem<EntityActivationManager>(true);
			m_messageManagerRef.Get().ProcessSystemAction(SystemAction.Shutdown);
		}
	}
}
