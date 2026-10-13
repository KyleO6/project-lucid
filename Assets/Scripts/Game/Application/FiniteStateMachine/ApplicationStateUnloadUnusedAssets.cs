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
	// Original Game.Runtime 0200012f; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateUnloadUnusedAssets : FSMState
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class JSONCtorArgs
		{
			[GraphUnityObject(typeof(ApplicationStateEvent))]
			public string EventId;

			public int FrameDelayCount;

			// 0600077b (1.10.1 RVA 0x656230)
			public JSONCtorArgs()
			{
			}
		}

		[CompilerGenerated]
		private sealed class _003CReleaseUnusedAssets_003Ed__6 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public ApplicationStateUnloadUnusedAssets _003C_003E4__this;

			public IGraphUser user;

			private int _003Ci_003E5__2;

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

			// 0600077c (1.10.1 RVA 0x656070)
			[DebuggerHidden]
			public _003CReleaseUnusedAssets_003Ed__6(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			// 0600077d (1.10.1 RVA 0x656240)
			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			// 0600077e (1.10.1 RVA 0x656250)
			private bool MoveNext()
			{
				switch (_003C_003E1__state)
				{
				case 0:
					_003C_003E1__state = -1;
					_003Ci_003E5__2 = 0;
					goto checkFrames;
				case 1:
					_003C_003E1__state = -1;
					_003Ci_003E5__2++;
					goto checkFrames;
				case 2:
					_003C_003E1__state = -1;
					_003C_003E4__this.PublishMessage(user);
					return false;
				default:
					return false;
				}
			checkFrames:
				if (_003Ci_003E5__2 < _003C_003E4__this.m_frameDelayCount)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				AsyncOperation operation = Resources.UnloadUnusedAssets();
				operation.priority = 0;
				_003C_003E2__current = operation;
				_003C_003E1__state = 2;
				return true;
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			// 06000780 (1.10.1 RVA 0x656320)
			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}

		private readonly ApplicationStateEvent m_stateEvent;

		private readonly int m_frameDelayCount;

		// 06000776 (1.10.1 RVA 0x655E60)
		protected ApplicationStateUnloadUnusedAssets(FiniteStateMachine fsm, FSMIdentifier stateId, ApplicationStateEvent stateEvent, int frameDelayCount) : base(fsm, stateId)
		{
			m_stateEvent = stateEvent;
			m_frameDelayCount = frameDelayCount;
		}

		// 06000777 (1.10.1 RVA 0x655EA0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			return new ApplicationStateUnloadUnusedAssets(fsm, stateId, ApplicationStateEvent.FindByName(args.EventId), args.FrameDelayCount);
		}

		// 06000778 (1.10.1 RVA 0x655F60)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			CoroutineUtils.RunCoroutine(ReleaseUnusedAssets(user));
		}

		// 06000779 (1.10.1 RVA 0x655FF0)
		[IteratorStateMachine(typeof(_003CReleaseUnusedAssets_003Ed__6))]
		private IEnumerator ReleaseUnusedAssets(IGraphUser user)
		{
			return new _003CReleaseUnusedAssets_003Ed__6(0) { _003C_003E4__this = this, user = user };
		}

		// 0600077a (1.10.1 RVA 0x656090)
		private void PublishMessage(IGraphUser user)
		{
			user.Storage.GetValue<List<ApplicationStateEvent>>(AppFSMKeys.StateEvents, null, true).AddUnique(m_stateEvent);
		}
	}
}
