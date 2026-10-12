using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 0200039b; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeDefaultName("TriggerEnableRenderers")]
	[GraphNodeMenuFormat("Character/Trigger/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterState_TriggerEnableRenderers : FSMState_Trigger
	{
		[Serializable]
		[FiniteStateMachineJsonCtorArgs]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class EnableRenderersJSONCtorArgs : TriggerJSONCtorArgs
		{
			public bool RenderersEnabled;

			// 06001625 (1.10.1 RVA 0x782F80)
			public EnableRenderersJSONCtorArgs()
			{
			}
		}

		private readonly bool m_renderersEnabled;

		// 06001620 (1.10.1 RVA 0x782DA0)
		protected CharacterState_TriggerEnableRenderers(FiniteStateMachine fsm, FSMIdentifier stateId, EnableRenderersJSONCtorArgs ctorArgs) : base(fsm, stateId, ctorArgs)
		{
			m_renderersEnabled = ctorArgs.RenderersEnabled;
		}

		// 06001621 (1.10.1 RVA 0x782DD0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new CharacterState_TriggerEnableRenderers(fsm, transitionId, JsonUtility.FromJson<EnableRenderersJSONCtorArgs>(jsonCtorArgs));
		}

		// 06001622 (1.10.1 RVA 0x782E70)
		protected override void InvokeTriggerStart(IGraphUser user, FSMStateChangeAction action)
		{
			SetRenderersEnabled(user, m_renderersEnabled);
		}

		// 06001623 (1.10.1 RVA 0x782F10)
		protected override void InvokeTriggerEnd(IGraphUser user, FSMStateChangeAction action)
		{
			SetRenderersEnabled(user, !m_renderersEnabled);
		}

		// 06001624 (1.10.1 RVA 0x782EC0)
		private void SetRenderersEnabled(IGraphUser user, bool enabled)
		{
			user.GetAs<Character>().ToggleFormRenderers(enabled);
		}
	}
}
