using Unity.IL2CPP.CompilerServices;
using System;
using System.Collections.Generic;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020003e2; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeDefaultName("ToHomingAttack")]
	[GraphNodeMenuFormat("Character/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterTransition_HomingAttack : CharacterTransition_Homing
	{
		[Serializable]
		[FiniteStateMachineJsonCtorArgs]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class HomingTransitionJSONCtorArgs
		{
			public List<HomingTargetType> HomingTargetTypes;

			// 06001742 (1.10.1 RVA 0x7917B0)
			public HomingTransitionJSONCtorArgs()
			{
			}
		}

		private readonly List<HomingTargetType> m_homingTargetTypes;

		// 0600173f (1.10.1 RVA 0x791590)
		protected CharacterTransition_HomingAttack(FiniteStateMachine fsm, FSMIdentifier transitionId, HomingTransitionJSONCtorArgs ctorArgs) : base(fsm, transitionId)
		{
			m_homingTargetTypes = ctorArgs.HomingTargetTypes;
		}

		// 06001740 (1.10.1 RVA 0x7915C0)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new CharacterTransition_HomingAttack(fsm, transitionId, JsonUtility.FromJson<HomingTransitionJSONCtorArgs>(jsonCtorArgs));
		}

		// 06001741 (1.10.1 RVA 0x791660)
		protected override bool TargetIsValid(CharacterTargetObjectData targetObject)
		{
			return targetObject.Target != null && m_homingTargetTypes.Exists(type => type == targetObject.Type);
		}
	}
}
