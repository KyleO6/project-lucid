using Unity.IL2CPP.CompilerServices;
using Hardlight;
using UnityEngine.Device;

namespace HardlightProject
{
	// Original Game.Runtime 020000fd; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateOpenWhatsNew : FSMState
	{
		private readonly SystemRef<UIManager> m_uiManagerRef = ProcessManager.GetSystemRef<UIManager>();

		private readonly SystemRef<DataManager> m_dataManagerRef = ProcessManager.GetSystemRef<DataManager>();

		private readonly SystemRef<SaveManager> m_saveManagerRef = ProcessManager.GetSystemRef<SaveManager>();

		// 060006f0 (1.10.1 RVA 0x64E4D0)
		private ApplicationStateOpenWhatsNew(FiniteStateMachine fsm, string stateName) : base(fsm, stateName)
		{
		}

		// 060006f1 (1.10.1 RVA 0x64E5E0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			return new ApplicationStateOpenWhatsNew(fsm, stateId);
		}

		// 060006f2 (1.10.1 RVA 0x64E650)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			if (!m_dataManagerRef.Get().TryGetSubversionOfOrLast(Application.version, out ApplicationVersionDefinition versionDefinition)
				|| versionDefinition.WhatsNewContainerIdentifier == null)
			{
				return;
			}
			m_uiManagerRef.Get().GetOrCreate(versionDefinition.WhatsNewContainerIdentifier);
			SaveManager saveManager = m_saveManagerRef.Get();
			saveManager.GetSaveDataSettings().LastWhatsNewVersion = Application.version;
			saveManager.RequestSaveSettings();
		}
	}
}
