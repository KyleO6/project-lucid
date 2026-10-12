using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020000c6; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateCharacterInterruptDyingState : FSMState
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class JSONCtorArgs
		{
			public bool Remove;

			// 0600064b (1.10.1 RVA 0x6444F0)
			public JSONCtorArgs()
			{;}
		}

		private readonly bool m_remove;

		private readonly SystemRef<CharacterManager> m_characterManagerRef = ProcessManager.GetSystemRef<CharacterManager>();

		// 06000648 (1.10.1 RVA 0x644180)
		private ApplicationStateCharacterInterruptDyingState(FiniteStateMachine fsm, string stateName, bool remove) : base(fsm, stateName)
		{

			m_remove = remove;
		}

		// 06000649 (1.10.1 RVA 0x644230)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			return new ApplicationStateCharacterInterruptDyingState(fsm, (string)stateId, args.Remove);
		}

		// 0600064a (1.10.1 RVA 0x6442D0)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			if (m_characterManagerRef.Get().TryGetCurrentCharacter(out Character character))
			{
				if (m_remove) character.Storage.RemoveValue<bool>(ActorFSMKeys.InterruptDyingState);
				else character.Storage.SetValue(ActorFSMKeys.InterruptDyingState, true);
			}
		}
	}
}
