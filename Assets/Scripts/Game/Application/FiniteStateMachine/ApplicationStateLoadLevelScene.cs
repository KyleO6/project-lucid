using Unity.IL2CPP.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Hardlight;
using Hardlight.Utils;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 0200013b; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/Levels/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateLoadLevelScene : FSMState
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class JSONCtorArgs
		{
			[GraphUnityObject(typeof(ApplicationStateEvent))]
			public string EventId;

			// 060007a7 (1.10.1 RVA 0x658E60)
			public JSONCtorArgs()
			{
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass6_0
		{
			public LevelManager levelManager;

			public string sceneDefinitionGUID;

			// 060007a8 (1.10.1 RVA 0x658E70)
			public _003C_003Ec__DisplayClass6_0()
			{
			}

			// 060007a9 (1.10.1 RVA 0x658E80)
			internal bool _003CLoadLevel_003Eb__0()
			{
				return levelManager.TryGetCurrentLevel(out LevelManagerLevel level) && level.SceneDefinition.GetGUID() == sceneDefinitionGUID;
			}
		}

		[CompilerGenerated]
		private sealed class _003CLoadLevel_003Ed__6 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public IGraphUser user;

			public ApplicationStateLoadLevelScene _003C_003E4__this;

			private _003C_003Ec__DisplayClass6_0 _003C_003E8__1;

			private GameplayLevelDefinition _003CsceneDefinition_003E5__2;

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{return _003C_003E2__current;}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{return _003C_003E2__current;}
			}

			// 060007aa (1.10.1 RVA 0x658CA0)
			[DebuggerHidden]
			public _003CLoadLevel_003Ed__6(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			// 060007ab (1.10.1 RVA 0x658EB0)
			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			// 060007ac (1.10.1 RVA 0x658EC0)
			private bool MoveNext()
			{
				if (_003C_003E1__state == 0)
				{
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass6_0();
					LevelManagerLoadLevelParameters parameters = user.Storage.GetValue<LevelManagerLoadLevelParameters>(AppFSMKeys.LevelLoadParameters, null, true);
					_003CsceneDefinition_003E5__2 = parameters.LevelDefinition;
					_003C_003E8__1.sceneDefinitionGUID = _003CsceneDefinition_003E5__2.GetGUID();
					MissionManager missionManager = _003C_003E4__this.m_missionManagerRef.Get();
					if (missionManager.HasActiveMission() && missionManager.ActiveMissionContext.MissionDefinition.SceneOverride != null)
					{
						_003CsceneDefinition_003E5__2 = missionManager.ActiveMissionContext.MissionDefinition.SceneOverride;
						_003C_003E8__1.sceneDefinitionGUID = _003CsceneDefinition_003E5__2.GetGUID();
					}
					if (parameters.LevelDefinition == null)
					{
						_003C_003E4__this.PublishMessage(user);
						return false;
					}
					_003C_003E8__1.levelManager = ProcessManager.GetSystem<LevelManager>();
					_003C_003E2__current = _003C_003E8__1.levelManager.LoadLevelSceneForFSM(parameters.LevelDefinition, _003CsceneDefinition_003E5__2);
					_003C_003E1__state = 1;
					return true;
				}
				if (_003C_003E1__state == 1)
				{
					_003C_003E1__state = -1;
					_003C_003E2__current = new WaitUntil(_003C_003E8__1._003CLoadLevel_003Eb__0);
					_003C_003E1__state = 2;
					return true;
				}
				if (_003C_003E1__state == 2)
				{
					_003C_003E1__state = -1;
					if (_003C_003E8__1.levelManager.TryGetCurrentLevel(out LevelManagerLevel level))
					{
						_003C_003E8__1.levelManager.OnLevelLoaded(level);
						_003C_003E4__this.PublishMessage(user);
					}
					else
					{
						HLOutput.LogError($"{nameof(ApplicationStateLoadLevelScene)} has waited for level to be loaded but still could not find level {_003CsceneDefinition_003E5__2.DisplayName}.");
					}
				}
				return false;
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			// 060007ae (1.10.1 RVA 0x6594F0)
			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}

		private readonly ApplicationStateEvent m_stateEvent;

		private readonly SystemRef<MissionManager> m_missionManagerRef = ProcessManager.GetSystemRef<MissionManager>();

		// 060007a2 (1.10.1 RVA 0x658A30)
		protected ApplicationStateLoadLevelScene(FiniteStateMachine fsm, FSMIdentifier stateId, ApplicationStateEvent stateEvent) : base(fsm, stateId)
		{
			m_stateEvent = stateEvent;
		}

		// 060007a3 (1.10.1 RVA 0x658AF0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			return new ApplicationStateLoadLevelScene(fsm, stateId, ApplicationStateEvent.FindByName(args.EventId));
		}

		// 060007a4 (1.10.1 RVA 0x658B90)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			CoroutineUtils.RunCoroutine(LoadLevel(user));
		}

		// 060007a5 (1.10.1 RVA 0x658C20)
		[IteratorStateMachine(typeof(_003CLoadLevel_003Ed__6))]
		private IEnumerator LoadLevel(IGraphUser user)
		{
			return new _003CLoadLevel_003Ed__6(0) { _003C_003E4__this = this, user = user };
		}

		// 060007a6 (1.10.1 RVA 0x658CC0)
		private void PublishMessage(IGraphUser user)
		{
			user.Storage.GetValue<List<ApplicationStateEvent>>(AppFSMKeys.StateEvents, null, true).AddUnique(m_stateEvent);
		}
	}
}
