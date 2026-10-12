using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using Hardlight.Utils;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000479; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "ApplicationVersionDefinitionGroup", menuName = "HardlightProject/DefinitionData/Groups/ApplicationVersionDefinitionGroup", order = 1)]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationVersionDefinitionGroup : DefinitionDataType<string, ApplicationVersionDefinition>
	{
		// 060019f6 (1.10.1 RVA 0x53DBC0)
		protected override string GetElementKey(ApplicationVersionDefinition data)
		{
			return data.Version.ToString();
		}

		// 060019f7 (1.10.1 RVA 0x53DC30)
		protected override IEqualityComparer<string> GetKeyComparer()
		{
			return null;
		}

		// 060019f8 (1.10.1 RVA 0x53DC40)
		public ApplicationVersionDefinitionGroup()
		{
			
		}
	}
}
