using Unity.IL2CPP.CompilerServices;
using System;
using System.Collections.Generic;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000194; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationTransitionStateMessage : FSMTransition
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class JSONCtorArgs
		{
			[GraphUnityObject(typeof(ApplicationStateEvent))]
			public string EventId;

			// 0600089e (1.10.1 RVA 0x666DA0)
			public JSONCtorArgs()
			{;}
		}

		private readonly ApplicationStateEvent m_eventId;

		// 0600089b (1.10.1 RVA 0x666B10)
		private ApplicationTransitionStateMessage(FiniteStateMachine fsm, FSMIdentifier transitionId, ApplicationStateEvent eventId) : base(fsm, transitionId)
		{
			m_eventId = eventId;
		}

		// 0600089c (1.10.1 RVA 0x666B40)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			return new ApplicationTransitionStateMessage(fsm, transitionId, ApplicationStateEvent.FindByName(args.EventId));
		}

		// 0600089d (1.10.1 RVA 0x666BF0)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext context)
		{
			base.DoUpdate(user, context);
			return user.Storage.GetValue<List<ApplicationStateEvent>>(AppFSMKeys.StateEvents).Remove(m_eventId);
		}
	}
}
