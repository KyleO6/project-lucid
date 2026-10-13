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
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace HardlightProject
{
	// Original Game.Runtime 020000e7; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateLoadOrUnloadScene : FSMState
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class JSONCtorArgs
		{
			[GraphUnityScene]
			public string SceneName;

			[GraphEnumPopup(typeof(SceneCommand))]
			public string Command;

			public bool Additive;

			public bool SetActiveOnLoad;

			[GraphUnityObject(typeof(ApplicationStateEvent))]
			public string EventId;

			// 060006ba (1.10.1 RVA 0x64A560)
			public JSONCtorArgs()
			{
			}
		}

		public enum SceneCommand
		{
			Load,
			Unload
		}

		[CompilerGenerated]
		private sealed class _003CLoadScene_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public IGraphUser user;

			public ApplicationStateLoadOrUnloadScene _003C_003E4__this;

			private AsyncOperationHandle<SceneInstance> _003CloadHandle_003E5__2;

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

			// 060006bb (1.10.1 RVA 0x64A380)
			[DebuggerHidden]
			public _003CLoadScene_003Ed__10(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			// 060006bc (1.10.1 RVA 0x64A570)
			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			// 060006bd (1.10.1 RVA 0x64A580)
			private bool MoveNext()
			{
				if (_003C_003E1__state == 1)
				{
					_003C_003E1__state = -1;
					if (_003C_003E4__this.m_setActive)
					{
						SceneManager.SetActiveScene(_003CloadHandle_003E5__2.Result.Scene);
					}
					_003C_003E4__this.PublishMessage(user);
					return false;
				}
				if (_003C_003E1__state != 0)
				{
					return false;
				}
				_003C_003E1__state = -1;
				Dictionary<string, AsyncOperationHandle> loadedLevels = user.Storage.GetValue<Dictionary<string, AsyncOperationHandle>>(AppFSMKeys.LoadedLevels, null, true);
				if (_003C_003E4__this.m_command == SceneCommand.Load)
				{
					_003CloadHandle_003E5__2 = Addressables.LoadSceneAsync(_003C_003E4__this.m_sceneName,
						_003C_003E4__this.m_additive ? LoadSceneMode.Additive : LoadSceneMode.Single, true, 100);
					loadedLevels[_003C_003E4__this.m_sceneName] = _003CloadHandle_003E5__2;
					user.Storage.SetValue(AppFSMKeys.LoadedLevels, loadedLevels);
					_003C_003E2__current = _003CloadHandle_003E5__2;
					_003C_003E1__state = 1;
					return true;
				}
				if (_003C_003E4__this.m_command == SceneCommand.Unload && loadedLevels.TryGetValue(_003C_003E4__this.m_sceneName, out AsyncOperationHandle sceneHandle))
				{
					AddressableManager.UnloadSceneAndClearResources(sceneHandle);
					loadedLevels.Remove(_003C_003E4__this.m_sceneName);
					user.Storage.SetValue(AppFSMKeys.LoadedLevels, loadedLevels);
				}
				_003C_003E4__this.PublishMessage(user);
				return false;
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			// 060006bf (1.10.1 RVA 0x64AC60)
			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}

		private readonly string m_sceneName;

		private readonly SceneCommand m_command;

		private readonly bool m_additive;

		private readonly bool m_setActive;

		private readonly ApplicationStateEvent m_stateEvent;

		// 060006b5 (1.10.1 RVA 0x64A030)
		private ApplicationStateLoadOrUnloadScene(FiniteStateMachine fsm, FSMIdentifier stateId, string sceneName, SceneCommand command, bool additive, bool setActive, ApplicationStateEvent stateEvent) : base(fsm, stateId)
		{
			m_sceneName = sceneName;
			m_command = command;
			m_additive = additive;
			m_setActive = setActive;
			m_stateEvent = stateEvent;
		}

		// 060006b6 (1.10.1 RVA 0x64A0A0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			return new ApplicationStateLoadOrUnloadScene(fsm, stateId, args.SceneName, (SceneCommand)Enum.Parse(typeof(SceneCommand), args.Command, true), args.Additive, args.SetActiveOnLoad, string.IsNullOrEmpty(args.EventId) ? null : ApplicationStateEvent.FindByName(args.EventId));
		}

		// 060006b7 (1.10.1 RVA 0x64A270)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			CoroutineUtils.RunCoroutine(LoadScene(user));
		}

		// 060006b8 (1.10.1 RVA 0x64A300)
		[IteratorStateMachine(typeof(_003CLoadScene_003Ed__10))]
		private IEnumerator LoadScene(IGraphUser user)
		{
			return new _003CLoadScene_003Ed__10(0) { _003C_003E4__this = this, user = user };
		}

		// 060006b9 (1.10.1 RVA 0x64A3A0)
		private void PublishMessage(IGraphUser user)
		{
			if (m_stateEvent == null)
			{
				return;
			}
			user.Storage.GetValue<List<ApplicationStateEvent>>(AppFSMKeys.StateEvents, null, true).AddUnique(m_stateEvent);
		}
	}
}
