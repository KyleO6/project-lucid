using System;

namespace Hardlight
{
	// Original HLUnityCore.Runtime 02000255; bodies translated from the 1.10.1 x86-64 binary.
	public interface IUpdateSubscriber
	{
		void OnUpdate(float delta);
	}
}
