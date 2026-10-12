using Unity.IL2CPP.CompilerServices;
using System;
using System.Collections.Generic;
using Hardlight;

namespace HardlightProject
{
	// Original Game.Runtime 020002e6; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterAbility_MovementTransporter : CharacterAbility_MovementTracker<CharacterAbilityDefinition_MovementTransporter, TransporterDefinition>
	{
		private readonly AudioManager m_audioManager = ProcessManager.GetSystem<AudioManager>();

		// 060011b9 (1.10.1 RVA 0x74AF10)
		protected override TransporterDefinition GetTrackerDefinition()
		{
			return DataManager.TransporterDefinitions[TransporterType.Default];
		}

		// 060011ba (1.10.1 RVA 0x74AF80)
		protected override void DoOnEnter()
		{
			m_audioManager.ActivateLowPass(InGameAudioListener.LowFilterType.Transporter);
			base.DoOnEnter();
		}

		// 060011bb (1.10.1 RVA 0x74AFD0)
		protected override void DoOnLeave()
		{
			m_audioManager.RemoveLowPassFilter(InGameAudioListener.LowFilterType.Transporter);
			base.DoOnLeave();
		}

		// 060011bc (1.10.1 RVA 0x74B020)
		public CharacterAbility_MovementTransporter()
		{
		}
	}
}
