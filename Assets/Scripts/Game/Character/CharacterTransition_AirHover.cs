using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020003ca; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Character/{0}")]
	[GraphNodeDefaultName("ToAirHover")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class CharacterTransition_AirHover : FSMTransition
	{
		[Serializable]
		[FiniteStateMachineJsonCtorArgs]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		protected class HoverJSONCtorArgs
		{
			public bool IsHovering;

			// 06001707 (1.10.1 RVA 0x78F6F0)
			public HoverJSONCtorArgs()
			{;}
		}

		private readonly bool m_isHovering;

		// 06001704 (1.10.1 RVA 0x78F590)
		protected CharacterTransition_AirHover(FiniteStateMachine fsm, FSMIdentifier transitionId, HoverJSONCtorArgs ctorArgs) : base(fsm, transitionId)
		{
			m_isHovering = ctorArgs.IsHovering;
		}

		// 06001705 (1.10.1 RVA 0x78F5C0)
		public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
		{
			return new CharacterTransition_AirHover(fsm, transitionId, JsonUtility.FromJson<HoverJSONCtorArgs>(jsonCtorArgs));
		}

		// 06001706 (1.10.1 RVA 0x78F660)
		protected override bool DoUpdate(IGraphUser user, FSMUpdateContext updateContext)
		{
			if (base.DoUpdate(user, updateContext))
				return true;
			bool hovering = user.GetAs<Character>().TryGetHoverData(out HoverData hoverData);
			return m_isHovering == hovering;
		}
	}
}
