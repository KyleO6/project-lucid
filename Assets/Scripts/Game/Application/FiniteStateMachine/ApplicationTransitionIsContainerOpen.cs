using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 02000177; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationTransitionIsContainerOpen : FSMTransition
	{
		public enum Comparison
		{
			TrueCausesTransition,
			FalseCausesTransition
		}

		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		private class JSONCtorArgs
		{
			[GraphUnityObject(typeof(UIContainerIdentifier))]
			public string Container;

			[GraphEnumPopup(typeof(Comparison))]
			public string Compare;

			// 0600085f (1.10.1 RVA 0x664190)
			public JSONCtorArgs()
			{;}
		}

		private readonly UIContainerIdentifier m_container;

		private readonly Comparison m_compare;

		private readonly SystemRef<UIManager> m_uiManagerRef;

		// 0600085b (1.10.1 RVA 0x663E10)
		private ApplicationTransitionIsContainerOpen(FiniteStateMachine fsm, FSMIdentifier transitionId, UIContainerIdentifier container, Comparison compare) : base(fsm, transitionId)
		{
			m_container = container;
			m_compare = compare;
			m_uiManagerRef = ProcessManager.GetSystemRef<UIManager>();
		}

		// 0600085c (1.10.1 RVA 0x663EE0)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			return new ApplicationTransitionIsContainerOpen(fsm, transitionId, UIContainerIdentifier.FindByName(args.Container), (Comparison)Enum.Parse(typeof(Comparison), args.Compare, true));
		}

		// 0600085d (1.10.1 RVA 0x664050)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext context)
		{
			return m_compare == Comparison.TrueCausesTransition ? IsContainerOpen() : !IsContainerOpen();
		}

		// 0600085e (1.10.1 RVA 0x664120)
		private bool IsContainerOpen()
		{
			return m_uiManagerRef.IsValid() && m_uiManagerRef.Get().IsOpen(m_container);
		}
	}
}
