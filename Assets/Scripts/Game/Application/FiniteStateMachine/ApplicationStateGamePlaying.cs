using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020000d3; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateGamePlaying : FSMState
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		private class JSONCtorArgs
		{
			[GraphUnityObject(typeof(UIContainerIdentifier))]
			public string FrameDebugInfoContainer;

			[GraphUnityObject(typeof(UIContainerIdentifier))]
			public string CollectableDebugContainer;

			// 0600066d (1.10.1 RVA 0x646A50)
			public JSONCtorArgs()
			{;}
		}

		private readonly UIContainerIdentifier m_frameDebugInfoContainerIdentifier;

		private readonly UIContainerIdentifier m_collectableDebugContainer;

		// 06000667 (1.10.1 RVA 0x646830)
		private ApplicationStateGamePlaying(FiniteStateMachine fsm, string stateName, UIContainerIdentifier frameDebugInfoContainerIdentifier, UIContainerIdentifier collectableDebugContainer) : base(fsm, stateName)
		{
			m_frameDebugInfoContainerIdentifier = frameDebugInfoContainerIdentifier;
			m_collectableDebugContainer = collectableDebugContainer;
		}

		// 06000668 (1.10.1 RVA 0x6468A0)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			UIContainerIdentifier frameDebugInfoContainer = UIContainerIdentifier.FindByName(args.FrameDebugInfoContainer);
			UIContainerIdentifier collectableDebugContainer = UIContainerIdentifier.FindByName(args.CollectableDebugContainer);
			return new ApplicationStateGamePlaying(fsm, stateId, frameDebugInfoContainer, collectableDebugContainer);
		}

		// 06000669 (1.10.1 RVA 0x646990)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{;}

		// 0600066a (1.10.1 RVA 0x6469A0)
		protected override void DoOnLeave(IGraphUser user, FSMStateChangeAction action)
		{;}

		// 0600066b (1.10.1 RVA 0x6469B0)
		private void ToggleFrameInfo()
		{
			ToggleContainer(m_frameDebugInfoContainerIdentifier);
		}

		// 0600066c (1.10.1 RVA 0x6469C0)
		private static void ToggleContainer(UIContainerIdentifier identifier)
		{
			UIManager uiManager = ProcessManager.GetSystem<UIManager>();
			if (uiManager.IsOpen(identifier))
			{
				uiManager.Close(identifier);
			}
			else
			{
				uiManager.GetOrCreate(identifier);
			}
		}
	}
}
