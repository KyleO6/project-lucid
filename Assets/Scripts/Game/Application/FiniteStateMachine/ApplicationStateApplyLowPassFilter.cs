using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020000bf; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateApplyLowPassFilter : FSMState
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		public class JSONCtorArgs
		{
			public InGameAudioListener.LowFilterType FilterToControl;

			public bool SetActive;

			// 0600063a (1.10.1 RVA 0x53C090)
			public JSONCtorArgs()
			{;}
		}

		private InGameAudioListener.LowFilterType m_filter;

		private bool m_setActive;

		// 06000637 (1.10.1 RVA 0x53BE90)
		public ApplicationStateApplyLowPassFilter(FiniteStateMachine fsm, string stateName, JSONCtorArgs ctorArgs) : base(fsm, stateName)
		{
			m_filter = ctorArgs.FilterToControl;
			m_setActive = ctorArgs.SetActive;
		}

		// 06000638 (1.10.1 RVA 0x53BEE0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			return new ApplicationStateApplyLowPassFilter(fsm, stateId, args);
		}

		// 06000639 (1.10.1 RVA 0x53BFA0)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			AudioManager audioManager = ProcessManager.GetSystemSafe<AudioManager>();
			if (audioManager == null)
			{
				return;
			}
			if (m_setActive)
			{
				audioManager.ActivateLowPass(m_filter);
			}
			else
			{
				audioManager.RemoveLowPassFilter(m_filter);
			}
		}
	}
}
