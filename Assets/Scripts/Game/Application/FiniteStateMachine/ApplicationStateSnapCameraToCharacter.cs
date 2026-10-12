using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000122; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeDefaultName("ApplicationStateSnapCameraToCharacter")]
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateSnapCameraToCharacter : FSMState
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		private class JSONCtorArgs
		{
			public bool TriggerTurnCamera;

			// 0600075a (1.10.1 RVA 0x6546C0)
			public JSONCtorArgs()
			{;}
		}

		private bool m_triggerTurnCamera;

		// 06000757 (1.10.1 RVA 0x654550)
		public ApplicationStateSnapCameraToCharacter(FiniteStateMachine fsm, string stateName, bool triggerTurnCamera) : base(fsm, stateName)
		{
			m_triggerTurnCamera = triggerTurnCamera;
		}

		// 06000758 (1.10.1 RVA 0x654590)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			return new ApplicationStateSnapCameraToCharacter(fsm, transitionId, args.TriggerTurnCamera);
		}

		// 06000759 (1.10.1 RVA 0x654640)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			ProcessManager.GetSystem<CinemachineCameraManager>().SnapToCharacter(m_triggerTurnCamera);
		}
	}
}
