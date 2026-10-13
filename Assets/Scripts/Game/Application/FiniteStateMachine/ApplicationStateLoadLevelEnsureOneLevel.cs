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
	// Original Game.Runtime 02000136; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/Levels/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateLoadLevelEnsureOneLevel : FSMState
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class JSONCtorArgs
		{
			[GraphUnityObject(typeof(ApplicationStateEvent))]
			public string EventId;

			public bool ForceUnloadAny;

			// 06000796 (1.10.1 RVA 0x6582F0)
			public JSONCtorArgs()
			{
			}
		}

		[CompilerGenerated]
		private sealed class _003CEnsureOneLevel_003Ed__6 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public IGraphUser user;

			public ApplicationStateLoadLevelEnsureOneLevel _003C_003E4__this;

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

			// 06000797 (1.10.1 RVA 0x658130)
			[DebuggerHidden]
			public _003CEnsureOneLevel_003Ed__6(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			// 06000798 (1.10.1 RVA 0x658300)
			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			// 06000799 (1.10.1 RVA 0x658310)
			private bool MoveNext()
			{
				if (_003C_003E1__state == 0)
				{
					_003C_003E1__state = -1;
					LevelManager levelManager = ProcessManager.GetSystem<LevelManager>();
					LevelManagerLoadLevelParameters parameters = user.Storage.GetValue<LevelManagerLoadLevelParameters>(AppFSMKeys.LevelLoadParameters, null, true);
					if (levelManager.TryGetCurrentLevel(out LevelManagerLevel currentLevel))
					{
						bool differentLevel = parameters == null || !(currentLevel.LevelDefinition == parameters.LevelDefinition);
						if (_003C_003E4__this.m_forceUnloadAny || differentLevel)
						{
							_003C_003E2__current = levelManager.UnloadLevel();
							_003C_003E1__state = 1;
							return true;
						}
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

			// 0600079b (1.10.1 RVA 0x6585D0)
			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}

		private readonly ApplicationStateEvent m_stateEvent;

		private readonly bool m_forceUnloadAny;

		// 06000791 (1.10.1 RVA 0x657F20)
		private ApplicationStateLoadLevelEnsureOneLevel(FiniteStateMachine fsm, FSMIdentifier stateId, ApplicationStateEvent stateEvent, bool forceUnloadAny) : base(fsm, stateId)
		{
			m_stateEvent = stateEvent;
			m_forceUnloadAny = forceUnloadAny;
		}

		// 06000792 (1.10.1 RVA 0x657F60)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			return new ApplicationStateLoadLevelEnsureOneLevel(fsm, stateId, ApplicationStateEvent.FindByName(args.EventId), args.ForceUnloadAny);
		}

		// 06000793 (1.10.1 RVA 0x658020)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			CoroutineUtils.RunCoroutine(EnsureOneLevel(user));
		}

		// 06000794 (1.10.1 RVA 0x6580B0)
		[IteratorStateMachine(typeof(_003CEnsureOneLevel_003Ed__6))]
		private IEnumerator EnsureOneLevel(IGraphUser user)
		{
			return new _003CEnsureOneLevel_003Ed__6(0) { _003C_003E4__this = this, user = user };
		}

		// 06000795 (1.10.1 RVA 0x658150)
		private void PublishMessage(IGraphUser user)
		{
			user.Storage.GetValue<List<ApplicationStateEvent>>(AppFSMKeys.StateEvents, null, true).AddUnique(m_stateEvent);
		}
	}
}
