using System;
using System.Runtime.CompilerServices;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 02000945; bodies translated from the 1.10.1 x86-64 binary.
	public struct UIContainerSoundLevelsOptionsParameters : IUIContainerParameters
	{
		public readonly float MasterVolume { get; }

		public readonly float MusicVolume { get; }

		public readonly float SfxVolume { get; }

		public readonly float VoiceVolume { get; }

		public readonly Action OnSaveClick { get; }

		public readonly Action<float> OnMasterVolumeToggle { get; }

		public readonly Action<float> OnMusicVolumeToggle { get; }

		public readonly Action<float> OnSfxVolumeToggle { get; }

		public readonly Action<float> OnVoiceVolumeToggle { get; }

		// 06003548 (1.10.1 RVA 0x67F600)
		public UIContainerSoundLevelsOptionsParameters(float masterVolume, float musicVolume, float sfxVolume, float voiceVolume, Action onSaveClick, Action<float> onMasterVolumeToggle, Action<float> onMusicVolumeToggle, Action<float> onSfxVolumeToggle, Action<float> onVoiceVolumeToggle)
		{
			MasterVolume = masterVolume;
			MusicVolume = musicVolume;
			SfxVolume = sfxVolume;
			VoiceVolume = voiceVolume;
			OnSaveClick = onSaveClick;
			OnMasterVolumeToggle = onMasterVolumeToggle;
			OnMusicVolumeToggle = onMusicVolumeToggle;
			OnSfxVolumeToggle = onSfxVolumeToggle;
			OnVoiceVolumeToggle = onVoiceVolumeToggle;
		}
	}
}
