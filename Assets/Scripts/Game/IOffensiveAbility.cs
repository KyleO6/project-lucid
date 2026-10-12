
namespace HardlightProject
{
	// Original Game.Runtime 02000082; bodies translated from the 1.10.1 x86-64 binary.
	public interface IOffensiveAbility
	{
		bool StartWindUp
		{
			get;
		}

		bool WoundUp
		{
			get;
		}

		bool CanAttack(Boss boss, Character character, ActorAnimationDefinition actorAnimationDefinition, float deltaTime);

		void SetAttackFlow(bool abilityActive);

		void SetAttackWindingUp();

		void SetAttackWoundUp();

		void SetAttackAnimStarted();

		void SkipAttackPreparation(ActorAnimationDefinition actorAnimationDefinition);

		void ResetProjectileGroupCounter();
	}
}
