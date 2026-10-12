using System;

namespace Hardlight
{
	// Original HLUnityCore.Runtime 020001e8; bodies translated from the 1.10.1 x86-64 binary.
	public interface IPayloadReader<in TInput, TOutput>
	{
		Type GetPayloadType();

		bool GetKeyValueFromData(TInput input, out string identifier, out TOutput output);
	}
}
