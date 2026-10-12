
namespace HardlightProject
{
	// Original Game.Runtime 0200007e; bodies translated from the 1.10.1 x86-64 binary.
	public interface IAbilityOneShot
	{
		bool CanTriggerBefore(float timeElapsed);

		bool CanTriggerAfter(float timeElapsed);
	}
}
