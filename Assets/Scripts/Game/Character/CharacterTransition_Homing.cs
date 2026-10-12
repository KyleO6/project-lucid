using Unity.IL2CPP.CompilerServices;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020003e1; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Character/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public abstract class CharacterTransition_Homing : FSMTransition
	{
		// 0600173c (1.10.1 RVA 0x78F710)
		protected CharacterTransition_Homing(FiniteStateMachine fsm, FSMIdentifier transitionId) : base(fsm, transitionId)
		{
		}

		// 0600173d (1.10.1 RVA 0x7914D0)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext updateContext)
		{
			if (base.DoUpdate(user, updateContext))
			{
				return true;
			}
			Character character = user.GetAs<Character>();
			return character.HomingPool.IsTargetActive() && TargetIsValid(character.HomingPool.ActiveTarget.Object);
		}

		// 0600173e (1.10.1 RVA 0x78F7F0)
		protected virtual bool TargetIsValid(CharacterTargetObjectData targetObject)
		{
			return targetObject.Target != null;
		}
	}
}
