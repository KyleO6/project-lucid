using Unity.IL2CPP.CompilerServices;
using System;
using System.Collections.Generic;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020000ca; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateCloseUIContainer : FSMState
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		private class JSONCtorArgs
		{
			[GraphUnityObject(typeof(UIContainerIdentifier))]
			public string Container;

			[GraphUnityObject(typeof(UIContainerIdentifier))]
			public List<string> DoNotCloseIfOpen;

			// 06000653 (1.10.1 RVA 0x644D40)
			public JSONCtorArgs()
			{;}
		}

		private readonly UIContainerIdentifier m_containerIdentifier;

		private readonly List<UIContainerIdentifier> m_doNotCloseIfOpen;

		// 06000650 (1.10.1 RVA 0x644880)
		private ApplicationStateCloseUIContainer(FiniteStateMachine fsm, string stateName, UIContainerIdentifier containerIdentifier, List<UIContainerIdentifier> doNotCloseIfOpen) : base(fsm, stateName)
		{
			m_containerIdentifier = containerIdentifier;
			m_doNotCloseIfOpen = doNotCloseIfOpen;
		}

		// 06000651 (1.10.1 RVA 0x6448F0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			UIContainerIdentifier containerIdentifier = UIContainerIdentifier.FindByName(args.Container);
			List<UIContainerIdentifier> doNotCloseIfOpen = new List<UIContainerIdentifier>(args.DoNotCloseIfOpen.Count);
			foreach (string container in args.DoNotCloseIfOpen)
			{
				doNotCloseIfOpen.Add(UIContainerIdentifier.FindByName(container));
			}
			return new ApplicationStateCloseUIContainer(fsm, stateId, containerIdentifier, doNotCloseIfOpen);
		}

		// 06000652 (1.10.1 RVA 0x644B90)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			UIManager uiManager = ProcessManager.GetSystem<UIManager>();
			if (m_doNotCloseIfOpen.Count > 0)
			{
				foreach (UIContainerIdentifier identifier in m_doNotCloseIfOpen)
				{
					if (uiManager.IsOpen(identifier))
					{
						return;
					}
				}
			}
			uiManager.Close(m_containerIdentifier);
		}
	}
}
