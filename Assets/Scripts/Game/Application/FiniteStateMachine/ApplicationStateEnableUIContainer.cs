using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020000cc; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateEnableUIContainer : FSMState
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		private class JSONCtorArgs
		{
			[GraphUnityObject(typeof(UIContainerIdentifier))]
			public string Container;

			public bool Enable;

			// 06000657 (1.10.1 RVA 0x644F00)
			public JSONCtorArgs()
			{;}
		}

		private readonly UIContainerIdentifier m_containerIdentifier;

		private readonly bool m_enable;

		// 06000654 (1.10.1 RVA 0x644D50)
		private ApplicationStateEnableUIContainer(FiniteStateMachine fsm, string stateName, UIContainerIdentifier containerIdentifier, bool enable) : base(fsm, stateName)
		{
			m_containerIdentifier = containerIdentifier;
			m_enable = enable;
		}

		// 06000655 (1.10.1 RVA 0x644DB0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			UIContainerIdentifier container = UIContainerIdentifier.FindByName(args.Container);
			return new ApplicationStateEnableUIContainer(fsm, stateId, container, args.Enable);
		}

		// 06000656 (1.10.1 RVA 0x644E80)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			ProcessManager.GetSystem<UIManager>().SetEnabled(m_containerIdentifier, m_enable);
		}
	}
}
