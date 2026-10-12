using Unity.IL2CPP.CompilerServices;
using System;
using System.Collections.Generic;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000111; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStatePrerenderEnvironment : FSMState
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class JSONCtorArgs
		{
			[GraphUnityObject(typeof(ApplicationStateEvent))]
			public string EventId;

			// 06000733 (1.10.1 RVA 0x652930)
			public JSONCtorArgs()
			{;}
		}

		private readonly ApplicationStateEvent m_stateEvent;

		// 0600072f (1.10.1 RVA 0x652560)
		private ApplicationStatePrerenderEnvironment(FiniteStateMachine fsm, FSMIdentifier stateId, ApplicationStateEvent stateEvent) : base(fsm, stateId)
		{
			m_stateEvent = stateEvent;
		}

		// 06000730 (1.10.1 RVA 0x652590)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			return new ApplicationStatePrerenderEnvironment(fsm, stateId, ApplicationStateEvent.FindByName(args.EventId));
		}

		// 06000731 (1.10.1 RVA 0x652640)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			if (ProcessManager.GetSystem<LevelManager>().TryGetCurrentLevel(out LevelManagerLevel currentLevel))
			{
				ShaderPrewarmer prewarmer = currentLevel.Data.TrackManager.ShaderPrewarmer;
				if (prewarmer == null)
				{
					PublishMessage(user);
				}
				else
				{
					prewarmer.StartPrewarmHashedComponentGroups(PublishMessage, user);
				}
			}
		}

		// 06000732 (1.10.1 RVA 0x652790)
		private void PublishMessage(IGraphUser user)
		{
			user.Storage.GetValue<List<ApplicationStateEvent>>(AppFSMKeys.StateEvents, null, true).AddUnique(m_stateEvent);
		}
	}
}
