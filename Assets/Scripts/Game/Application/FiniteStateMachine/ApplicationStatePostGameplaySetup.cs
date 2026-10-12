using Unity.IL2CPP.CompilerServices;
using System;
using System.Collections.Generic;
using Hardlight;
using Hardlight.Utils;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 0200010f; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public sealed class ApplicationStatePostGameplaySetup : FSMState
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		private class JSONCtorArgs
		{
			[GraphUnityObject(typeof(MissionDefinition))]
			public List<string> SkipEndGameMissions;

			// 0600072e (1.10.1 RVA 0x652550)
			public JSONCtorArgs()
			{;}
		}

		private readonly SystemRef<MissionManager> m_missionManagerRef = ProcessManager.GetSystemRef<MissionManager>();

		private readonly List<MissionDefinition> m_skipEndGameMissions;

		// 0600072b (1.10.1 RVA 0x651D60)
		private ApplicationStatePostGameplaySetup(FiniteStateMachine fsm, string stateName, List<MissionDefinition> skipEndGameMissions) : base(fsm, stateName)
		{
			m_skipEndGameMissions = skipEndGameMissions;
		}

		// 0600072c (1.10.1 RVA 0x651E20)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			List<MissionDefinition> missions = new List<MissionDefinition>(args.SkipEndGameMissions.Count);
			foreach (string mission in args.SkipEndGameMissions)
			{
				missions.Add(ObjectUtils.FindByName<MissionDefinition>(mission));
			}
			return new ApplicationStatePostGameplaySetup(fsm, transitionId, missions);
		}

		// 0600072d (1.10.1 RVA 0x652090)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			App app = user.GetAs<App>();
			app.SetHasGameplayControl(false, false);
			app.Storage.SetValue(AppFSMKeys.IsInPostGame, true);
			if (m_missionManagerRef.TryGet(out MissionManager missionManager) &&
				missionManager.TryGetActiveMissionState(out MissionState missionState, false))
			{
				GameplayLevelDefinition levelDefinition = ProcessManager.GetSystem<DataManager>()
					.MissionDefinitionLevelLookup[missionState.Definition];
				ProcessManager.GetSystem<ProgressionManager>().CacheLevelProgress(levelDefinition);
				foreach (MissionDefinition mission in m_skipEndGameMissions)
				{
					if (mission != missionState.Definition)
					{
						continue;
					}
					user.Storage.SetValue(AppFSMKeys.CutsceneSkipEndGame, true);
				}
			}
		}
	}
}
