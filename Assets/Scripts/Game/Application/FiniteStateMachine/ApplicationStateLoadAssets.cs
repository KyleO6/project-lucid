using Unity.IL2CPP.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Hardlight;
using Hardlight.Utils;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace HardlightProject
{
	// Original Game.Runtime 020000de; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateLoadAssets : FSMState
	{
		[CompilerGenerated]
		private sealed class _003CInitialiseSaveData_003Ed__3 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public SaveManager saveManager;

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

			// 0600069c (1.10.1 RVA 0x648EE0)
			[DebuggerHidden]
			public _003CInitialiseSaveData_003Ed__3(int _003C_003E1__state)
			{
				this._003C_003E1__state = _003C_003E1__state;
			}

			// 0600069d (1.10.1 RVA 0x6490C0)
			[DebuggerHidden]
			void IDisposable.Dispose()
			{;}

			// 0600069e (1.10.1 RVA 0x6490D0)
			private bool MoveNext()
			{
				if (_003C_003E1__state == 0)
				{
					_003C_003E1__state = -1;
					_003C_003E2__current = saveManager.InitialiseSaveData();
					_003C_003E1__state = 1;
					return true;
				}
				if (_003C_003E1__state == 1)
				{
					_003C_003E1__state = -1;
					_003C_003E2__current = saveManager.OpenSave();
					_003C_003E1__state = 2;
					return true;
				}
				if (_003C_003E1__state == 2)
				{
					_003C_003E1__state = -1;
					user.Storage.SetValue(AppFSMKeys.GameSaveLoaded, true);
				}
				return false;
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			// 060006a0 (1.10.1 RVA 0x649300)
			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}
		}

		// 06000693 (1.10.1 RVA 0x648B50)
		public ApplicationStateLoadAssets(FiniteStateMachine fsm, string stateName) : base(fsm, stateName)
		{;}

		// 06000694 (1.10.1 RVA 0x648B80)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new ApplicationStateLoadAssets(fsm, transitionId);
		}

		// 06000695 (1.10.1 RVA 0x648C00)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			Social.localUser.Authenticate(_ => { });
			user.GetAs<App>().DataManager.LoadDefinitionData();
			CoroutineUtils.RunCoroutine(InitialiseSaveData(ProcessManager.GetSystem<SaveManager>(), user));
		}

		// 06000696 (1.10.1 RVA 0x648E60)
		[IteratorStateMachine(typeof(_003CInitialiseSaveData_003Ed__3))]
		private static IEnumerator InitialiseSaveData(SaveManager saveManager, IGraphUser user)
		{
			return new _003CInitialiseSaveData_003Ed__3(0) { saveManager = saveManager, user = user };
		}
	}
}
