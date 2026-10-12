using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 02000149; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/MetaGame/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateMetaGamePrepareLoading : FSMState
	{
		// 060007d3 (1.10.1 RVA 0x65C650)
		protected ApplicationStateMetaGamePrepareLoading(FiniteStateMachine fsm, FSMIdentifier stateId) : base(fsm, stateId)
		{;}

		// 060007d4 (1.10.1 RVA 0x65C660)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			return new ApplicationStateMetaGamePrepareLoading(fsm, stateId);
		}

		// 060007d5 (1.10.1 RVA 0x65C6C0)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			MetaGameState metaGameState = user.Storage.GetValue<MetaGameState>(AppFSMKeys.MetaGameState, null, true);
			user.Storage.SetValue(AppFSMKeys.LoadingParameters, new UIContainerLoadingParameters(metaGameState.LevelDefinition, metaGameState.MissionDefinition));
		}
	}
}
