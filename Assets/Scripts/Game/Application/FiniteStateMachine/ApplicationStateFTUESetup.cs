using Unity.IL2CPP.CompilerServices;
using System;
using System.Collections.Generic;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 02000134; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/FTUE/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateFTUESetup : FSMState
	{
		// 0600078a (1.10.1 RVA 0x657440)
		protected ApplicationStateFTUESetup(FiniteStateMachine fsm, FSMIdentifier stateId) : base(fsm, stateId)
		{;}

		// 0600078b (1.10.1 RVA 0x657450)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			return new ApplicationStateFTUESetup(fsm, stateId);
		}

		// 0600078c (1.10.1 RVA 0x6574B0)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			GameplayLevelDefinition levelDefinition = ProcessManager.GetSystem<LevelManager>().GetDefaultProductionLevel();
			user.Storage.SetValue(AppFSMKeys.LevelLoadParameters, new LevelManagerLoadLevelParameters(levelDefinition));
			MissionDefinition missionDefinition = levelDefinition.MissionList.GetMissions()[0];
			MissionManager missionManager = ProcessManager.GetSystem<MissionManager>();
			missionManager.SetActiveMissionContext(new MissionContextRegular(missionDefinition, levelDefinition, false));
			if (missionManager.TryGetActiveMissionState(out MissionState missionState) && missionState.Complete)
			{
				missionManager.SetActiveMissionContext(new MissionContextRegular(missionDefinition, levelDefinition, true));
			}
			user.Storage.SetValue(AppFSMKeys.UIContainerParameters, new UIContainerLoadingParameters(levelDefinition, missionDefinition));
			user.Storage.SetValue(AppFSMKeys.LevelPlayed, true);
			user.Storage.SetValue(AppFSMKeys.GameplayActive, true);
			CharacterId characterId = GetCharacterId();
			user.Storage.SetValue(AppFSMKeys.CurrentCharacterId, characterId);
			user.Storage.SetValue(AppFSMKeys.MissionCharacterId, characterId);
			user.GetAs<App>().SetLastLevelUIVisitedGUID(levelDefinition, true);
		}

		// 0600078d (1.10.1 RVA 0x657C40)
		private static CharacterId GetCharacterId()
		{
			List<CharacterId> characterIds = new List<CharacterId>();
			ProcessManager.GetSystem<MissionManager>().GetCharactersForActiveMission(ref characterIds);
			return characterIds[0];
		}
	}
}
