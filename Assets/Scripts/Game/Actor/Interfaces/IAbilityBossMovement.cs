using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 0200007b; bodies translated from the 1.10.1 x86-64 binary.
	public interface IAbilityBossMovement
	{
		Vector3 CurrentTargetPosition
		{
			get;
		}

		float MovementAnimationAngleMultiplier
		{
			get;
		}

		float FaceAngleTolerance
		{
			get;
		}

		float TurnSpeed
		{
			get;
		}

		bool IsMomentum
		{
			get;
		}

		bool TurnToCharacter
		{
			get;
		}
	}
}
