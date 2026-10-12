using Unity.IL2CPP.CompilerServices;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 0200011f; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateSaveSettings : FSMState
	{
		// 06000750 (1.10.1 RVA 0x653F30)
		public ApplicationStateSaveSettings(FiniteStateMachine fsm, string stateName) : base(fsm, stateName)
		{;}

		// 06000751 (1.10.1 RVA 0x653F60)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new ApplicationStateSaveSettings(fsm, transitionId);
		}

		// 06000752 (1.10.1 RVA 0x653FE0)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			SaveDataSettings settings = user.Storage.GetValue<SaveDataSettings>(AppFSMKeys.GameSettings);
			if (settings == null)
			{
				HLOutput.LogError("ApplicationStateSaveSettings needs settings stored in the application storage to work.");
				return;
			}
			SaveManager saveManager = ProcessManager.GetSystem<SaveManager>();
			saveManager.GetSaveDataSettings().CopyFrom(settings);
			saveManager.RequestSaveSettings();
			user.Storage.RemoveValue<SaveDataSettings>(AppFSMKeys.GameSettings);
		}
	}
}
