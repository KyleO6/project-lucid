using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000181; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationTransitionIsLevelLoading : FSMTransition
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class JSONCtorArgs
		{
			public bool TrueCausesTransition;

			// 06000873 (1.10.1 RVA 0x664DF0)
			public JSONCtorArgs()
			{;}
		}

		private readonly bool m_trueCausesTransition;

		// 06000870 (1.10.1 RVA 0x664C60)
		private ApplicationTransitionIsLevelLoading(FiniteStateMachine fsm, FSMIdentifier transitionId, bool trueCausesTransition) : base(fsm, transitionId)
		{
			m_trueCausesTransition = trueCausesTransition;
		}

		// 06000871 (1.10.1 RVA 0x664C80)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			return new ApplicationTransitionIsLevelLoading(fsm, transitionId, args.TrueCausesTransition);
		}

		// 06000872 (1.10.1 RVA 0x664D20)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext context)
		{
			bool isLoading = ProcessManager.GetSystem<App>().Storage.HasValue(AppFSMKeys.LevelLoadParameters);
			return isLoading == m_trueCausesTransition;
		}
	}
}
