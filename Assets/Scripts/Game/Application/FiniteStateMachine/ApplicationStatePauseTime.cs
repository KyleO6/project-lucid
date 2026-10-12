using Unity.IL2CPP.CompilerServices;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020000fe; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStatePauseTime : FSMState
	{
		private enum PauseTimeMode
		{
			Include,
			Exclude
		}

		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		private class JSONCtorArgs
		{
			[GraphEnumPopup(typeof(PauseTimeMode))]
			public string Mode;

			[GraphEnumPopup(typeof(TimeCategory))]
			public List<string> Categories;

			// 060006f8 (1.10.1 RVA 0x64EEF0)
			public JSONCtorArgs()
			{;}
		}

		private PauseTimeMode m_mode { get; }

		private List<TimeCategory> m_categories { get; }

		// 060006f5 (1.10.1 RVA 0x64E770)
		private ApplicationStatePauseTime(FiniteStateMachine fsm, string stateName, PauseTimeMode mode, List<TimeCategory> categories) : base(fsm, stateName)
		{
			m_mode = mode;
			m_categories = categories;
		}

		// 060006f6 (1.10.1 RVA 0x64E7D0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			PauseTimeMode mode = (PauseTimeMode)Enum.Parse(typeof(PauseTimeMode), args.Mode, true);
			List<TimeCategory> categories = new List<TimeCategory>(args.Categories.Count);
			foreach (string category in args.Categories)
			{
				categories.Add((TimeCategory)Enum.Parse(typeof(TimeCategory), category, true));
			}
			return new ApplicationStatePauseTime(fsm, stateId, mode, categories);
		}

		// 060006f7 (1.10.1 RVA 0x64EAB0)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			if (user.Storage.HasValue<StackableDataHandle>(AppFSMKeys.PauseTimeOverride))
			{
				return;
			}

			TimeSetting setting = TimeSetting.GetDefault(m_mode != PauseTimeMode.Exclude ? 1f : 0f);
			foreach (TimeCategory category in m_categories)
			{
				if (category == TimeCategory.UnityGlobal && m_mode == PauseTimeMode.Include)
				{
					setting.SetCategory(category, 1f);
				}
				else
				{
					float value;
					switch (m_mode)
					{
						case PauseTimeMode.Include:
							value = 0f;
							break;
						case PauseTimeMode.Exclude:
							value = 1f;
							break;
						default:
							return;
					}
					setting.SetCategory(category, value);
				}
			}
			StackableDataHandle handle = ProcessManager.GetSystem<TimeManager>().ApplyTimeSetting(setting);
			user.Storage.SetValue(AppFSMKeys.PauseTimeOverride, handle);
		}
	}
}
