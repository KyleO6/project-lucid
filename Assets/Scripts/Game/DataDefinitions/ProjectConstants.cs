using Unity.IL2CPP.CompilerServices;

namespace HardlightProject
{
	// Original Game.Runtime 02000a1c; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public static class ProjectConstants
	{
		public const string AssetMenu = "HardlightProject/";

		public const string ConfigAssetMenu = "HardlightProject/Config/";

		public const string MainMixerMasterVolume = "MasterVolume";

		public const string MainMixerMusicVolume = "MusicVolume";

		public const string MainMixerSfxVolume = "SfxVolume";

		public const string MainMixerAmbientVolume = "AmbientVolume";

		public const string MainMixerVoiceVolume = "VO";

		public const int AudioLogarithmicMultiplier = 20;

		public const int MaxVolume = 1;

		public const float MinVolume = 0.0001f;

		public const int MinPlatformSize = 2;

		public const int MaxPlatformSize = 25;

		public const int MaxPlatformHeight = 25;

		public const float DefaultScreenTransitionDuration = 1f;

		public const string DefaultSaveIdentifier = "HL_Default_Save_ID";

		public const string CloudTestSaveIdentifier = "HL_CloudTest_Save_ID";

		public const string HLPropertyStoreKey = "H4RDl1ght_S4VE";

		public const string HLPropertyStoreFileName = "Primary";

		public const int HLPropertyStoreClientVersion = 0;

		public const string HLPropertyKeyFirstBoot = "HL_First_Boot";
	}
}
