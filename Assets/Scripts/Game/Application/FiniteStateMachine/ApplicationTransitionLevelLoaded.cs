using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 02000188; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationTransitionLevelLoaded : FSMTransition
	{
		// 06000881 (1.10.1 RVA 0x6656A0)
		public ApplicationTransitionLevelLoaded(FiniteStateMachine fsm, FSMIdentifier transitionId) : base(fsm, transitionId)
		{
		}

		// 06000882 (1.10.1 RVA 0x6656B0)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new ApplicationTransitionLevelLoaded(fsm, transitionId);
		}

		// 06000883 (1.10.1 RVA 0x665710)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext context)
		{
			return !ProcessManager.GetSystem<App>().Storage.HasValue(AppFSMKeys.LevelLoadParameters);
		}
	}
}
