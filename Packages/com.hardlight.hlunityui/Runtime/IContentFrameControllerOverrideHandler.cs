using System;

// Original HLUnityUI.Runtime 02000008; bodies translated from the 1.10.1 x86-64 binary.
public interface IContentFrameControllerOverrideHandler
{
	event Action<bool> OnApply;

	bool IsOverrideActive();
}
