using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 02000167; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationTransitionCutsceneLeadsToGameplay : FSMTransition
	{
		private readonly SystemRef<CutsceneManager> m_cutsceneManagerRef;

		// 06000834 (1.10.1 RVA 0x662660)
		public ApplicationTransitionCutsceneLeadsToGameplay(FiniteStateMachine fsm, FSMIdentifier transitionId) : base(fsm, transitionId)
		{
			m_cutsceneManagerRef = ProcessManager.GetSystemRef<CutsceneManager>();
		}

		// 06000835 (1.10.1 RVA 0x662700)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new ApplicationTransitionCutsceneLeadsToGameplay(fsm, transitionId);
		}

		// 06000836 (1.10.1 RVA 0x6627D0)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext context)
		{
			return m_cutsceneManagerRef.TryGet(out CutsceneManager cutsceneManager)
				&& cutsceneManager.TryGetCurrentCutscene(out Cutscene cutscene)
				&& cutscene.Definition.ExplicitlyLeadsIntoGameplay;
		}
	}
}
