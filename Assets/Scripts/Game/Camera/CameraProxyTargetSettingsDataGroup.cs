using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020004a3; bodies translated from the 1.10.1 x86-64 binary.
	[CreateAssetMenu(fileName = "CameraProxyTargetSettingsDataGroup", menuName = "HardlightProject/DefinitionData/Groups/CameraProxyTargetSettingsDataGroup")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CameraProxyTargetSettingsDataGroup : DefinitionDataType<string, CameraProxyTargetSettingsData>
	{
		// 06001a88 (1.10.1 RVA 0x53FF00)
		protected override string GetElementKey(CameraProxyTargetSettingsData data)
		{
			return data.name;
		}

		// 06001a89 (1.10.1 RVA 0x53FF10)
		protected override IEqualityComparer<string> GetKeyComparer()
		{
			return null;
		}

		// 06001a8a (1.10.1 RVA 0x53FF20)
		public CameraProxyTargetSettingsDataGroup()
		{
			
		}
	}
}
