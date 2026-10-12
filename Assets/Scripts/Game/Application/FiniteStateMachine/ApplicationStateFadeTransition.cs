using Unity.IL2CPP.CompilerServices;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020000ce; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public sealed class ApplicationStateFadeTransition : FSMState
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		private class JSONCtorArgs
		{
			[GraphUnityObject(typeof(ApplicationStateEvent))]
			public string EventOnMidpoint;

			// 0600065d (1.10.1 RVA 0x645630)
			public JSONCtorArgs()
			{;}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass4_0
		{
			public IGraphUser user;

			public FadeTransitionParameters fadeParameters;

			public ApplicationStateFadeTransition _003C_003E4__this;

			// 0600065e (1.10.1 RVA 0x6452F0)
			public _003C_003Ec__DisplayClass4_0()
			{;}

			// 0600065f (1.10.1 RVA 0x645640)
			internal void _003CDoOnEnter_003Eb__0()
			{
				Midpoint(user, fadeParameters.OnMidpointCallback);
			}

			// 06000660 (1.10.1 RVA 0x645660)
			internal void _003CDoOnEnter_003Eb__1()
			{
				_003C_003E4__this.PublishMessage(user);
			}
		}

		private readonly ApplicationStateEvent m_eventOnMidpoint;

		// 06000658 (1.10.1 RVA 0x644F10)
		private ApplicationStateFadeTransition(FiniteStateMachine fsm, string stateName, ApplicationStateEvent eventOnMidpoint) : base(fsm, stateName)
		{
			m_eventOnMidpoint = eventOnMidpoint;
		}

		// 06000659 (1.10.1 RVA 0x644F60)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			ApplicationStateEvent eventOnMidpoint = ApplicationStateEvent.FindByName(args.EventOnMidpoint);
			return new ApplicationStateFadeTransition(fsm, (string)stateId, eventOnMidpoint);
		}

		// 0600065a (1.10.1 RVA 0x645020)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			App app = user.GetAs<App>();
			FadeTransitionParameters fadeParameters = app.Storage.GetValue<FadeTransitionParameters>(AppFSMKeys.TransitionFadeParameters, null, true);
			UIScreenTransitionManager transitionManager = ProcessManager.GetSystem<UIScreenTransitionManager>();
			DataManager dataManager = ProcessManager.GetSystem<DataManager>();
			if (dataManager.FadeTransitionDefinitions.TryGetValue(fadeParameters.Type, out FadeTransitionDefinition definition))
			{
				transitionManager.QueueTransition(definition.ContainerIdentifier,
					() => Midpoint(user, fadeParameters.OnMidpointCallback), () => PublishMessage(user));
			}
		}

		// 0600065b (1.10.1 RVA 0x645300)
		private static void Midpoint(IGraphUser user, Action fadeParametersOnMidpointCallback)
		{
			fadeParametersOnMidpointCallback?.Invoke();
			user.Storage.RemoveValue<FadeTransitionParameters>(AppFSMKeys.TransitionFadeParameters);
		}

		// 0600065c (1.10.1 RVA 0x645490)
		private void PublishMessage(IGraphUser user)
		{
			user.Storage.GetValue<List<ApplicationStateEvent>>(AppFSMKeys.StateEvents, null, true).AddUnique(m_eventOnMidpoint);
		}
	}
}
