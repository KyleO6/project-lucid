using Unity.IL2CPP.CompilerServices;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020003af; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Character/Trigger/{0}")]
	[GraphNodeDefaultName("TriggerSpinDashChargeDisable")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterState_TriggerSpinDashChargeDisable : FSMState_Trigger
	{
		// 06001663 (1.10.1 RVA 0x7854B0)
		protected CharacterState_TriggerSpinDashChargeDisable(FiniteStateMachine fsm, FSMIdentifier stateId, TriggerJSONCtorArgs ctorArgs) : base(fsm, stateId, ctorArgs)
		{
			
		}

		// 06001664 (1.10.1 RVA 0x7854C0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new CharacterState_TriggerSpinDashChargeDisable(fsm, transitionId, JsonUtility.FromJson<TriggerJSONCtorArgs>(jsonCtorArgs));
		}

		// 06001665 (1.10.1 RVA 0x785550)
		protected override void InvokeTriggerStart(IGraphUser user, FSMStateChangeAction action)
		{
			user.GetAs<Character>().SpinDashRoll.DisableCharge();
		}
	}
}
