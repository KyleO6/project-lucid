using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020000d1; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public sealed class ApplicationStateForceSaveClearUnlocks : FSMState
	{
		// 06000661 (1.10.1 RVA 0x645680)
		public ApplicationStateForceSaveClearUnlocks(FiniteStateMachine fsm, string stateName) : base(fsm, stateName)
		{
		}

		// 06000662 (1.10.1 RVA 0x6456B0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			return new ApplicationStateForceSaveClearUnlocks(fsm, stateId);
		}

		// 06000663 (1.10.1 RVA 0x645730)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			MissionManager missionManager = ProcessManager.GetSystem<MissionManager>();
			LevelManager levelManager = ProcessManager.GetSystem<LevelManager>();
			missionManager.UpdateUnlocks();
			levelManager.UpdateUnlocks();
			List<IMetaGameUnlock> unlocks = user.Storage.GetValue<List<IMetaGameUnlock>>(AppFSMKeys.MetaGameUnlocks);
			SaveManager saveManager = ProcessManager.GetSystem<SaveManager>();
			foreach (IMetaGameUnlock unlock in unlocks)
			{
				unlock.Save(saveManager);
			}
			unlocks.Clear();
		}
	}
}
