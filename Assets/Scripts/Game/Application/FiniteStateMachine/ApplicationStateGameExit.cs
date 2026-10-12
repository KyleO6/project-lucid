using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020000d2; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateGameExit : FSMState
	{
		private readonly SystemRef<SaveManager> m_saveManagerRef = ProcessManager.GetSystemRef<SaveManager>();

		// 06000664 (1.10.1 RVA 0x645B20)
		public ApplicationStateGameExit(FiniteStateMachine fsm, string stateName) : base(fsm, stateName)
		{
		}

		// 06000665 (1.10.1 RVA 0x645BC0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			return new ApplicationStateGameExit(fsm, stateId);
		}

		// 06000666 (1.10.1 RVA 0x645C30)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			AudioManager audioManager = ProcessManager.GetSystem<AudioManager>();
			audioManager.ReleaseAudioSources(true);
			audioManager.ReleaseAudioClips(true);
			user.Storage.RemoveValue<bool>(AppFSMKeys.UIHUDFocusMode);
			user.Storage.RemoveValue<bool>(AppFSMKeys.MetaGameActive);
			user.Storage.RemoveValue<bool>(AppFSMKeys.GameplayActive);
			user.Storage.RemoveValue<bool>(AppFSMKeys.BonusZoneActive);
			user.Storage.RemoveValue<bool>(AppFSMKeys.LevelPlayed);
			user.Storage.RemoveValue<bool>(AppFSMKeys.RestartInProgress);
			LevelManager levelManager = ProcessManager.GetSystem<LevelManager>();
			levelManager.ProcessSystemAction(SystemAction.Shutdown);
			levelManager.ProcessSystemAction(SystemAction.Initialise);
			MissionManager missionManager = ProcessManager.GetSystem<MissionManager>();
			missionManager.SetActiveMissionContext(null);
			missionManager.ClearPendingRewards();
			if (m_saveManagerRef.TryGet(out SaveManager saveManager))
			{
				saveManager.GameIsPlaying = false;
			}
			TimeTrialLeaderboardManager leaderboardManager = ProcessManager.GetSystemSafe<TimeTrialLeaderboardManager>();
			if (leaderboardManager != null)
			{
				leaderboardManager.ProcessSystemAction(SystemAction.Shutdown);
				ProcessManager.UnregisterSystem<TimeTrialLeaderboardManager>();
			}
		}
	}
}
