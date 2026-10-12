using Unity.IL2CPP.CompilerServices;
using Hardlight.UI.Binding;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000604; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ViewCameraInformation : MonoBehaviour
	{
		public readonly Bindable<string> CameraInfo = new Bindable<string>();

		// 06002056 (1.10.1 RVA 0x563DD0)
		public ViewCameraInformation()
		{
		}
	}
}
