using Unity.IL2CPP.CompilerServices;
using Hardlight;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace HardlightProject
{
	// Original Game.Runtime 02000191; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationTransitionSceneOperationComplete : FSMTransition
	{
		// 06000894 (1.10.1 RVA 0x666830)
		public ApplicationTransitionSceneOperationComplete(FiniteStateMachine fsm, FSMIdentifier transitionId) : base(fsm, transitionId)
		{
			
		}

		// 06000895 (1.10.1 RVA 0x666840)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new ApplicationTransitionSceneOperationComplete(fsm, transitionId);
		}

		// 06000896 (1.10.1 RVA 0x6668A0)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext updateContext)
		{
			App app = user.GetAs<App>();
			return app.Storage.TryGetValue(AppFSMKeys.SceneOpKey, out AsyncOperationHandle<SceneInstance> operation) && operation.IsDone;
		}
	}
}
