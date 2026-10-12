using Unity.IL2CPP.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 0200069a; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public static class FSMStorageKeys
	{
		// Static constructor 0600236a (1.10.1 RVA 0x5873E0) builds each key as GraphStorageKey(name, 0, 0) in declaration order.
		public static readonly GraphStorageKey CameraOverride = new GraphStorageKey("CameraOverride", 0, 0);

		public static readonly GraphStorageKey TimeScalesActive = new GraphStorageKey("TimeScalesActive", 0, 0);

		public static readonly GraphStorageKey ShaderOverride = new GraphStorageKey("ShaderOverride", 0, 0);
	}
}
