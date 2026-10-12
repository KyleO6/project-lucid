using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000192; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationTransitionSplashProgress : FSMTransition
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		private class JSONCtorArgs
		{
			public float Threshold;

			// 0600089a (1.10.1 RVA 0x666B00)
			public JSONCtorArgs()
			{;}
		}

		private readonly float m_threshold;

		// 06000897 (1.10.1 RVA 0x666980)
		public ApplicationTransitionSplashProgress(FiniteStateMachine fsm, FSMIdentifier transitionId, float threshold) : base(fsm, transitionId)
		{
			m_threshold = threshold;
		}

		// 06000898 (1.10.1 RVA 0x6669B0)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			return new ApplicationTransitionSplashProgress(fsm, transitionId, args.Threshold);
		}

		// 06000899 (1.10.1 RVA 0x666A50)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext updateContext)
		{
			App app = user.GetAs<App>();
			return app.SplashScreen != null && app.SplashScreen.Progress >= m_threshold;
		}
	}
}
