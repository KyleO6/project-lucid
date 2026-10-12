using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000073; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Actor/{0}")]
	[GraphNodeDefaultName("VelocityMagnitudeThreshold")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ActorTransition_VelocityMagnitudeThreshold : FSMTransition
	{
		[Serializable]
		[FiniteStateMachineJsonCtorArgs]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class VelocityMagnitudeJSONCtorArgs
		{
			public float VelocityMagnitude;

			public ComparisonType ComparisonType;

			// 06000460 (1.10.1 RVA 0x529B30)
			public VelocityMagnitudeJSONCtorArgs()
			{;}
		}

		private readonly float m_velocityMagnitude;

		private readonly ComparisonType m_comparisonType;

		// 0600045d (1.10.1 RVA 0x5299C0)
		protected ActorTransition_VelocityMagnitudeThreshold(FiniteStateMachine fsm, FSMIdentifier transitionId, VelocityMagnitudeJSONCtorArgs ctorArgs) : base(fsm, transitionId)
		{
			m_velocityMagnitude = ctorArgs.VelocityMagnitude;
			m_comparisonType = ctorArgs.ComparisonType;
		}

		// 0600045e (1.10.1 RVA 0x5299F0)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new ActorTransition_VelocityMagnitudeThreshold(fsm, transitionId, JsonUtility.FromJson<VelocityMagnitudeJSONCtorArgs>(jsonCtorArgs));
		}

		// 0600045f (1.10.1 RVA 0x529AA0)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext updateContext)
		{
			return base.DoUpdate(user, updateContext) || m_comparisonType.Evaluate(user.GetAs<Actor>().WorldVelocityMagnitude, m_velocityMagnitude);
		}
	}
}
