using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 02000148; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/MetaGame/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateMetaGameCleanup : FSMState
	{
		// 060007d0 (1.10.1 RVA 0x65BC50)
		private ApplicationStateMetaGameCleanup(FiniteStateMachine fsm, FSMIdentifier stateId) : base(fsm, stateId)
		{;}

		// 060007d1 (1.10.1 RVA 0x65BC60)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			return new ApplicationStateMetaGameCleanup(fsm, stateId);
		}

		// 060007d2 (1.10.1 RVA 0x65BCC0)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			MetaGameState metaGameState = user.Storage.GetValue<MetaGameState>(AppFSMKeys.MetaGameState, null, true);
			IMissionContext missionContext = ProcessManager.GetSystem<MissionManager>().ActiveMissionContext;
			bool updateInSave = missionContext == null || missionContext.IsReplay;
			App app = user.GetAs<App>();
			GameplayLevelDefinition levelDefinition = metaGameState != null
				? metaGameState.LevelDefinition
				: ProcessManager.GetSystem<LevelManager>().GetCurrentLevelUnsafe().LevelDefinition;
			app.SetLastLevelUIVisitedGUID(levelDefinition, updateInSave);
			app.SetHasGameplayControl(true);
			user.Storage.SetValue(AppFSMKeys.LevelPlayed, true);
			user.Storage.SetValue(AppFSMKeys.MetaGameActive, false);
			user.Storage.SetValue(AppFSMKeys.GameplayActive, true);
			user.Storage.RemoveValue<MetaGameState>(AppFSMKeys.MetaGameState);
			user.Storage.RemoveValue<MetaGameState>(AppFSMKeys.LevelUnlocked);
			user.Storage.RemoveValue<bool>(AppFSMKeys.CutsceneSkipEndGame);
		}
	}
}
