using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 02000173; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationTransitionGameCenterLoginComplete : FSMTransition
	{
		private readonly SystemRef<GameCenter> m_gameCenterRef;

		// 06000851 (1.10.1 RVA 0x663890)
		public ApplicationTransitionGameCenterLoginComplete(FiniteStateMachine fsm, FSMIdentifier transitionId) : base(fsm, transitionId)
		{
			m_gameCenterRef = ProcessManager.GetSystemRef<GameCenter>();
		}

		// 06000852 (1.10.1 RVA 0x663930)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new ApplicationTransitionGameCenterLoginComplete(fsm, transitionId);
		}

		// 06000853 (1.10.1 RVA 0x663A00)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext context)
		{
			return m_gameCenterRef.IsValid() && m_gameCenterRef.Get().SystemIsReady;
		}
	}
}
