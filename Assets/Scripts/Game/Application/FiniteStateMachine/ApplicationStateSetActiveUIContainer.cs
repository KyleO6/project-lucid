using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000120; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateSetActiveUIContainer : FSMState
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		private class JSONCtorArgs
		{
			[GraphUnityObject(typeof(UIContainerIdentifier))]
			public string Container;

			public bool Active;

			public bool CreateIfDoesNotExist;

			// 06000756 (1.10.1 RVA 0x654540)
			public JSONCtorArgs()
			{
				CreateIfDoesNotExist = true;
			}
		}

		private readonly UIContainerIdentifier m_containerIdentifier;

		private readonly bool m_setActive;

		private readonly bool m_createIfDoesNotExist;

		// 06000753 (1.10.1 RVA 0x654350)
		public ApplicationStateSetActiveUIContainer(FiniteStateMachine fsm, string stateName, UIContainerIdentifier containerIdentifier, bool setActive, bool createIfDoesNotExist) : base(fsm, stateName)
		{
			m_containerIdentifier = containerIdentifier;
			m_setActive = setActive;
			m_createIfDoesNotExist = createIfDoesNotExist;
		}

		// 06000754 (1.10.1 RVA 0x6543C0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			UIContainerIdentifier container = UIContainerIdentifier.FindByName(args.Container);
			return new ApplicationStateSetActiveUIContainer(fsm, stateId, container, args.Active, args.CreateIfDoesNotExist);
		}

		// 06000755 (1.10.1 RVA 0x654490)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			UIManager uiManager = ProcessManager.GetSystem<UIManager>();
			if (!uiManager.IsOpen(m_containerIdentifier) && (!m_setActive || !m_createIfDoesNotExist))
			{
				return;
			}
			uiManager.GetOrCreate(m_containerIdentifier).gameObject.SetActive(m_setActive);
		}
	}
}
