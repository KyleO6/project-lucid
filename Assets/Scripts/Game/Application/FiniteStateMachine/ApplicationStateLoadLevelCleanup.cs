using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 02000135; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/Levels/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateLoadLevelCleanup : FSMState
	{
		// 0600078e (1.10.1 RVA 0x657D20)
		protected ApplicationStateLoadLevelCleanup(FiniteStateMachine fsm, FSMIdentifier stateId) : base(fsm, stateId)
		{
		}

		// 0600078f (1.10.1 RVA 0x657D30)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			return new ApplicationStateLoadLevelCleanup(fsm, stateId);
		}

		// 06000790 (1.10.1 RVA 0x657D90)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			user.Storage.RemoveValue<LevelManagerLoadLevelParameters>(AppFSMKeys.LevelLoadParameters);
		}
	}
}
