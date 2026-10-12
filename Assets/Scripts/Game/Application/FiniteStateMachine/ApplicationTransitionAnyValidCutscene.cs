using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000163; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationTransitionAnyValidCutscene : FSMTransition
	{
		public enum Comparison
		{
			TrueCausesTransition,
			FalseCausesTransition
		}

		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		private class JSONCtorArgs
		{
			[GraphEnumPopup(typeof(CutsceneTriggerType))]
			public string TriggerType;

			[GraphEnumPopup(typeof(Comparison))]
			public string Compare;

			public bool OnlyCheckQueue;

			// 06000830 (1.10.1 RVA 0x662490)
			public JSONCtorArgs()
			{;}
		}

		private readonly CutsceneTriggerType m_triggerType;

		private readonly Comparison m_compare;

		private readonly bool m_onlyCheckQueue;

		private readonly SystemRef<CutsceneManager> m_cutsceneManagerRef = ProcessManager.GetSystemRef<CutsceneManager>();

		private readonly SystemRef<SaveManager> m_saveManagerRef = ProcessManager.GetSystemRef<SaveManager>();

		// 0600082c (1.10.1 RVA 0x661F30)
		private ApplicationTransitionAnyValidCutscene(FiniteStateMachine fsm, FSMIdentifier transitionId, CutsceneTriggerType triggerType, Comparison compare, bool onlyCheckQueue) : base(fsm, transitionId)
		{
			m_triggerType = triggerType;
			m_compare = compare;
			m_onlyCheckQueue = onlyCheckQueue;
		}

		// 0600082d (1.10.1 RVA 0x662020)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			return new ApplicationTransitionAnyValidCutscene(fsm, transitionId, (CutsceneTriggerType)Enum.Parse(typeof(CutsceneTriggerType), args.TriggerType, true), (Comparison)Enum.Parse(typeof(Comparison), args.Compare, true), args.OnlyCheckQueue);
		}

		// 0600082e (1.10.1 RVA 0x662200)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext context)
		{
			return (m_compare != Comparison.TrueCausesTransition) != AnyValidCutscene(user);
		}

		// 0600082f (1.10.1 RVA 0x662220)
		private bool AnyValidCutscene(IGraphUser user)
		{
			if (m_cutsceneManagerRef.IsNull())
			{
				return false;
			}
			if (m_saveManagerRef.TryGet(out SaveManager saveManager) && !saveManager.IsAnySaveOpen)
			{
				return false;
			}
			CutsceneManager cutsceneManager = m_cutsceneManagerRef.Get();
			CutsceneDefinition cutscene = user.Storage.GetValue<CutsceneDefinition>(AppFSMKeys.CutsceneQueued, null, false);
			if (!m_onlyCheckQueue && cutscene == null)
			{
				cutsceneManager.HasAnyValidCutscene(m_triggerType, out cutscene);
			}
			if (cutscene == null)
			{
				return false;
			}
			cutsceneManager.QueueCutscene(cutscene);
			return true;
		}
	}
}
