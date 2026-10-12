using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000128; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateUINavigation : FSMState
	{
		private enum SystemStatus
		{
			DoNothing,
			Enable,
			Disable
		}

		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		private class JSONCtorArgs
		{
			public SystemStatus Status;

			// 0600076b (1.10.1 RVA 0x6550F0)
			public JSONCtorArgs()
			{;}
		}

		private readonly SystemStatus m_status;

		// 06000768 (1.10.1 RVA 0x654F60)
		private ApplicationStateUINavigation(FiniteStateMachine fsm, string stateName, SystemStatus status) : base(fsm, stateName)
		{
			m_status = status;
		}

		// 06000769 (1.10.1 RVA 0x654FA0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			return new ApplicationStateUINavigation(fsm, (string)stateId, args.Status);
		}

		// 0600076a (1.10.1 RVA 0x655050)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			UINavigationManager navigationManager = ProcessManager.GetSystem<UINavigationManager>();
			switch (m_status)
			{
			case SystemStatus.Enable:
				navigationManager.Enable();
				break;
			case SystemStatus.Disable:
				navigationManager.Disable();
				break;
			}
		}
	}
}
