using Unity.IL2CPP.CompilerServices;
using System;
using System.Collections.Generic;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 0200019a; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationTransitionUIMessageCloseUIContainers : ApplicationTransitionUIMessage
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected new class JSONCtorArgs
		{
			[GraphUnityObject(typeof(UIModernEvent))]
			public string EventId;

			[GraphUnityObject(typeof(UIContainerIdentifier))]
			public List<string> Containers;

			// 060008aa (1.10.1 RVA 0x667630)
			public JSONCtorArgs()
			{;}
		}

		private readonly List<UIContainerIdentifier> m_containers;

		// 060008a7 (1.10.1 RVA 0x667160)
		private ApplicationTransitionUIMessageCloseUIContainers(FiniteStateMachine fsm, FSMIdentifier transitionId, UIModernEvent eventId, List<UIContainerIdentifier> containers) : base(fsm, transitionId, eventId)
		{
			m_containers = containers;
		}

		// 060008a8 (1.10.1 RVA 0x6671B0)
		public new static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			List<UIContainerIdentifier> containers = new List<UIContainerIdentifier>(args.Containers.Count);
			foreach (string container in args.Containers)
			{
				containers.Add(UIContainerIdentifier.FindByName(container));
			}
			return new ApplicationTransitionUIMessageCloseUIContainers(fsm, transitionId, UIModernEvent.FindByName(args.EventId), containers);
		}

		// 060008a9 (1.10.1 RVA 0x667430)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext context)
		{
			bool received = base.DoUpdate(user, context);
			if (received)
			{
				UIManager uiManager = ProcessManager.GetSystem<UIManager>();
				foreach (UIContainerIdentifier container in m_containers)
				{
					uiManager.Close(container);
				}
			}
			return received;
		}
	}
}
