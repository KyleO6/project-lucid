using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 0200017b; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationTransitionIsCharacterDying : FSMTransition
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class JSONCtorArgs
		{
			public bool TrueCausesTransition;

			// 06000866 (1.10.1 RVA 0x664690)
			public JSONCtorArgs()
			{;}
		}

		private readonly bool m_trueCausesTransition;

		private readonly SystemRef<CharacterManager> m_characterManagerRef = ProcessManager.GetSystemRef<CharacterManager>();

		// 06000863 (1.10.1 RVA 0x664430)
		private ApplicationTransitionIsCharacterDying(FiniteStateMachine fsm, FSMIdentifier transitionId, bool trueCausesTransition) : base(fsm, transitionId)
		{
			m_trueCausesTransition = trueCausesTransition;
		}

		// 06000864 (1.10.1 RVA 0x6644E0)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			return new ApplicationTransitionIsCharacterDying(fsm, transitionId, args.TrueCausesTransition);
		}

		// 06000865 (1.10.1 RVA 0x6645E0)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext context)
		{
			if (!m_characterManagerRef.TryGet(out CharacterManager characterManager) ||
				!characterManager.TryGetCurrentCharacter(out Character character) || character.IsExitingLevel())
			{
				return false;
			}
			return character.DyingIsInProgress() == m_trueCausesTransition;
		}
	}
}
