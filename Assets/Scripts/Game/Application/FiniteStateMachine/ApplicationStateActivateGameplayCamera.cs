using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000153; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/Systems/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateActivateGameplayCamera : FSMState
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class JSONCtorArgs
		{
			public bool ActivateCamera;

			// 060007fd (1.10.1 RVA 0x65FD70)
			public JSONCtorArgs()
			{;}
		}

		private readonly bool m_activateCamera;

		private readonly SystemRef<CinemachineCameraManager> m_cinemachineCameraManagerRef = ProcessManager.GetSystemRef<CinemachineCameraManager>();

		// 060007fa (1.10.1 RVA 0x65FB40)
		private ApplicationStateActivateGameplayCamera(FiniteStateMachine fsm, FSMIdentifier stateId, bool activateCamera) : base(fsm, stateId)
		{
			m_activateCamera = activateCamera;
		}

		// 060007fb (1.10.1 RVA 0x65FBF0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			return new ApplicationStateActivateGameplayCamera(fsm, transitionId, args.ActivateCamera);
		}

		// 060007fc (1.10.1 RVA 0x65FCF0)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			if (m_cinemachineCameraManagerRef.IsNull() && !m_activateCamera)
			{
				return;
			}
			m_cinemachineCameraManagerRef.Get().ToggleGameplayCameraActive(m_activateCamera);
		}
	}
}
