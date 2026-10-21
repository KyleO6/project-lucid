using Unity.IL2CPP.CompilerServices;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 0200068b; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[GraphNodeDefaultName("TriggerCharacterCamera")]
	[GraphNodeMenuFormat("Generic/{0}")]
	public class FSMState_TriggerCharacterCamera : FSMState_Trigger
	{
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		[Il2CppSetOption(Option.NullChecks, false)]
		protected class JSONCtorArgs : TriggerJSONCtorArgs
		{
			public CameraType CameraTypeOverride;

			// 06002343 (1.10.1 RVA 0x5856D0)
			public JSONCtorArgs()
			{;}
		}

		private readonly CameraType m_cameraTypeOverride;

		// The system reference is a field initializer, so it is acquired before base registration.
		private readonly SystemRef<CinemachineCameraManager> m_cinemachineCameraManager = ProcessManager.GetSystemRef<CinemachineCameraManager>();

		// 0600233f (1.10.1 RVA 0x585490); the trigger base constructor is inlined natively.
		protected FSMState_TriggerCharacterCamera(FiniteStateMachine fsm, FSMIdentifier stateId, JSONCtorArgs ctorArgs) : base(fsm, stateId, ctorArgs)
		{
			m_cameraTypeOverride = ctorArgs.CameraTypeOverride;
		}

		// 06002340 (1.10.1 RVA 0x585540)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			return new FSMState_TriggerCharacterCamera(fsm, transitionId, args);
		}

		// 06002341 (1.10.1 RVA 0x5855D0)
		protected override void InvokeTriggerStart(IGraphUser user, FSMStateChangeAction action)
		{
			if (!m_cinemachineCameraManager.IsNull())
			{
				m_cinemachineCameraManager.Get().RequestCharacterCameraChange(m_cameraTypeOverride);
			}
		}

		// 06002342 (1.10.1 RVA 0x585650); ending the trigger requests no camera type.
		protected override void InvokeTriggerEnd(IGraphUser user, FSMStateChangeAction action)
		{
			if (!m_cinemachineCameraManager.IsNull())
			{
				m_cinemachineCameraManager.Get().RequestCharacterCameraChange(CameraType.None);
			}
		}
	}
}
