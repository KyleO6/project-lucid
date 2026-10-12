using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020003f1; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeDefaultName("LastHomingTargetType")]
	[GraphNodeMenuFormat("Character/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterTransition_LastHomingTargetType : FSMTransition
	{
		[Serializable]
		[FiniteStateMachineJsonCtorArgs]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class HitLayerJSONCtorArgs
		{
			public HomingTargetType TargetType;

			public bool InverseCheck;

			// 06001768 (1.10.1 RVA 0x7928B0)
			public HitLayerJSONCtorArgs()
			{;}
		}

		private readonly HomingTargetType m_targetType;

		private readonly bool m_inverseCheck;

		// 06001765 (1.10.1 RVA 0x792680)
		protected CharacterTransition_LastHomingTargetType(FiniteStateMachine fsm, FSMIdentifier transitionId, HitLayerJSONCtorArgs ctorArgs) : base(fsm, transitionId)
		{
			m_targetType = ctorArgs.TargetType;
			m_inverseCheck = ctorArgs.InverseCheck;
		}

		// 06001766 (1.10.1 RVA 0x7926B0)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new CharacterTransition_LastHomingTargetType(fsm, transitionId, JsonUtility.FromJson<HitLayerJSONCtorArgs>(jsonCtorArgs));
		}

		// 06001767 (1.10.1 RVA 0x792750)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext updateContext)
		{
			if (base.DoUpdate(user, updateContext))
				return true;

			bool matches = user.GetAs<Character>().Storage.GetValue(ActorFSMKeys.HomingTargetType, default(HomingTargetType), true) == m_targetType;
			return m_inverseCheck ? !matches : matches;
		}
	}
}
