using Unity.IL2CPP.CompilerServices;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020003db; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeDefaultName("ToHalfPipeAir")]
	[GraphNodeMenuFormat("Character/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterTransition_HalfPipeAir : FSMTransition
	{
		// 0600172c (1.10.1 RVA 0x790800)
		private CharacterTransition_HalfPipeAir(FiniteStateMachine fsm, FSMIdentifier transitionId) : base(fsm, transitionId)
		{
		}

		// 0600172d (1.10.1 RVA 0x790810)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new CharacterTransition_HalfPipeAir(fsm, transitionId);
		}

		// 0600172e (1.10.1 RVA 0x790870)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext updateContext)
		{
			if (base.DoUpdate(user, updateContext))
				return true;
			Character character = user.GetAs<Character>();
			if (character.Collider.LinkedCollisionData != null && character.Collider.LinkedCollisionData.HasHalfPipeIgnoreDefinition)
				return false;
			if (character.Storage.GetValue(ActorFSMKeys.HalfPipeColliderActive, false, true))
				return true;
			if (character.IsDirectionOverrideActive())
				return false;
			RaycastColliderHit current = character.Collider.GroundHit;
			RaycastColliderHit previous = character.Collider.GroundHitLastFrame;
			Vector3 currentNormal = current.Hit ? current.HitNormal().ClampToZero() : Vector3.zero;
			Vector3 previousNormal = previous.Hit ? previous.HitNormal().ClampToZero() : Vector3.zero;
			if (!previous.Hit || !previous.HitIsEdge || current.Hit)
				return false;
			if (current.HitProjected && current.HitIsEdge && Vector3.Dot(current.HitNormal(), previous.HitNormal()) >= 0.9999f)
				return false;
			Vector3 worldUp = character.WorldUp;
			if (Vector3.Dot(character.WorldVelocityNormalised, worldUp) < 0.0001f || Vector3.Dot(currentNormal, worldUp) < -0.0001f)
				return false;
			character.Storage.SetValue(ActorFSMKeys.HalfPipeColliderNormal, previousNormal);
			character.Storage.SetValue(ActorFSMKeys.HalfPipeColliderActive, true);
			return true;
		}
	}
}
