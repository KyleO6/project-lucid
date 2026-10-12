using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 0200014e; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/Missions/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateLoadMissions : FSMState
	{
		// 060007e8 (1.10.1 RVA 0x65EC90)
		public ApplicationStateLoadMissions(FiniteStateMachine fsm, string stateName) : base(fsm, stateName)
		{
		}

		// 060007e9 (1.10.1 RVA 0x65ECC0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new ApplicationStateLoadMissions(fsm, transitionId);
		}

		// 060007ea (1.10.1 RVA 0x65ED40)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			MissionManager missionManager = ProcessManager.GetSystem<MissionManager>();
			if (missionManager.MissionList == null)
			{
				return;
			}
			if (!ProcessManager.GetSystem<LevelManager>().TryGetCurrentLevel(out LevelManagerLevel currentLevel))
			{
				return;
			}
			foreach (MissionDefinition missionDefinition in missionManager.MissionList.GetMissions())
			{
				missionManager.LoadMission(missionDefinition, currentLevel.LevelDefinition);
			}
		}
	}
}
