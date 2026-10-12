using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020000f5; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateOpenGameCenterPage : FSMState
	{
		public enum GameCenterPage
		{
			AchievementsPage,
			LeaderboardsPage
		}

		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		private class JSONCtorArgs
		{
			[GraphEnumPopup(typeof(GameCenterPage))]
			public string page;

			// 060006e2 (1.10.1 RVA 0x64CD90)
			public JSONCtorArgs()
			{;}
		}

		private readonly GameCenterPage gameCenterPage;

		// 060006de (1.10.1 RVA 0x64CA80)
		public ApplicationStateOpenGameCenterPage(FiniteStateMachine fsm, string stateName) : base(fsm, stateName)
		{;}

		// 060006df (1.10.1 RVA 0x64CAB0)
		public ApplicationStateOpenGameCenterPage(FiniteStateMachine fsm, string stateName, GameCenterPage page) : base(fsm, stateName)
		{
			gameCenterPage = page;
		}

		// 060006e0 (1.10.1 RVA 0x64CAF0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			GameCenterPage page = (GameCenterPage)Enum.Parse(typeof(GameCenterPage), args.page, true);
			return new ApplicationStateOpenGameCenterPage(fsm, stateId, page);
		}

		// 060006e1 (1.10.1 RVA 0x64CC60)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			App app = user.GetAs<App>();
			switch (gameCenterPage)
			{
			case GameCenterPage.AchievementsPage:
				app.GameCenter.ShowAchievementPage();
				break;
			case GameCenterPage.LeaderboardsPage:
				app.GameCenter.ShowLeaderboardPage();
				break;
			default:
				HLOutput.LogError("Game Center page " + gameCenterPage + " passed into ApplicationStateOpenGameCenterPage is not supported.");
				break;
			}
		}
	}
}
