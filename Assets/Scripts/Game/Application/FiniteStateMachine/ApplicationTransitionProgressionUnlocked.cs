using Unity.IL2CPP.CompilerServices;
using System;
using System.Collections.Generic;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 0200018d; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationTransitionProgressionUnlocked : FSMTransition
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class JSONCtorArgs
		{
			public bool TrueWhenUnlocked;

			[GraphEnumPopup(typeof(PlayerProgressionTypes))]
			public List<string> Progressions;

			// 0600088f (1.10.1 RVA 0x6664F0)
			public JSONCtorArgs()
			{;}
		}

		private readonly bool m_trueWhenUnlocked;

		private readonly List<PlayerProgressionTypes> m_progressions;

		// 0600088c (1.10.1 RVA 0x665F50)
		private ApplicationTransitionProgressionUnlocked(FiniteStateMachine fsm, FSMIdentifier transitionId, bool trueWhenUnlocked, List<PlayerProgressionTypes> progressions) : base(fsm, transitionId)
		{
			m_trueWhenUnlocked = trueWhenUnlocked;
			m_progressions = progressions;
		}

		// 0600088d (1.10.1 RVA 0x665F90)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			List<PlayerProgressionTypes> progressions = new List<PlayerProgressionTypes>();
			foreach (string progression in args.Progressions)
			{
				progressions.Add(EnumUtilities.Parse<PlayerProgressionTypes>(progression));
			}
			return new ApplicationTransitionProgressionUnlocked(fsm, transitionId, args.TrueWhenUnlocked, progressions);
		}

		// 0600088e (1.10.1 RVA 0x666200)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext context)
		{
			List<IMetaGameUnlock> unlocks = user.GetAs<App>().Storage.GetValue<List<IMetaGameUnlock>>(AppFSMKeys.MetaGameUnlocks);
			bool unlocked = false;
			foreach (IMetaGameUnlock unlock in unlocks)
			{
				if (m_progressions.Contains(unlock.GetProgressionType()))
				{
					unlocked = true;
					break;
				}
			}
			return m_trueWhenUnlocked ? unlocked : !unlocked;
		}
	}
}
