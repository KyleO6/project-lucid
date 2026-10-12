using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 0200017d; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationTransitionIsFirstTimePlaying : FSMTransition
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class JSONCtorArgs
		{
			public bool TrueCausesTransition;

			// 0600086a (1.10.1 RVA 0x6647E0)
			public JSONCtorArgs()
			{;}
		}

		private readonly bool m_trueCausesTransition;

		// 06000867 (1.10.1 RVA 0x6646A0)
		private ApplicationTransitionIsFirstTimePlaying(FiniteStateMachine fsm, FSMIdentifier transitionId, bool trueCausesTransition) : base(fsm, transitionId)
		{
			m_trueCausesTransition = trueCausesTransition;
		}

		// 06000868 (1.10.1 RVA 0x6646C0)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			return new ApplicationTransitionIsFirstTimePlaying(fsm, transitionId, args.TrueCausesTransition);
		}

		// 06000869 (1.10.1 RVA 0x664760)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext context)
		{
			return ProcessManager.GetSystem<LevelManager>().HasFTUEBeenCompleted() ^ m_trueCausesTransition;
		}
	}
}
