using Unity.IL2CPP.CompilerServices;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 0200068d; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeDefaultName("FSMState_TriggerCutscene")]
	[GraphNodeMenuFormat("Generic/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class FSMState_TriggerCutscene : FSMState_Trigger
	{
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class JSONCtorArgs : TriggerJSONCtorArgs
		{
			[GraphEnumPopup(typeof(CutsceneTriggerType))]
			public string CutsceneTriggerType;

			public bool PreloadOnly;

			// 06002347 (1.10.1 RVA 0x585BF0)
			public JSONCtorArgs()
			{;}
		}

		private readonly CutsceneTriggerType m_cutsceneTriggerType;

		private bool m_preloadOnly;

		// 06002344 (1.10.1 RVA 0x5856E0); the trigger base constructor is inlined natively.
		protected FSMState_TriggerCutscene(FiniteStateMachine fsm, FSMIdentifier stateId, JSONCtorArgs ctorArgs, CutsceneTriggerType cutsceneTriggerType, bool preloadOnly) : base(fsm, stateId, ctorArgs)
		{
			m_cutsceneTriggerType = cutsceneTriggerType;
			m_preloadOnly = preloadOnly;
		}

		// 06002345 (1.10.1 RVA 0x585720); the trigger type is parsed before allocation.
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			return new FSMState_TriggerCutscene(fsm, transitionId, args, EnumUtilities.Parse<CutsceneTriggerType>(args.CutsceneTriggerType), args.PreloadOnly);
		}

		// 06002346 (1.10.1 RVA 0x5857F0). A cutscene queued in the user's storage is
		// loaded, played unless preloading, and consumed; otherwise any valid cutscene
		// for the trigger type is preloaded or played.
		protected override void InvokeTriggerStart(IGraphUser user, FSMStateChangeAction action)
		{
			CutsceneManager cutsceneManager = ProcessManager.GetSystem<CutsceneManager>();
			if (user.Storage.HasValue<CutsceneDefinition>(AppFSMKeys.CutsceneQueued))
			{
				CutsceneDefinition cutscene = user.Storage.GetValue<CutsceneDefinition>(AppFSMKeys.CutsceneQueued, null, true);
				cutsceneManager.LoadCutscene(cutscene, null, !m_preloadOnly);
				user.Storage.RemoveValue<CutsceneDefinition>(AppFSMKeys.CutsceneQueued);
			}
			else if (m_preloadOnly)
			{
				cutsceneManager.PreloadAnyValidCutscene(m_cutsceneTriggerType, null);
			}
			else
			{
				cutsceneManager.PlayAnyValidCutscene(m_cutsceneTriggerType, null);
			}
		}
	}
}
