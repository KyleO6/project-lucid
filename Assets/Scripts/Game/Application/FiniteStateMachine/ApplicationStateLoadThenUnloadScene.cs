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
	// Original Game.Runtime 020000ee; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateLoadThenUnloadScene : FSMState
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		private class JSONCtorArgs
		{
			[GraphUnityScene]
			public string SceneName;

			// 060006ce (1.10.1 RVA 0x64BE60)
			public JSONCtorArgs()
			{
			}
		}

		[CompilerGenerated]
		private sealed class _003CLoadAsyncScene_003Ed__6 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public ApplicationStateLoadThenUnloadScene _003C_003E4__this;

			public IGraphUser user;

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

			// 060006cf (1.10.1 RVA 0x64BE40)
			[DebuggerHidden]
			public _003CLoadAsyncScene_003Ed__6(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			// 060006d0 (1.10.1 RVA 0x64BE70)
			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			// 060006d1 (1.10.1 RVA 0x64BE80)
			private bool MoveNext()
			{
				if (_003C_003E1__state == 0)
				{
					_003C_003E1__state = -1;
					_003C_003E2__current = Resources.UnloadUnusedAssets();
					_003C_003E1__state = 1;
					return true;
				}
				if (_003C_003E1__state == 1)
				{
					_003C_003E1__state = -1;
					AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(_003C_003E4__this.m_sceneName, LoadSceneMode.Additive, true, 100);
					Dictionary<string, AsyncOperationHandle> loadedLevels = user.Storage.GetValue<Dictionary<string, AsyncOperationHandle>>(AppFSMKeys.LoadedLevels, null, true);
					loadedLevels[_003C_003E4__this.m_sceneName] = handle;
					user.Storage.SetValue(AppFSMKeys.LoadedLevels, loadedLevels);
					_003C_003E2__current = handle;
					_003C_003E1__state = 2;
					return true;
				}
				if (_003C_003E1__state == 2)
				{
					_003C_003E1__state = -1;
				}
				return false;
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			// 060006d3 (1.10.1 RVA 0x64C2E0)
			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}

		private readonly string m_sceneName;

		// 060006c9 (1.10.1 RVA 0x64B8D0)
		private ApplicationStateLoadThenUnloadScene(FiniteStateMachine fsm, string stateName, string sceneName) : base(fsm, stateName)
		{
			m_sceneName = sceneName;
		}

		// 060006ca (1.10.1 RVA 0x64B920)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			return new ApplicationStateLoadThenUnloadScene(fsm, stateId, args.SceneName);
		}

		// 060006cb (1.10.1 RVA 0x64B9E0)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			CoroutineUtils.RunCoroutine(LoadAsyncScene(user));
		}

		// 060006cc (1.10.1 RVA 0x64BAF0)
		protected override void DoOnLeave(IGraphUser user, FSMStateChangeAction action)
		{
			Dictionary<string, AsyncOperationHandle> loadedLevels = user.Storage.GetValue<Dictionary<string, AsyncOperationHandle>>(AppFSMKeys.LoadedLevels, null, true);
			if (loadedLevels.TryGetValue(m_sceneName, out AsyncOperationHandle sceneHandle))
			{
				AddressableManager.UnloadSceneAndClearResources(sceneHandle);
				loadedLevels.Remove(m_sceneName);
				user.Storage.SetValue(AppFSMKeys.LoadedLevels, loadedLevels);
				Resources.UnloadUnusedAssets();
			}
		}

		// 060006cd (1.10.1 RVA 0x64BA70)
		[IteratorStateMachine(typeof(_003CLoadAsyncScene_003Ed__6))]
		private IEnumerator LoadAsyncScene(IGraphUser user)
		{
			return new _003CLoadAsyncScene_003Ed__6(0) { _003C_003E4__this = this, user = user };
		}
	}
}
