using System;

namespace Hardlight
{
	// Original HLUnityCore.Runtime 020001e7; bodies translated from the 1.10.1 x86-64 binary.
	public interface IPayload<in TValue>
	{
		string GetIdentifier();

		void UpdateData(TValue newData);
	}
}
