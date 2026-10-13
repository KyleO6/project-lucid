using Unity.IL2CPP.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Hardlight;
using Hardlight.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HardlightProject
{
	// Original Game.Runtime 02000103; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[GraphNodeMenuFormat("Application/{0}")]
	public class ApplicationStatePlayIntroSequences : FSMState
	{
		[Serializable]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		[Il2CppSetOption(Option.NullChecks, false)]
		protected class JSONCtorArgs
		{
			[GraphUnityObject(typeof(ApplicationStateEvent))]
			public string EventId;

			[GraphUnityObject(typeof(UIContainerIdentifier))]
			public List<string> DoNotCloseContainers;

			public float ControlsDelayTime;

			// 06000707 (1.10.1 RVA 0x650C80)
			public JSONCtorArgs()
			{
			}
		}

		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		private sealed class StateCustomIntroSequence
		{
			public IntroSequence IntroSequence { get; }

			public Transform Start { get; }

			public bool DestroySelf { get; }

			public bool WaitForUnityEvents { get; }

			// 0600070c (1.10.1 RVA 0x6503A0)
			public StateCustomIntroSequence(IntroSequence introSequence, Transform start, bool destroySelf, bool waitForUnityEvents)
			{
				IntroSequence = introSequence;
				Start = start;
				DestroySelf = destroySelf;
				WaitForUnityEvents = waitForUnityEvents;
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass11_0
		{
			public ApplicationStatePlayIntroSequences _003C_003E4__this;

			public Character currentCharacter;

			public App app;

			public LevelManagerLevel currentLevel;

			// 0600070d (1.10.1 RVA 0x64FAD0)
			public _003C_003Ec__DisplayClass11_0()
			{
			}

			// 0600070e (1.10.1 RVA 0x650CE0)
			internal void _003CDoOnEnter_003Eb__0(StateCustomIntroSequence customIntroSequence)
			{
				_003C_003E4__this.PlayIntroSequence(customIntroSequence, currentCharacter, app, currentLevel);
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass13_0
		{
			public AudioManager audioManager;

			// 0600070f (1.10.1 RVA 0x650D10)
			public _003C_003Ec__DisplayClass13_0()
			{
			}

			// 06000710 (1.10.1 RVA 0x650D20)
			internal bool _003CPlayLevelAudio_003Eb__0()
			{
				return audioManager.HasSource(HLAudioSourceIdentifier.Background);
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass14_0
		{
			public IntroSequence customIntroSequence;

			public Action<StateCustomIntroSequence> onComplete;

			public Transform startingPosition;

			// 06000711 (1.10.1 RVA 0x650390)
			public _003C_003Ec__DisplayClass14_0()
			{
			}

			// 06000712 (1.10.1 RVA 0x650D50)
			internal void _003CTryGetCustomIntroSequence_003Eb__0(GameObject asset)
			{
				customIntroSequence = asset.GetComponent<IntroSequence>();
				onComplete(new StateCustomIntroSequence(customIntroSequence, startingPosition, true, true));
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass15_0
		{
			public ApplicationStatePlayIntroSequences _003C_003E4__this;

			public App app;

			public Character currentCharacter;

			// 06000713 (1.10.1 RVA 0x6506D0)
			public _003C_003Ec__DisplayClass15_0()
			{
			}

			// 06000714 (1.10.1 RVA 0x650E30)
			internal void _003CPlayIntroSequence_003Eb__0()
			{
				_003C_003E4__this.OnIntroSequencesCompleted(app, currentCharacter);
			}
		}

		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass16_0
		{
			public ApplicationStatePlayIntroSequences _003C_003E4__this;

			public App app;

			public Character currentCharacter;

			// 06000715 (1.10.1 RVA 0x650E60)
			public _003C_003Ec__DisplayClass16_0()
			{
			}

			// 06000716 (1.10.1 RVA 0x650E70)
			internal void _003CPlayCustomIntroSequence_003Eb__0()
			{
				_003C_003E4__this.OnIntroSequencesCompleted(app, currentCharacter);
			}
		}

		[CompilerGenerated]
		private sealed class _003CPlayCustomIntroSequence_003Ed__16 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public ApplicationStatePlayIntroSequences _003C_003E4__this;

			public App app;

			public Character currentCharacter;

			public StateCustomIntroSequence stateIntroSequence;

			private _003C_003Ec__DisplayClass16_0 _003C_003E8__1;

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

			// 06000717 (1.10.1 RVA 0x6507A0)
			[DebuggerHidden]
			public _003CPlayCustomIntroSequence_003Ed__16(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			// 06000718 (1.10.1 RVA 0x650EA0)
			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			// 06000719 (1.10.1 RVA 0x650EB0)
			private bool MoveNext()
			{
				if (_003C_003E1__state == 0)
				{
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass16_0 { _003C_003E4__this = _003C_003E4__this, app = app, currentCharacter = currentCharacter };
					if (stateIntroSequence.WaitForUnityEvents)
					{
						_003C_003E2__current = null;
						_003C_003E1__state = 1;
						return true;
					}
				}
				else if (_003C_003E1__state == 1)
				{
					_003C_003E1__state = -1;
				}
				else
				{
					return false;
				}
				_003C_003E8__1.currentCharacter.ParentBlobShadowToRig();
				_003C_003E4__this.m_islandManagerRef.Get().ForceActiveIsland(_003C_003E8__1.currentCharacter.WorldPosition);
				_003C_003E8__1.app.TryReleaseUIScreenTransition();
				if (_003C_003E4__this.m_levelManagerRef.Get().TryGetCurrentLevel(out LevelManagerLevel level))
				{
					level.Data.PlayIntroSequence(stateIntroSequence.IntroSequence.Identifier, stateIntroSequence.Start,
						_003C_003E8__1._003CPlayCustomIntroSequence_003Eb__0, stateIntroSequence.DestroySelf);
				}
				return false;
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			// 0600071b (1.10.1 RVA 0x651190)
			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}

		[CompilerGenerated]
		private sealed class _003CPlayLevelAudio_003Ed__13 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public ApplicationStatePlayIntroSequences _003C_003E4__this;

			public LevelManagerLevel loadedLevel;

			private _003C_003Ec__DisplayClass13_0 _003C_003E8__1;

			private HLAudioClipIdentifier _003ClevelAudioClipIdentifier_003E5__2;

			private bool _003CstartAtRandomTime_003E5__3;

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

			// 0600071d (1.10.1 RVA 0x650370)
			[DebuggerHidden]
			public _003CPlayLevelAudio_003Ed__13(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			// 0600071e (1.10.1 RVA 0x6511E0)
			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			// 0600071f (1.10.1 RVA 0x6511F0)
			private bool MoveNext()
			{
				if (_003C_003E1__state == 0)
				{
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass13_0();
					_003ClevelAudioClipIdentifier_003E5__2 = HLAudioClipIdentifier.None;
					_003CstartAtRandomTime_003E5__3 = false;
					MissionManager missionManager = _003C_003E4__this.m_missionManagerRef.Get();
					if (missionManager.HasActiveMission())
					{
						_003ClevelAudioClipIdentifier_003E5__2 = missionManager.ActiveMissionContext.MissionDefinition.MusicOverrideIdentifier;
						_003CstartAtRandomTime_003E5__3 = missionManager.ActiveMissionContext.MissionDefinition.MusicStartAtRandomTime;
					}
					if (_003ClevelAudioClipIdentifier_003E5__2 == HLAudioClipIdentifier.None)
						_003ClevelAudioClipIdentifier_003E5__2 = loadedLevel.LevelDefinition.BackgroundAudioClip;
					_003C_003E8__1.audioManager = ProcessManager.GetSystem<AudioManager>();
					_003C_003E2__current = new WaitUntil(_003C_003E8__1._003CPlayLevelAudio_003Eb__0);
					_003C_003E1__state = 1;
					return true;
				}
				if (_003C_003E1__state == 1)
				{
					_003C_003E1__state = -1;
					_003C_003E8__1.audioManager.PlayClipAtSource(_003ClevelAudioClipIdentifier_003E5__2, HLAudioSourceIdentifier.Background, _003CstartAtRandomTime_003E5__3);
				}
				return false;
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			// 06000721 (1.10.1 RVA 0x651550)
			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}

		private readonly ApplicationStateEvent m_stateEvent;

		private readonly float m_controlsDelayTime;

		private readonly IReadOnlyList<UIContainerIdentifier> m_doNotCloseContainers;

		private readonly SystemRef<MissionManager> m_missionManagerRef = ProcessManager.GetSystemRef<MissionManager>();

		private readonly SystemRef<AddressableManager> m_addressablesManagerRef = ProcessManager.GetSystemRef<AddressableManager>();

		private readonly SystemRef<LevelManager> m_levelManagerRef = ProcessManager.GetSystemRef<LevelManager>();

		private readonly SystemRef<GameplayIslandManager> m_islandManagerRef = ProcessManager.GetSystemRef<GameplayIslandManager>();

		// 060006fd (1.10.1 RVA 0x64F440)
		private ApplicationStatePlayIntroSequences(FiniteStateMachine fsm, string stateName, ApplicationStateEvent stateEvent, IReadOnlyList<UIContainerIdentifier> doNotCloseContainers, float controlsDelayTime) : base(fsm, stateName)
		{
			m_stateEvent = stateEvent;
			m_doNotCloseContainers = doNotCloseContainers;
			m_controlsDelayTime = controlsDelayTime;
		}

		// 060006fe (1.10.1 RVA 0x64F5C0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			ApplicationStateEvent stateEvent = ApplicationStateEvent.FindByName(args.EventId);
			List<UIContainerIdentifier> doNotCloseContainers = new List<UIContainerIdentifier>(args.DoNotCloseContainers.Count);
			foreach (string container in args.DoNotCloseContainers)
			{
				doNotCloseContainers.Add(UIContainerIdentifier.FindByName(container));
			}
			return new ApplicationStatePlayIntroSequences(fsm, stateId, stateEvent, doNotCloseContainers, args.ControlsDelayTime);
		}

		// 060006ff (1.10.1 RVA 0x64F850)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			_003C_003Ec__DisplayClass11_0 locals = new _003C_003Ec__DisplayClass11_0();
			locals._003C_003E4__this = this;
			base.DoOnEnter(user, action);
			LevelManager levelManager = ProcessManager.GetSystem<LevelManager>();
			if (!levelManager.TryGetCurrentLevel(out locals.currentLevel))
			{
				return;
			}
			levelManager.StartLevel();
			levelManager.SetIntroSequencePlaying(true);
			CoroutineUtils.RunCoroutine(PlayLevelAudio(locals.currentLevel));
			ProcessManager.GetSystem<CollectableManager>().ResetTemporaryCollectables();
			ProcessManager.GetSystem<CharacterManager>().TryGetCurrentCharacter(out locals.currentCharacter);
			locals.app = user.GetAs<App>();
			InitialiseCharacter(locals.currentCharacter);
			TryGetCustomIntroSequence(user, locals.currentLevel, locals._003CDoOnEnter_003Eb__0);
		}

		// 06000700 (1.10.1 RVA 0x64FB60)
		private void InitialiseCharacter(Character currentCharacter)
		{
			UnityEngine.Random.InitState(0);
			if (m_missionManagerRef.Get().TryGetActiveMissionState(out MissionState missionState, false) && missionState.Definition.ExclusiveToCharacter)
			{
				CharacterId characterId = missionState.Definition.CharacterId;
				if (currentCharacter.Definition.Id != characterId)
				{
					ProcessManager.GetSystem<CharacterManager>().SetCharacter(characterId, true);
				}
			}
			currentCharacter.OnStart(Vector3.zero, false);
			StackableDataHandle inputOverrideHandle = currentCharacter.AddModifierOverride<bool>(unchecked((int)0xc22df46d), false);
			currentCharacter.Storage.SetValue(ActorFSMKeys.InputOverrideModifierHandle, inputOverrideHandle);
		}

		// 06000701 (1.10.1 RVA 0x64FAE0)
		[IteratorStateMachine(typeof(_003CPlayLevelAudio_003Ed__13))]
		private IEnumerator PlayLevelAudio(LevelManagerLevel loadedLevel)
		{
			return new _003CPlayLevelAudio_003Ed__13(0) { _003C_003E4__this = this, loadedLevel = loadedLevel };
		}

		// 06000702 (1.10.1 RVA 0x64FDA0)
		private void TryGetCustomIntroSequence(IGraphUser user, LevelManagerLevel levelManagerLevel, Action<StateCustomIntroSequence> onComplete)
		{
			_003C_003Ec__DisplayClass14_0 locals = new _003C_003Ec__DisplayClass14_0();
			locals.onComplete = onComplete;
			MissionManager missionManager = m_missionManagerRef.Get();
			MissionDefinition definition = missionManager.ActiveMissionContext?.MissionDefinition;
			if (definition == null)
			{
				locals.onComplete(null);
				return;
			}
			if (missionManager.MissionStates.TryGetValue(definition.GetGUID(), out MissionState state))
			{
				LevelStartPositionDefinition startDefinition = missionManager.GetActiveMissionStartPosition();
				if (!(startDefinition == null))
				{
					locals.startingPosition = levelManagerLevel.Data.GetStartingPosition(startDefinition);
					locals.customIntroSequence = state.Tracker.CustomIntroSequence;
					if (!(locals.customIntroSequence == null) || state.Tracker.IntroBehaviour != MissionTracker.IntroSequenceBehaviour.UseDefault)
					{
						locals.onComplete(new StateCustomIntroSequence(locals.customIntroSequence, locals.startingPosition, false, false));
						return;
					}
					DataManager dataManager = ProcessManager.GetSystem<DataManager>();
					AsyncOperationHandle<GameObject> handle = m_addressablesManagerRef.Get().LoadAssetAsyncInstance(
						dataManager.IntroSequenceDefinitions[IntroSequenceIdentifier.WalkThroughGate].IntroSequence,
						m_levelManagerRef.Get().ManagedGameObjectParent,
						locals._003CTryGetCustomIntroSequence_003Eb__0);
					user.Storage.SetValue(AppFSMKeys.IntroSequenceHandle, handle);
					return;
				}
			}
			locals.onComplete(null);
		}

		// 06000703 (1.10.1 RVA 0x650400)
		private void PlayIntroSequence(StateCustomIntroSequence customIntroSequence, Character currentCharacter, App app, LevelManagerLevel loadedLevel)
		{
			_003C_003Ec__DisplayClass15_0 locals = new _003C_003Ec__DisplayClass15_0();
			locals._003C_003E4__this = this;
			locals.app = app;
			locals.currentCharacter = currentCharacter;
			bool hasCustomIntroSequence = customIntroSequence?.IntroSequence != null;
			if (!hasCustomIntroSequence || customIntroSequence.IntroSequence.CharacterSkipLevelEnterState)
			{
				locals.currentCharacter.Storage.SetValue(ActorFSMKeys.SkipLevelEnter, true);
			}
			if (hasCustomIntroSequence)
			{
				loadedLevel.Data.RegisterIntroSequence(customIntroSequence.IntroSequence);
				CoroutineUtils.RunCoroutine(PlayCustomIntroSequence(customIntroSequence, locals.currentCharacter, locals.app));
				return;
			}
			m_islandManagerRef.Get().ForceActiveIsland(locals.currentCharacter.WorldPosition);
			locals.app.TryReleaseUIScreenTransition();
			loadedLevel.Data.PlayIntroSequence(IntroSequenceIdentifier.IntroRail, null, locals._003CPlayIntroSequence_003Eb__0, false);
		}

		// 06000704 (1.10.1 RVA 0x6506E0)
		[IteratorStateMachine(typeof(_003CPlayCustomIntroSequence_003Ed__16))]
		private IEnumerator PlayCustomIntroSequence(StateCustomIntroSequence stateIntroSequence, Character currentCharacter, App app)
		{
			return new _003CPlayCustomIntroSequence_003Ed__16(0) { _003C_003E4__this = this, stateIntroSequence = stateIntroSequence, currentCharacter = currentCharacter, app = app };
		}

		// 06000705 (1.10.1 RVA 0x6507C0)
		private void OnIntroSequencesCompleted(App app, Character currentCharacter)
		{
			ProcessManager.GetSystem<UIManager>().CloseAllContainersExcept(m_doNotCloseContainers);
			currentCharacter.OnEnterLevel(m_controlsDelayTime);
			if (app.Storage.HasValue(AppFSMKeys.IntroSequenceHandle))
			{
				AsyncOperationHandle<GameObject> handle = app.Storage.GetValue<AsyncOperationHandle<GameObject>>(AppFSMKeys.IntroSequenceHandle);
				m_addressablesManagerRef.Get().ReleaseHandle(handle);
			}
			m_missionManagerRef.Get().StartMission();
			m_levelManagerRef.Get().SetIntroSequencePlaying(false);
			m_islandManagerRef.Get().ForceActiveIsland(currentCharacter.WorldPosition);
			app.SetHasGameplayControl(true, false);
			PublishMessage(app);
		}

		// 06000706 (1.10.1 RVA 0x650AE0)
		private void PublishMessage(IGraphUser user)
		{
			user.Storage.GetValue<List<ApplicationStateEvent>>(AppFSMKeys.StateEvents, null, true).AddUnique(m_stateEvent);
		}
	}
}
