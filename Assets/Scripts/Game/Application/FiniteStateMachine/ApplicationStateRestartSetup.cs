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
	// Original Game.Runtime 02000150; bodies translated from the 1.10.1 x86-64 binary.
	[Il2CppSetOption(Option.NullChecks, false)]
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateRestartSetup : FSMState
	{
		[Serializable]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		[Il2CppSetOption(Option.NullChecks, false)]
		private class JSONCtorArgs
		{
			[GraphUnityObject(typeof(ApplicationStateEvent))]
			public string Event;

			// 060007f3 (1.10.1 RVA 0x65F7C0)
			public JSONCtorArgs()
			{
			}
		}

		[CompilerGenerated]
		private sealed class _003CDestroyCharacter_003Ed__5 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public LevelManager levelManager;

			public IGraphUser user;

			public LevelManagerLevel loadedLevel;

			public ApplicationStateRestartSetup _003C_003E4__this;

			private CharacterManager _003CcharacterManager_003E5__2;

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

			// 060007f4 (1.10.1 RVA 0x65F600)
			[DebuggerHidden]
			public _003CDestroyCharacter_003Ed__5(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			// 060007f5 (1.10.1 RVA 0x65F7D0)
			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			// 060007f6 (1.10.1 RVA 0x65F7E0)
			private bool MoveNext()
			{
				if (_003C_003E1__state == 0)
				{
					_003C_003E1__state = -1;
					_003CcharacterManager_003E5__2 = ProcessManager.GetSystem<CharacterManager>();
					if (_003CcharacterManager_003E5__2.TryGetCurrentCharacter(out Character character))
					{
						_003C_003E2__current = character.ExitTriggerVolumes();
						_003C_003E1__state = 1;
						return true;
					}
				}
				else if (_003C_003E1__state == 1)
				{
					_003C_003E1__state = -1;
					_003CcharacterManager_003E5__2.DestroyCharacters();
				}
				else
				{
					return false;
				}
				levelManager.OnLevelRestart();
				user.Storage.SetValue(AppFSMKeys.LevelLoadParameters, new LevelManagerLoadLevelParameters(loadedLevel.LevelDefinition));
				_003C_003E4__this.PublishMessage(user);
				return false;
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			// 060007f8 (1.10.1 RVA 0x65FAF0)
			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}

		private readonly ApplicationStateEvent m_stateEvent;

		// 060007ee (1.10.1 RVA 0x65F2D0)
		private ApplicationStateRestartSetup(FiniteStateMachine fsm, string stateName, ApplicationStateEvent stateEvent) : base(fsm, stateName)
		{
			m_stateEvent = stateEvent;
		}

		// 060007ef (1.10.1 RVA 0x65F320)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			ApplicationStateEvent stateEvent = ApplicationStateEvent.FindByName(args.Event);
			return new ApplicationStateRestartSetup(fsm, stateId, stateEvent);
		}

		// 060007f0 (1.10.1 RVA 0x65F3E0)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			LevelManager levelManager = ProcessManager.GetSystem<LevelManager>();
			if (levelManager.TryGetCurrentLevel(out LevelManagerLevel loadedLevel))
			{
				loadedLevel.Data.UnregisterOutroSequence(user.Storage);
				CoroutineUtils.RunCoroutine(DestroyCharacter(user, levelManager, loadedLevel));
			}
		}

		// 060007f1 (1.10.1 RVA 0x65F540)
		[IteratorStateMachine(typeof(_003CDestroyCharacter_003Ed__5))]
		private IEnumerator DestroyCharacter(IGraphUser user, LevelManager levelManager, LevelManagerLevel loadedLevel)
		{
			return new _003CDestroyCharacter_003Ed__5(0)
			{
				_003C_003E4__this = this, user = user, levelManager = levelManager, loadedLevel = loadedLevel
			};
		}

		// 060007f2 (1.10.1 RVA 0x65F620)
		private void PublishMessage(IGraphUser user)
		{
			user.Storage.GetValue<List<ApplicationStateEvent>>(AppFSMKeys.StateEvents, null, true).AddUnique(m_stateEvent);
		}
	}
}
