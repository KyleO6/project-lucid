using Unity.IL2CPP.CompilerServices;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000395; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeDefaultName("ClearCameraProxySettings")]
	[GraphNodeMenuFormat("Character/Trigger/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterState_ClearCameraProxySettings : FSMState_Trigger
	{
		// 0600160f (1.10.1 RVA 0x781D90)
		protected CharacterState_ClearCameraProxySettings(FiniteStateMachine fsm, FSMIdentifier stateId, TriggerJSONCtorArgs ctorArgs) : base(fsm, stateId, ctorArgs)
		{
			
		}

		// 06001610 (1.10.1 RVA 0x781DA0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new CharacterState_ClearCameraProxySettings(fsm, transitionId, JsonUtility.FromJson<TriggerJSONCtorArgs>(jsonCtorArgs));
		}

		// 06001611 (1.10.1 RVA 0x781E30)
		protected override void InvokeTriggerStart(IGraphUser user, FSMStateChangeAction action)
		{
			CameraUtilities.ClearCharacterCameraProxyTargetSettings(user.GetAs<Character>());
		}
	}
}
