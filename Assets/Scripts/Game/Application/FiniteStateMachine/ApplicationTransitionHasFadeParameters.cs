using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 02000176; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationTransitionHasFadeParameters : FSMTransition
	{
		// 06000858 (1.10.1 RVA 0x663CE0)
		private ApplicationTransitionHasFadeParameters(FiniteStateMachine fsm, FSMIdentifier transitionId) : base(fsm, transitionId)
		{
			
		}

		// 06000859 (1.10.1 RVA 0x663CF0)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new ApplicationTransitionHasFadeParameters(fsm, transitionId);
		}

		// 0600085a (1.10.1 RVA 0x663D50)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext context)
		{
			base.DoUpdate(user, context);
			return user.GetAs<App>().Storage.HasValue<FadeTransitionParameters>(AppFSMKeys.TransitionFadeParameters);
		}
	}
}
