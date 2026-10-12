
namespace HardlightProject
{
	// Original Game.Runtime 02000875; bodies translated from the 1.10.1 x86-64 binary.
	public interface IUIContainerProgress
	{
		void SetEnabled(bool enabled);

		void SetProgress(float progress);

		void SetProgress(int current, int total);
	}
}
