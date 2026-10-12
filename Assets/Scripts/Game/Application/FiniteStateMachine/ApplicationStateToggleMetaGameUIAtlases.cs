using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine;
using UnityEngine.U2D;

namespace HardlightProject
{
	// Original Game.Runtime 02000158; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/Systems/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateToggleMetaGameUIAtlases : FSMState
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class JSONCtorArgs
		{
			public bool AtlasesShouldBeLoaded;

			// 0600080a (1.10.1 RVA 0x6604A0)
			public JSONCtorArgs()
			{;}
		}

		private readonly bool m_atlasesShouldBeLoaded;

		private readonly SystemRef<VisualAssetManager> m_visualAssetManagerRef = ProcessManager.GetSystemRef<VisualAssetManager>();

		// 06000807 (1.10.1 RVA 0x660260)
		public ApplicationStateToggleMetaGameUIAtlases(FiniteStateMachine fsm, FSMIdentifier stateId, bool atlasesShouldBeLoaded) : base(fsm, stateId)
		{
			m_atlasesShouldBeLoaded = atlasesShouldBeLoaded;
		}

		// 06000808 (1.10.1 RVA 0x660310)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			return new ApplicationStateToggleMetaGameUIAtlases(fsm, transitionId, args.AtlasesShouldBeLoaded);
		}

		// 06000809 (1.10.1 RVA 0x660410)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			if (m_visualAssetManagerRef.TryGet(out VisualAssetManager manager))
			{
				if (m_atlasesShouldBeLoaded) manager.RequestAtlas("Characters_UI_Atlas", null);
				else manager.UnloadAtlas("Characters_UI_Atlas");
			}
		}
	}
}
