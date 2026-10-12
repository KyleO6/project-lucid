using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000115; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStatePublishUIEvent : FSMState
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class JSONCtorArgs
		{
			[GraphUnityObject(typeof(UIModernEvent))]
			public string EventId;

			// 0600073b (1.10.1 RVA 0x652DC0)
			public JSONCtorArgs()
			{;}
		}

		private readonly UIModernEvent m_eventId;

		// 06000738 (1.10.1 RVA 0x652C10)
		private ApplicationStatePublishUIEvent(FiniteStateMachine fsm, string stateName, UIModernEvent eventId) : base(fsm, stateName)
		{
			m_eventId = eventId;
		}

		// 06000739 (1.10.1 RVA 0x652C60)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			UIModernEvent eventId = UIModernEvent.FindByName(args.EventId);
			return new ApplicationStatePublishUIEvent(fsm, (string)transitionId, eventId);
		}

		// 0600073a (1.10.1 RVA 0x652D20)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			ProcessManager.GetSystem<App>().AddUniqueUIModernEvent(m_eventId);
		}
	}
}
