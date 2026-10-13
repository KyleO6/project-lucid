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
	// Original Game.Runtime 02000141; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/Levels/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateUnloadAllLevels : FSMState
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class JSONCtorArgs
		{
			[GraphUnityObject(typeof(ApplicationStateEvent))]
			public string EventId;

			// 060007bf (1.10.1 RVA 0x65A780)
			public JSONCtorArgs()
			{
			}
		}

		[CompilerGenerated]
		private sealed class _003CUnloadAllLevels_003Ed__5 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public ApplicationStateUnloadAllLevels _003C_003E4__this;

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

			// 060007c0 (1.10.1 RVA 0x65A5C0)
			[DebuggerHidden]
			public _003CUnloadAllLevels_003Ed__5(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			// 060007c1 (1.10.1 RVA 0x65A790)
			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			// 060007c2 (1.10.1 RVA 0x65A7A0)
			private bool MoveNext()
			{
				if (_003C_003E1__state == 0)
				{
					_003C_003E1__state = -1;
					LevelManager levelManager = ProcessManager.GetSystem<LevelManager>();
					if (levelManager.CurrentLevelValid)
					{
						_003C_003E2__current = levelManager.UnloadLevel();
						_003C_003E1__state = 1;
						return true;
					}
				}
				else if (_003C_003E1__state != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				_003C_003E4__this.PublishMessage(user);
				return false;
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			// 060007c4 (1.10.1 RVA 0x65A890)
			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}

		private readonly ApplicationStateEvent m_stateEvent;

		// 060007ba (1.10.1 RVA 0x65A3D0)
		private ApplicationStateUnloadAllLevels(FiniteStateMachine fsm, FSMIdentifier stateId, ApplicationStateEvent stateEvent) : base(fsm, stateId)
		{
			m_stateEvent = stateEvent;
		}

		// 060007bb (1.10.1 RVA 0x65A400)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			return new ApplicationStateUnloadAllLevels(fsm, stateId, ApplicationStateEvent.FindByName(args.EventId));
		}

		// 060007bc (1.10.1 RVA 0x65A4B0)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			CoroutineUtils.RunCoroutine(UnloadAllLevels(user));
		}

		// 060007bd (1.10.1 RVA 0x65A540)
		[IteratorStateMachine(typeof(_003CUnloadAllLevels_003Ed__5))]
		private IEnumerator UnloadAllLevels(IGraphUser user)
		{
			return new _003CUnloadAllLevels_003Ed__5(0) { _003C_003E4__this = this, user = user };
		}

		// 060007be (1.10.1 RVA 0x65A5E0)
		private void PublishMessage(IGraphUser user)
		{
			user.Storage.GetValue<List<ApplicationStateEvent>>(AppFSMKeys.StateEvents, null, true).AddUnique(m_stateEvent);
		}
	}
}
