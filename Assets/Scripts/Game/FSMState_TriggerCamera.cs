using Unity.IL2CPP.CompilerServices;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000689; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[GraphNodeDefaultName("TriggerCamera")]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[GraphNodeMenuFormat("Generic/{0}")]
	public class FSMState_TriggerCamera : FSMState_Trigger
	{
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class JSONCtorArgs : TriggerJSONCtorArgs
		{
			public CameraType CameraTypeOverride;

			// 0600233e (1.10.1 RVA 0x585480)
			public JSONCtorArgs()
			{;}
		}

		private readonly CameraType m_cameraTypeOverride;

		// 0600233b (1.10.1 RVA 0x584E40); the trigger base constructor is inlined natively.
		protected FSMState_TriggerCamera(FiniteStateMachine fsm, FSMIdentifier stateId, JSONCtorArgs ctorArgs) : base(fsm, stateId, ctorArgs)
		{
			m_cameraTypeOverride = ctorArgs.CameraTypeOverride;
		}

		// 0600233c (1.10.1 RVA 0x584E70)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			return new FSMState_TriggerCamera(fsm, transitionId, args);
		}

		// 0600233d (1.10.1 RVA 0x584F10). An existing override handle is reapplied or
		// released; otherwise a new handle is taken only for a real camera type. A
		// reapplied handle is stored twice, as the native body does.
		protected override void InvokeTriggerStart(IGraphUser user, FSMStateChangeAction action)
		{
			SystemRef<CinemachineCameraManager> cameraManagerRef = ProcessManager.GetSystemRef<CinemachineCameraManager>();
			if (cameraManagerRef.IsNull())
			{
				return;
			}
			CinemachineCameraManager cameraManager = cameraManagerRef.Get();
			if (user.Storage.TryGetValue(FSMStorageKeys.CameraOverride, out StackableDataHandle handle))
			{
				if (m_cameraTypeOverride == CameraType.None)
				{
					cameraManager.RemoveCameraSettingOverride(handle);
					user.Storage.RemoveValue<StackableDataHandle>(FSMStorageKeys.CameraOverride);
					return;
				}
				cameraManager.ApplyCameraTypeOverride(m_cameraTypeOverride, handle);
				user.Storage.SetValue(FSMStorageKeys.CameraOverride, handle);
			}
			else
			{
				if (m_cameraTypeOverride == CameraType.None)
				{
					return;
				}
				handle = cameraManager.ApplyCameraTypeOverride(m_cameraTypeOverride);
			}
			user.Storage.SetValue(FSMStorageKeys.CameraOverride, handle);
		}
	}
}
