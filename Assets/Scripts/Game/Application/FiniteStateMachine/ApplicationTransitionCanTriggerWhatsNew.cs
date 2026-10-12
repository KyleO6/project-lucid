using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight.Utils;
using HardlightProject;
using UnityEngine;

namespace Hardlight
{
	// Original Game.Runtime 0200000c; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public sealed class ApplicationTransitionCanTriggerWhatsNew : FSMTransition
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		private class JSONCtorArgs
		{
			public bool TrueCausesTransition;

			// 0600001f (1.10.1 RVA 0x507380)
			public JSONCtorArgs()
			{;}
		}

		private readonly SystemRef<SaveManager> m_saveManagerRef = ProcessManager.GetSystemRef<SaveManager>();

		private readonly SystemRef<DataManager> m_dataManagerRef = ProcessManager.GetSystemRef<DataManager>();

		private readonly bool m_trueCausesTransition;

		// 0600001c (1.10.1 RVA 0x507040)
		private ApplicationTransitionCanTriggerWhatsNew(FiniteStateMachine fsm, FSMIdentifier transitionId, bool trueCausesTransition) : base(fsm, transitionId)
		{
			m_trueCausesTransition = trueCausesTransition;
		}

		// 0600001d (1.10.1 RVA 0x507120)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			return new ApplicationTransitionCanTriggerWhatsNew(fsm, transitionId, args.TrueCausesTransition);
		}

		// 0600001e (1.10.1 RVA 0x5071B0)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext context)
		{
			if (!m_saveManagerRef.TryGet(out SaveManager saveManager) || !m_dataManagerRef.TryGet(out DataManager dataManager))
			{
				return false;
			}
			bool canTrigger = false;
			if (dataManager.TryGetSubversionOfOrLast(Application.version, out ApplicationVersionDefinition versionDefinition)
				&& !(versionDefinition.WhatsNewContainerIdentifier == null)
				&& versionDefinition.MeetsAllRequirements())
			{
				string gameVersion = saveManager.CurrentSave.GameVersion;
				canTrigger = string.IsNullOrWhiteSpace(gameVersion) || new Hardlight.Utils.Version(gameVersion) < versionDefinition.Version;
			}
			return canTrigger == m_trueCausesTransition;
		}
	}
}
