using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000189; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationTransitionLevelPlayed : FSMTransition
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		public class JSONCtorArgs
		{
			public bool TransitionIfTrue;

			// 06000887 (1.10.1 RVA 0x665A10)
			public JSONCtorArgs()
			{;}
		}

		private bool m_transitionIfTrue;

		// 06000884 (1.10.1 RVA 0x6657C0)
		private ApplicationTransitionLevelPlayed(FiniteStateMachine fsm, FSMIdentifier transitionId, JSONCtorArgs ctorArgs) : base(fsm, transitionId)
		{
			m_transitionIfTrue = ctorArgs.TransitionIfTrue;
		}

		// 06000885 (1.10.1 RVA 0x6657F0)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new ApplicationTransitionLevelPlayed(fsm, transitionId, JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs));
		}

		// 06000886 (1.10.1 RVA 0x665890)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext updateContext)
		{
			return user.Storage.GetValue(AppFSMKeys.LevelPlayed, false, true) == m_transitionIfTrue;
		}
	}
}
