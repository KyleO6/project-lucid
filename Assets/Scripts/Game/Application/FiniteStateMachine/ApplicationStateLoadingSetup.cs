using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 02000147; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/Loading/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateLoadingSetup : FSMState
	{
		// 060007cd (1.10.1 RVA 0x65AF50)
		protected ApplicationStateLoadingSetup(FiniteStateMachine fsm, FSMIdentifier stateId) : base(fsm, stateId)
		{
		}

		// 060007ce (1.10.1 RVA 0x65AF60)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			return new ApplicationStateLoadingSetup(fsm, stateId);
		}

		// 060007cf (1.10.1 RVA 0x65AFC0)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			LevelManager levelManager = ProcessManager.GetSystem<LevelManager>();
			SaveManager saveManager = ProcessManager.GetSystem<SaveManager>();
			SaveDataSettings saveDataSettings = saveManager.GetSaveDataSettings();
			App app = user.GetAs<App>();
			MissionManager missionManager = ProcessManager.GetSystem<MissionManager>();

			if (saveDataSettings.SkipFTUE && !levelManager.HasFTUEBeenCompleted() && saveManager.HasAnySave())
			{
				GameplayLevelDefinition ftueLevel = levelManager.GetDefaultProductionLevel();
				IReadOnlyDictionary<MissionDefinition, SaveDataLevelMission> missionSaveData = missionManager.GetMissionSaveDataForLevel(ftueLevel);
				ftueLevel.TryGetMissionGroups(out IReadOnlyList<MissionGroup> groups);
				foreach (MissionGroup group in groups)
				{
					if (!group.HideOrbCountUntilUnlocked || group.MeetsAllRequirements(ftueLevel))
					{
						foreach (MissionDefinition mission in group.Definitions)
						{
							if (mission.IsReachGoalTracker)
							{
								missionSaveData[mission].MarkComplete(mission.TargetObjectivesToComplete);
							}
						}
					}
				}
				saveManager.CurrentSave.SetLastLevelVisited(ftueLevel);
				saveManager.RequestSave();
				ProcessManager.GetSystem<ProgressionManager>().OnSaveGameOpen(saveManager.CurrentSave);
			}

			GameplayLevelDefinition levelDefinition = null;
			MissionDefinition missionDefinition = null;
			if (app.Storage.TryGetValue(AppFSMKeys.MetaGameState, out MetaGameState metaGameState))
			{
				levelDefinition = metaGameState.LevelDefinition;
				missionDefinition = metaGameState.MissionDefinition;
			}
			if (levelDefinition == null)
			{
				levelDefinition = levelManager.GetProductionLevelByGUID(app.GetLastLevelUISelectedGUID(false));
			}
			if (missionDefinition == null && missionManager.TryGetActiveMissionState(out MissionState missionState, false))
			{
				missionDefinition = missionState.Definition;
			}
			if (!user.Storage.TryGetValue(AppFSMKeys.LevelLoadParameters, out LevelManagerLoadLevelParameters loadParameters) ||
				loadParameters == null || loadParameters.LevelDefinition == null)
			{
				user.Storage.SetValue(AppFSMKeys.LevelLoadParameters, new LevelManagerLoadLevelParameters(levelDefinition));
			}
			user.Storage.SetValue(AppFSMKeys.UIContainerParameters, new UIContainerLoadingParameters(levelDefinition, missionDefinition));
		}
	}
}
