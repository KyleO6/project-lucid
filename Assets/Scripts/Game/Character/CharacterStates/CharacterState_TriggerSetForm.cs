using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020003ad; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeDefaultName("TriggerSetForm")]
	[GraphNodeMenuFormat("Character/Trigger/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterState_TriggerSetForm : FSMState_Trigger
	{
		[Serializable]
		[FiniteStateMachineJsonCtorArgs]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class SetFormJSONCtorArgs : TriggerJSONCtorArgs
		{
			public ActorFormType FormType;

			public bool FormIsLocked;

			public bool FormClearsLock;

			// 06001662 (1.10.1 RVA 0x7854A0)
			public SetFormJSONCtorArgs()
			{
			}
		}

		private readonly ActorFormType m_formType;

		private readonly bool m_formIsLocked;

		private readonly bool m_formClearsLock;

		// 0600165f (1.10.1 RVA 0x785350)
		protected CharacterState_TriggerSetForm(FiniteStateMachine fsm, FSMIdentifier stateId, SetFormJSONCtorArgs ctorArgs) : base(fsm, stateId, ctorArgs)
		{
			m_formType = ctorArgs.FormType;
			m_formIsLocked = ctorArgs.FormIsLocked;
			m_formClearsLock = ctorArgs.FormClearsLock;
		}

		// 06001660 (1.10.1 RVA 0x785390)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new CharacterState_TriggerSetForm(fsm, transitionId, JsonUtility.FromJson<SetFormJSONCtorArgs>(jsonCtorArgs));
		}

		// 06001661 (1.10.1 RVA 0x785440)
		protected override void InvokeTriggerStart(IGraphUser user, FSMStateChangeAction action)
		{
			user.GetAs<Character>().SwitchToForm(m_formType, m_formIsLocked, m_formClearsLock);
		}
	}
}
