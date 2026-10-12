using Unity.IL2CPP.CompilerServices;
using System;
using System.Collections.Generic;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 0200019c; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationTransitionUIMessageInQueue : FSMTransition
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class JSONCtorArgs
		{
			[GraphUnityObject(typeof(UIModernEvent))]
			public List<string> EventIds;

			// 060008ae (1.10.1 RVA 0x667940)
			public JSONCtorArgs()
			{
			}
		}

		private readonly List<UIModernEvent> m_eventId;

		// 060008ab (1.10.1 RVA 0x667640)
		public ApplicationTransitionUIMessageInQueue(FiniteStateMachine fsm, FSMIdentifier transitionId, List<string> eventIds) : base(fsm, transitionId)
		{
			m_eventId = new List<UIModernEvent>();
			for (int i = 0; i < eventIds.Count; i++)
			{
				m_eventId.Add(UIModernEvent.FindByName(eventIds[i]));
			}
		}

		// 060008ac (1.10.1 RVA 0x6677B0)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			return new ApplicationTransitionUIMessageInQueue(fsm, transitionId, args.EventIds);
		}

		// 060008ad (1.10.1 RVA 0x667840)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext context)
		{
			base.DoUpdate(user, context);
			List<UIModernEvent> events = user.GetAs<App>().GetUIModernEvents();
			if (events.Count == 0)
			{
				return false;
			}
			for (int i = 0; i < m_eventId.Count; i++)
			{
				if (events.Contains(m_eventId[i]))
				{
					return true;
				}
			}
			return false;
		}
	}
}
