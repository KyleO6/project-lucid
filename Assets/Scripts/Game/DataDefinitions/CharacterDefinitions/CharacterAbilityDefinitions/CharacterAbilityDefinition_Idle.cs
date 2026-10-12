using Unity.IL2CPP.CompilerServices;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020004dd; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "CharacterAbilityDefinition_Idle", menuName = "HardlightProject/DefinitionData/AbilityDef/Idle", order = 1)]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterAbilityDefinition_Idle : CharacterAbilityDefinition
	{
		[Tooltip("Considered stopped when below this velocity for a given number of seconds.")]
		public float MinimumVelocityCheck;

		[Tooltip("Considered stopped when velocity is below threshold for this many seconds.")]
		public float SecondsInMinimumVelocityCheck;

		public override Type ScriptType
		{
			get
			{
				return typeof(CharacterAbility_Idle);
			}
		}

		public float MinimumVelocityCheckSqr { get; private set; }

		// 06001b28 (1.10.1 RVA 0x5438D0)
		public override void CalculateCachedValues()
		{
			base.CalculateCachedValues();
			MinimumVelocityCheckSqr = MinimumVelocityCheck * MinimumVelocityCheck;
		}

		// 06001b29 (1.10.1 RVA 0x543900)
		public CharacterAbilityDefinition_Idle()
		{
		}
	}
}
