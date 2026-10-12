using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020000f4; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/Missions/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateMissionSetup : FSMState
	{
		// 060006db (1.10.1 RVA 0x64C940)
		protected ApplicationStateMissionSetup(FiniteStateMachine fsm, FSMIdentifier stateId) : base(fsm, stateId)
		{;}

		// 060006dc (1.10.1 RVA 0x64C950)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			return new ApplicationStateMissionSetup(fsm, stateId);
		}

		// 060006dd (1.10.1 RVA 0x64C9B0)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			LevelManager levelManager = ProcessManager.GetSystem<LevelManager>();
			MissionManager missionManager = ProcessManager.GetSystem<MissionManager>();
			levelManager.TeleportInstant(missionManager.GetActiveMissionStartPosition());
			ProcessManager.GetSystem<GameplayIslandManager>().ReenterActiveIsland();
		}
	}
}
