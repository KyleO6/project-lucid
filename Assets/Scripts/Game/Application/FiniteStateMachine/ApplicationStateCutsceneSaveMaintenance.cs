using Unity.IL2CPP.CompilerServices;
using System;
using System.Collections.Generic;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 02000132; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public sealed class ApplicationStateCutsceneSaveMaintenance : FSMState
	{
		// 06000782 (1.10.1 RVA 0x656370)
		private ApplicationStateCutsceneSaveMaintenance(FiniteStateMachine fsm, string stateName) : base(fsm, stateName)
		{
		}

		// 06000783 (1.10.1 RVA 0x6563A0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			return new ApplicationStateCutsceneSaveMaintenance(fsm, stateId);
		}

		// 06000784 (1.10.1 RVA 0x656420)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			SaveManager saveManager = ProcessManager.GetSystem<SaveManager>();
			SaveDataGame saveData = saveManager.CurrentSave;
			if (saveData.CutsceneSeenSaveMaintenance)
			{
				return;
			}
			List<CutsceneDefinition> cutscenes = new List<CutsceneDefinition>(ProcessManager.GetSystem<DataManager>().CutsceneDefinitions.Values);
			if (HasDefeatedFinalBoss())
			{
				foreach (CutsceneDefinition cutscene in cutscenes)
				{
					saveData.AddSeenCutsceneGuid(cutscene.GetGUID());
				}
				saveManager.RequestSave();
				return;
			}

			HashSet<string> previouslySeenCutscenes = new HashSet<string>();
			previouslySeenCutscenes.AddRange(saveData.CutsceneIsNewGuids);
			foreach (SaveDataLevel levelData in saveData.Levels)
			{
				previouslySeenCutscenes.AddRange(levelData.SeenCutsceneOnLevelGuids);
			}
			foreach (CutsceneDefinition cutscene in cutscenes)
			{
				if (previouslySeenCutscenes.Count < 1)
				{
					break;
				}
				string guid = cutscene.GetGUID();
				saveData.AddSeenCutsceneGuid(guid);
				previouslySeenCutscenes.Remove(guid);
			}
			saveData.CutsceneSeenSaveMaintenance = true;
			saveManager.RequestSave();
		}

		// 06000785 (1.10.1 RVA 0x656B20)
		private static bool HasDefeatedFinalBoss()
		{
			GameplayLevelDefinition finalBossLevelDefinition = ProcessManager.GetSystem<LevelManager>().FinalBossLevelDefinition;
			return finalBossLevelDefinition.MissionList.HasAnyCompleteMissions(finalBossLevelDefinition);
		}
	}
}
