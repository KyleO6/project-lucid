using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 02000127; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateTimeTrials : FSMState
	{
		private readonly SystemRef<TimeTrialLeaderboardManager> m_timeTrialLeaderboardManagerRef = ProcessManager.GetSystemRef<TimeTrialLeaderboardManager>();

		// 06000764 (1.10.1 RVA 0x654D20)
		public ApplicationStateTimeTrials(FiniteStateMachine fsm, string stateName) : base(fsm, stateName)
		{
		}

		// 06000765 (1.10.1 RVA 0x654DC0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new ApplicationStateTimeTrials(fsm, transitionId);
		}

		// 06000766 (1.10.1 RVA 0x654E30)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			RegisterTimeTrialLeaderboards(user.GetAs<App>());
		}

		// 06000767 (1.10.1 RVA 0x654EF0)
		private void RegisterTimeTrialLeaderboards(App app)
		{
			if (!m_timeTrialLeaderboardManagerRef.IsValid())
			{
				app.RegisterAndInitialiseSystem<TimeTrialLeaderboardManager>();
			}
		}
	}
}
