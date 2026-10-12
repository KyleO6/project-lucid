using Unity.IL2CPP.CompilerServices;
using System;
using System.Collections.Generic;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020000f8; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateOpenProgressionContainer : FSMState
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		private class JSONCtorArgs
		{
			[GraphUnityObject(typeof(UIContainerIdentifier))]
			public string Container;

			[GraphEnumPopup(typeof(PlayerProgressionTypes))]
			public List<string> Progressions;

			// 060006e7 (1.10.1 RVA 0x64D710)
			public JSONCtorArgs()
			{;}
		}

		private readonly UIContainerIdentifier m_containerIdentifier;

		private readonly List<PlayerProgressionTypes> m_progressions;

		// 060006e3 (1.10.1 RVA 0x64CDA0)
		private ApplicationStateOpenProgressionContainer(FiniteStateMachine fsm, string stateName, UIContainerIdentifier containerIdentifier, List<PlayerProgressionTypes> progressions) : base(fsm, stateName)
		{
			m_containerIdentifier = containerIdentifier;
			m_progressions = progressions;
		}

		// 060006e4 (1.10.1 RVA 0x64CE10)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			UIContainerIdentifier container = UIContainerIdentifier.FindByName(args.Container);
			List<PlayerProgressionTypes> progressions = new List<PlayerProgressionTypes>();
			foreach (string progression in args.Progressions)
			{
				progressions.Add(EnumUtilities.Parse<PlayerProgressionTypes>(progression));
			}
			return new ApplicationStateOpenProgressionContainer(fsm, stateId, container, progressions);
		}

		// 060006e5 (1.10.1 RVA 0x64D0A0)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			App app = user.GetAs<App>();
			List<IMetaGameUnlock> unlocks = app.Storage.GetValue<List<IMetaGameUnlock>>(AppFSMKeys.MetaGameUnlocks);
			Queue<IMetaGameUnlock> queuedUnlocks = new Queue<IMetaGameUnlock>();
			HashSet<string> queuedIds = new HashSet<string>();
			foreach (IMetaGameUnlock unlock in unlocks)
			{
				if (m_progressions.Contains(unlock.GetProgressionType()) && queuedIds.Add(unlock.GetId()))
				{
					queuedUnlocks.Enqueue(unlock);
				}
			}
			foreach (IMetaGameUnlock unlock in queuedUnlocks)
			{
				unlocks.Remove(unlock);
			}
			OpenContainer(new UIContainerPlayerProgressionParameters(queuedUnlocks));
		}

		// 060006e6 (1.10.1 RVA 0x64D680)
		private void OpenContainer(UIContainerPlayerProgressionParameters parameters)
		{
			ProcessManager.GetSystem<UIManager>().GetOrCreate(m_containerIdentifier).Setup(parameters);
		}
	}
}
