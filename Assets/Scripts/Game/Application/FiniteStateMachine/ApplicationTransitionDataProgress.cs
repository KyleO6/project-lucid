using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 0200016e; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationTransitionDataProgress : FSMTransition
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		private class JSONCtorArgs
		{
			public float Threshold;

			// 06000846 (1.10.1 RVA 0x663290)
			public JSONCtorArgs()
			{;}
		}

		private readonly float m_threshold;

		// 06000843 (1.10.1 RVA 0x6630F0)
		public ApplicationTransitionDataProgress(FiniteStateMachine fsm, FSMIdentifier transitionId, float threshold) : base(fsm, transitionId)
		{
			m_threshold = threshold;
		}

		// 06000844 (1.10.1 RVA 0x663120)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new ApplicationTransitionDataProgress(fsm, transitionId, JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs).Threshold);
		}

		// 06000845 (1.10.1 RVA 0x6631C0)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext updateContext)
		{
			App app = (App)user;
			return app.DataManager != null && app.DataManager.Progress >= m_threshold;
		}
	}
}
