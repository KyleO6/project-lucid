using Unity.IL2CPP.CompilerServices;
using System;
using System.Collections.Generic;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 02000133; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateInitCutscene : FSMState
	{
		// 06000786 (1.10.1 RVA 0x656B90)
		public ApplicationStateInitCutscene(FiniteStateMachine fsm, FSMIdentifier stateId) : base(fsm, stateId)
		{;}

		// 06000787 (1.10.1 RVA 0x656BA0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			return new ApplicationStateInitCutscene(fsm, stateId);
		}

		// 06000788 (1.10.1 RVA 0x656C00)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			if (!user.Storage.TryGetValue(AppFSMKeys.CutsceneQueued, out CutsceneDefinition cutscene))
			{
				return;
			}
			LevelManager levelManager = ProcessManager.GetSystem<LevelManager>();
			GameplayLevelDefinition level = levelManager.GetLevelForCutscene(cutscene);
			user.Storage.SetValue(AppFSMKeys.LevelLoadParameters, new LevelManagerLoadLevelParameters(level));
			MissionDefinition mission = level.MissionList.GetMissions()[0];
			MissionManager missionManager = ProcessManager.GetSystem<MissionManager>();
			GameplayLevelDefinition missionContextLevel = cutscene.LevelDefinitionForMissionContext == null ? level : cutscene.LevelDefinitionForMissionContext;
			missionManager.SetActiveMissionContext(new MissionContextRegular(mission, missionContextLevel, false));
			user.Storage.SetValue(AppFSMKeys.UIContainerParameters, new UIContainerLoadingParameters(level, mission));
			user.Storage.SetValue(AppFSMKeys.LevelPlayed, true);
			user.Storage.SetValue(AppFSMKeys.GameplayActive, true);
			CharacterId characterId = GetCharacterId();
			user.Storage.SetValue(AppFSMKeys.CurrentCharacterId, characterId);
			user.Storage.SetValue(AppFSMKeys.MissionCharacterId, characterId);
		}

		// 06000789 (1.10.1 RVA 0x657360)
		private static CharacterId GetCharacterId()
		{
			List<CharacterId> characters = new List<CharacterId>();
			ProcessManager.GetSystem<MissionManager>().GetCharactersForActiveMission(ref characters);
			return characters[0];
		}
	}
}
