using Unity.IL2CPP.CompilerServices;
using System;
using HardlightProject;
using UnityEngine;

namespace Hardlight
{
	// Original Game.Runtime 0200000f; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public sealed class ApplicationTransitionTriggerDreamPowerStoreInfo : FSMTransition
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		private class JSONCtorArgs
		{
			public bool TrueCausesTransition;

			// 06000026 (1.10.1 RVA 0x507A30)
			public JSONCtorArgs()
			{;}
		}

		private readonly SystemRef<DreamPowerStoreManager> m_dreamPowerStoreManager = ProcessManager.GetSystemRef<DreamPowerStoreManager>();

		private readonly SystemRef<SaveManager> m_saveManagerRef = ProcessManager.GetSystemRef<SaveManager>();

		private readonly bool m_trueCausesTransition;

		// 06000023 (1.10.1 RVA 0x507800)
		private ApplicationTransitionTriggerDreamPowerStoreInfo(FiniteStateMachine fsm, FSMIdentifier transitionId, bool trueCausesTransition) : base(fsm, transitionId)
		{
			m_trueCausesTransition = trueCausesTransition;
		}

		// 06000024 (1.10.1 RVA 0x5078E0)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			return new ApplicationTransitionTriggerDreamPowerStoreInfo(fsm, transitionId, args.TrueCausesTransition);
		}

		// 06000025 (1.10.1 RVA 0x507970)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext context)
		{
			if (!m_saveManagerRef.TryGet(out SaveManager saveManager) || !m_dreamPowerStoreManager.TryGet(out DreamPowerStoreManager storeManager))
			{
				return false;
			}
			bool available = saveManager.CurrentSave.DreamPowerStore.IsNewState == DreamPowerStoreIsNewState.IsNew && storeManager.IsAvailable();
			return available == m_trueCausesTransition;
		}
	}
}
