using Unity.IL2CPP.CompilerServices;
using System;
using System.Collections.Generic;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020000c8; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateCloseAllUIContainers : FSMState
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class JSONCtorArgs
		{
			[GraphUnityObject(typeof(UIContainerIdentifier))]
			public List<string> DoNotCloseContainers;

			// 0600064f (1.10.1 RVA 0x644870)
			public JSONCtorArgs()
			{;}
		}

		private readonly IReadOnlyList<UIContainerIdentifier> m_doNotCloseContainers;

		// 0600064c (1.10.1 RVA 0x644500)
		public ApplicationStateCloseAllUIContainers(FiniteStateMachine fsm, string stateName, List<UIContainerIdentifier> doNotCloseContainers) : base(fsm, stateName)
		{
			m_doNotCloseContainers = doNotCloseContainers;
		}

		// 0600064d (1.10.1 RVA 0x644550)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			List<UIContainerIdentifier> doNotCloseContainers = new List<UIContainerIdentifier>();
			if (args?.DoNotCloseContainers != null)
			{
				foreach (string container in args.DoNotCloseContainers)
				{
					doNotCloseContainers.Add(UIContainerIdentifier.FindByName(container));
				}
			}
			return new ApplicationStateCloseAllUIContainers(fsm, stateId, doNotCloseContainers);
		}

		// 0600064e (1.10.1 RVA 0x6447F0)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			ProcessManager.GetSystem<UIManager>().CloseAllContainersExcept(m_doNotCloseContainers);
		}
	}
}
