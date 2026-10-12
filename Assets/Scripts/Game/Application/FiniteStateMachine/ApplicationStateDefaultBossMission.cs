using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 0200014d; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/Missions/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateDefaultBossMission : FSMState
	{
		// 060007e5 (1.10.1 RVA 0x65E8E0)
		protected ApplicationStateDefaultBossMission(FiniteStateMachine fsm, FSMIdentifier stateId) : base(fsm, stateId)
		{
		}

		// 060007e6 (1.10.1 RVA 0x65E8F0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			return new ApplicationStateDefaultBossMission(fsm, stateId);
		}

		// 060007e7 (1.10.1 RVA 0x65E950)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			user.Storage.RemoveValue<bool>(AppFSMKeys.FirstTimeBossEntry);
			LevelManager levelManager = ProcessManager.GetSystem<LevelManager>();
			if (levelManager.TryGetCurrentLevel(out LevelManagerLevel level))
			{
				MissionDefinition mission = level.LevelDefinition.MissionList.GetMissions()[0];
				ProcessManager.GetSystem<MissionManager>().SetActiveMissionContext(
					new MissionContextRegular(mission, level.LevelDefinition, false));
			}
			else
			{
				HLOutput.LogError("Should not be in App State ApplicationStateDefaultBossMission without a level loaded.");
			}
		}
	}
}
