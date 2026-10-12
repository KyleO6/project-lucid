using System;

namespace Hardlight
{
	// Original HLUnityCore.Runtime 020001e3; bodies translated from the 1.10.1 x86-64 binary.
	public interface IObservable<out T>
	{
		event Action<T> OnValueUpdated;
	}
}
