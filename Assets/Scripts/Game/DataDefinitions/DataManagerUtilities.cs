using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020009fa; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public static class DataManagerUtilities
	{
		// 06003951 (1.10.1 RVA 0x8FD770)
		private static bool TryGetDefinition<T>(Dictionary<string, T> definitions, string definitionName, out T definition, string errorContext = "") where T : ScriptableObject
		{
			definition = null;
			if (string.IsNullOrEmpty(definitionName))
			{
				return false;
			}
			definitions.TryGetValue(definitionName, out definition);
			return true;
		}

		// 06003952 (1.10.1 RVA 0x6AFAE0)
		public static bool TryGetAbilityDefinition(string definitionName, out AbilityDefinition definition, string errorContext = "")
		{
			return TryGetDefinition(ProcessManager.GetSystemRef<App>().Get().DataManager.AbilityDefinitions, definitionName, out definition, errorContext);
		}

		// 06003953 (1.10.1 RVA 0x6AFBB0)
		public static bool TryGetTimeScaleDefinition(string definitionName, out TimeScaleDefinition definition, string errorContext = "")
		{
			return TryGetDefinition(ProcessManager.GetSystemRef<App>().Get().DataManager.TimeScaleDefinitions, definitionName, out definition, errorContext);
		}

		// 06003954 (1.10.1 RVA 0x6AFC80)
		public static bool TryGetStickyControlsDefinition(string definitionName, out CharacterStickyControlsDefinition definition, string errorContext = "")
		{
			return TryGetDefinition(ProcessManager.GetSystemRef<App>().Get().DataManager.CharacterStickyControlsDefinitions, definitionName, out definition, errorContext);
		}
	}
}
