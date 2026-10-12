using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020005a8; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "RequirementLevelAlwaysLocked", menuName = "HardlightProject/Requirements/RequirementLevelAlwaysLocked")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public sealed class RequirementLevelAlwaysLocked : RequirementGameplayLevelBase
	{
		// 06001efc (1.10.1 RVA 0x556950)
		public override bool RequirementsMet(GameplayLevelDefinition levelDefinition)
		{return default;}

		// 06001efd (1.10.1 RVA 0x556960)
		public RequirementLevelAlwaysLocked()
		{
		}
	}
}
