using Unity.IL2CPP.CompilerServices;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020002c5; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public abstract class CharacterAbility_Climbing<T> : CharacterAbility<T> where T : CharacterAbilityDefinition_Climbing
	{
		// 060010d0 (1.10.1 RVA 0xE5F8F0)
		public void UpdateVerticalSpeedWithLateralBias(ref float forwardSpeed, float lateralSpeed, float deltaTime)
		{
			CharacterAbilityDefinition_Climbing.ClimbingLateralMotion lateral = Definition.LateralMotion;
			float lateralRatio = Mathf.Clamp01(Mathf.Abs(lateralSpeed) / lateral.SpeedMax);
			float bias = lateral.VerticalLateralSpeedBias.Evaluate(lateralRatio);
			CharacterAbilityDefinition_Climbing.ClimbingVerticalMotion vertical = Definition.VerticalMotion;
			float input = m_character.ControllerMovement.y;
			float targetSpeed = Mathf.Abs(input) < 0.0001f
				? 0f
				: Mathf.Clamp01(bias) * (input < 0f ? vertical.SpeedMaxDownward : vertical.SpeedMaxUpward) * input;
			UpdateSpeedAxis(ref forwardSpeed, targetSpeed, vertical.Acceleration, vertical.Deceleration, deltaTime);
		}

		// 060010d1 (1.10.1 RVA 0xE5FB60)
		private static void UpdateSpeedAxis(ref float speed, float targetSpeed, float acceleration, float deceleration, float deltaTime)
		{
			if (targetSpeed > 0f)
			{
				speed = speed <= targetSpeed ? Mathf.Min(speed + acceleration * deltaTime, targetSpeed) : Mathf.Max(speed - deceleration * deltaTime, targetSpeed);
			}
			else
			{
				speed = speed > targetSpeed ? Mathf.Max(speed - acceleration * deltaTime, targetSpeed) : Mathf.Min(speed + deceleration * deltaTime, targetSpeed);
			}
		}

		// 060010d2 (1.10.1 RVA 0xE5FBD0)
		public void UpdateLateralSpeed(ref float lateralSpeed, float deltaTime)
		{
			CharacterAbilityDefinition_Climbing.ClimbingLateralMotion motion = Definition.LateralMotion;
			float input = m_character.ControllerMovement.x;
			float targetSpeed = Mathf.Abs(input) < 0.0001f ? 0f : input * motion.SpeedMax;
			UpdateSpeedAxis(ref lateralSpeed, targetSpeed, motion.Acceleration, motion.Deceleration, deltaTime);
		}

		// 060010d3 (1.10.1 RVA 0xE5FCF0)
		public bool IsClimbingAngleValid(Vector3 surfaceNormal, Vector3 gravityNormalised)
		{
			return MathUtilities.WithinRangeWithTolerance(-Vector3.Dot(surfaceNormal, gravityNormalised), Definition.MaxClimbAngleCosine, Definition.MinClimbAngleCosine);
		}

		// 060010d4 (1.10.1 RVA 0xE5FE20)
		public void UpdateJumpAngleExtra(IGraphStorage storage, bool remove = false)
		{
			if (remove)
			{
				storage.RemoveValue<float>(ActorFSMKeys.JumpAngleExtra);
			}
			else
			{
				storage.SetValue(ActorFSMKeys.JumpAngleExtra, Definition.JumpAngleExtra);
			}
		}

		// 060010d5 (1.10.1 RVA 0xE60070)
		protected CharacterAbility_Climbing()
		{
		}
	}
}
