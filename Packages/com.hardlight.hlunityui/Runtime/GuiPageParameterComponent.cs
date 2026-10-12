using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Hardlight
{
	// Original HLUnityUI.Runtime 02000034; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public abstract class GuiPageParameterComponent : MonoBehaviour
	{
		protected GuiPageParameters m_pageParameters;

		public virtual GuiPageParameters PageParameters
		{
			get
			{return m_pageParameters;}
		}

		// 06000179 (1.10.1 RVA 0x1B446C0)
		protected GuiPageParameterComponent()
		{
		}
	}
}
