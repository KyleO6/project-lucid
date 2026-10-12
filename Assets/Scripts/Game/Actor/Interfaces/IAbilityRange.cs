
namespace HardlightProject
{
	// Original Game.Runtime 0200007f; bodies translated from the 1.10.1 x86-64 binary.
	public interface IAbilityRange
	{
		float MaxRangeSquared
		{
			get;
		}

		float MinRangeSquared
		{
			get;
		}

		float DestinationReachedToleranceSquared
		{
			get;
		}

		float MaxFollowUpRangeSquared
		{
			get;
		}
	}
}
