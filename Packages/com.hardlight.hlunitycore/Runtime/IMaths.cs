
namespace Hardlight
{
	// Original HLUnityCore.Runtime 0200017f; bodies translated from the 1.10.1 x86-64 binary.
	public interface IMaths<T>
	{
		T Add(T left, T right);

		T Subtract(T left, T right);

		T Multiply(T left, T right);

		T Divide(T left, T right);

		bool GreaterThan(T left, T right);

		bool GreaterThanOrEqual(T left, T right);

		bool LessThan(T left, T right);

		bool LessThanOrEqual(T left, T right);
	}
}
