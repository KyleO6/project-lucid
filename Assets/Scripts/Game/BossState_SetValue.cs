using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 0200021f; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public abstract class BossState_SetValue : ActorState_Effect
	{
		[Serializable]
		[FiniteStateMachineJsonCtorArgs]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class SettableJSONCtorArgs : BaseJSONCtorArgs
		{
			public bool SetOnEntry;

			public bool SetOnLeave;

			// 06000c0f (1.10.1 RVA 0x709E60)
			public SettableJSONCtorArgs()
			{;}
		}

		protected bool m_setOnEntry;

		protected bool m_setOnLeave;

		// 06000c0e (1.10.1 RVA 0x709C20)
		protected BossState_SetValue(FiniteStateMachine fsm, FSMIdentifier stateId, SettableJSONCtorArgs ctorArgs) : base(fsm, stateId, ctorArgs)
		{
			m_setOnEntry = ctorArgs.SetOnEntry;
			m_setOnLeave = ctorArgs.SetOnLeave;
		}
	}
}
