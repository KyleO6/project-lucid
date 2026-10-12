using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000399; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeDefaultName("TriggerDying")]
	[GraphNodeMenuFormat("Character/Trigger/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterState_TriggerDying : ActorState_Effect
	{
		[Serializable]
		[FiniteStateMachineJsonCtorArgs]
		protected class DyingJSONCtorArgs : BaseJSONCtorArgs
		{
			public float DyingTime;

			// 0600161f (1.10.1 RVA 0x782D90)
			public DyingJSONCtorArgs()
			{;}
		}

		private readonly float m_dyingTime;

		// 0600161a (1.10.1 RVA 0x7823C0)
		protected CharacterState_TriggerDying(FiniteStateMachine fsm, FSMIdentifier stateId, DyingJSONCtorArgs ctorArgs) : base(fsm, stateId, ctorArgs)
		{
			m_dyingTime = ctorArgs.DyingTime;
		}

		// 0600161b (1.10.1 RVA 0x7823F0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new CharacterState_TriggerDying(fsm, transitionId, JsonUtility.FromJson<DyingJSONCtorArgs>(jsonCtorArgs));
		}

		// 0600161c (1.10.1 RVA 0x782490)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			ProcessManager.GetSystem<CameraYAxisProxyManager>().enabled = false;
			Character character = user.GetAs<Character>();
			character.Close(false);
			float dyingTime = ProcessManager.GetSystem<App>().Storage.GetValue(AppFSMKeys.RestartInProgress, false, true) ? m_dyingTime : 0f;
			character.Storage.SetValue(ActorFSMKeys.InDyingStateTime, dyingTime);
		}

		// 0600161d (1.10.1 RVA 0x782750)
		protected override void DoOnLeave(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnLeave(user, action);
			user.Storage.RemoveValue<bool>(ActorFSMKeys.TriggerDyingState);
			user.Storage.RemoveValue<float>(ActorFSMKeys.InDyingStateTime);
			ProcessManager.GetSystem<CameraYAxisProxyManager>().enabled = true;
		}

		// 0600161e (1.10.1 RVA 0x782A50)
		protected override void DoUpdate(IGraphUser user, FSMUpdateContext updateContext)
		{
			base.DoUpdate(user, updateContext);
			float time = user.Storage.AdjustValue(ActorFSMKeys.InDyingStateTime, updateContext.DeltaTime);
			Character character = user.GetAs<Character>();
			if (!character.Storage.GetValue(ActorFSMKeys.InterruptDyingState, false, true) && time >= m_dyingTime)
			{
				user.Storage.SetValue(ActorFSMKeys.InHardFailureState, true);
			}
		}
	}
}
