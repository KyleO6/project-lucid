using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using Hardlight.Utils;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 0200011a; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateRequestAppReview : FSMState
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class JSONCtorArgs
		{
			[GraphUnityObject(typeof(AppRatingRequesterBase))]
			public string Requester;

			// 06000746 (1.10.1 RVA 0x653740)
			public JSONCtorArgs()
			{;}
		}

		private readonly AppRatingRequesterBase m_requester;

		// 06000743 (1.10.1 RVA 0x6534D0)
		private ApplicationStateRequestAppReview(FiniteStateMachine fsm, string stateName, AppRatingRequesterBase requester) : base(fsm, stateName)
		{
			m_requester = requester;
		}

		// 06000744 (1.10.1 RVA 0x653520)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			AppRatingRequesterBase requester = ObjectUtils.FindByName<AppRatingRequesterBase>(args.Requester);
			return new ApplicationStateRequestAppReview(fsm, transitionId, requester);
		}

		// 06000745 (1.10.1 RVA 0x653650)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			m_requester.TryRequest();
		}
	}
}
