using Unity.IL2CPP.CompilerServices;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000692; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[GraphNodeDefaultName("TriggerScreenControls")]
	[GraphNodeMenuFormat("Generic/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	public class FSMState_TriggerScreenControls : FSMState_Trigger
	{
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class JSONCtorArgs : TriggerJSONCtorArgs
		{
			public ScreenControlsGroup ScreenControlsGroupOverride;

			// 06002352 (1.10.1 RVA 0x586470)
			public JSONCtorArgs()
			{;}
		}

		private readonly ScreenControlsGroup m_screenControlsGroupOverride;

		// 0600234e (1.10.1 RVA 0x586270); the trigger base constructor is inlined natively.
		protected FSMState_TriggerScreenControls(FiniteStateMachine fsm, FSMIdentifier stateId, JSONCtorArgs ctorArgs) : base(fsm, stateId, ctorArgs)
		{
			m_screenControlsGroupOverride = ctorArgs.ScreenControlsGroupOverride;
		}

		// 0600234f (1.10.1 RVA 0x5862A0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			return new FSMState_TriggerScreenControls(fsm, transitionId, args);
		}

		// 06002350 (1.10.1 RVA 0x586340); the request is sent from the user's Character
		// and skipped when no MessageManager is registered.
		protected override void InvokeTriggerStart(IGraphUser user, FSMStateChangeAction action)
		{
			Component character = user.GetAs<Character>();
			MessageManager messageManager = ProcessManager.GetSystemSafe<MessageManager>();
			ScreenControlsRequestMessage message = new ScreenControlsRequestMessage { OnScreenControlsGroup = m_screenControlsGroupOverride };
			if (messageManager != null)
			{
				messageManager.ComponentMessagesWithCompletion.PublishMessage(in character, in message, OnRequestComplete, -1);
			}
		}

		// 06002351 (1.10.1 RVA 0x586460)
		private void OnRequestComplete()
		{;}
	}
}
