using Unity.IL2CPP.CompilerServices;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000a20; bodies translated from the 1.10.1 x86-64 binary.
	[Serializable]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class RuntimeType : ISerializationCallbackReceiver
	{
		[SerializeField]
		[HideInInspector]
		private string m_targetAssemblyQualifiedName;

		public Type Type { get; private set; }

		// 060039fa (1.10.1 RVA 0x6B3B00)
		public void OnBeforeSerialize()
		{
		}

		// 060039fb (1.10.1 RVA 0x6B3B10)
		public void OnAfterDeserialize()
		{
			Type = System.Type.GetType(m_targetAssemblyQualifiedName);
		}

		// 060039fc (1.10.1 RVA 0x6B3BA0)
		public RuntimeType()
		{
		}
	}
}
