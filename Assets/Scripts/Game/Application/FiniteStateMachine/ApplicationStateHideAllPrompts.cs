using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020000d5; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateHideAllPrompts : FSMState
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class JSONCtorArgs
		{
			public bool SuppressPrompts;

			// 06000673 (1.10.1 RVA 0x646CF0)
			public JSONCtorArgs()
			{;}
		}

		private readonly SystemRef<PromptManager> m_promptManagerRef = ProcessManager.GetSystemRef<PromptManager>();

		private readonly bool m_suppressPrompts;

		// 0600066e (1.10.1 RVA 0x646A60)
		private ApplicationStateHideAllPrompts(FiniteStateMachine fsm, string stateName, bool suppressPrompts) : base(fsm, stateName)
		{

			m_suppressPrompts = suppressPrompts;
		}

		// 0600066f (1.10.1 RVA 0x646B10)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			return new ApplicationStateHideAllPrompts(fsm, (string)stateId, args.SuppressPrompts);
		}

		// 06000670 (1.10.1 RVA 0x646BB0)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			DoHide();
		}

		// 06000671 (1.10.1 RVA 0x646C80)
		protected override void DoUpdate(IGraphUser user, FSMUpdateContext context)
		{
			base.DoUpdate(user, context);
			if (m_suppressPrompts) DoHide();
		}

		// 06000672 (1.10.1 RVA 0x646C20)
		private void DoHide()
		{
			if (m_promptManagerRef.TryGet(out PromptManager manager)) manager.HideAllActivePrompts();
		}
	}
}
