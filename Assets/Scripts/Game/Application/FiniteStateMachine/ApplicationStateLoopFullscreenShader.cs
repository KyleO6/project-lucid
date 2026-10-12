using Unity.IL2CPP.CompilerServices;
using System;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
	// Original Game.Runtime 020000f1; bodies translated from the 1.10.1 x86-64 binary.
	[GraphNodeMenuFormat("Application/{0}")]
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class ApplicationStateLoopFullscreenShader : FSMState
	{
		[Serializable]
		[Il2CppSetOption(Option.NullChecks, false)]
		[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
		private class JSONCtorArgs
		{
			[GraphEnumPopup(typeof(FullscreenShaderParametersType))]
			public string StartShaderType;

			public float StartShaderDuration;

			[GraphEnumPopup(typeof(FullscreenShaderParametersType))]
			public string LoopShaderType;

			// 060006d8 (1.10.1 RVA 0x64C760)
			public JSONCtorArgs()
			{;}
		}

		private readonly FullscreenShaderParametersType m_startShaderType;

		private readonly float m_startShaderDuration;

		private readonly FullscreenShaderParametersType m_loopShaderType;

		// 060006d5 (1.10.1 RVA 0x64C330)
		private ApplicationStateLoopFullscreenShader(FiniteStateMachine fsm, string stateName, FullscreenShaderParametersType startShaderType, float startShaderDuration, FullscreenShaderParametersType loopShaderType) : base(fsm, stateName)
		{
			m_startShaderType = startShaderType;
			m_startShaderDuration = startShaderDuration;
			m_loopShaderType = loopShaderType;
		}

		// 060006d6 (1.10.1 RVA 0x64C390)
		public new static IFSMState ConstructInstance(FiniteStateMachine fsm, FSMIdentifier stateId, string jsonCtorArgs)
		{
			JSONCtorArgs args = JsonUtility.FromJson<JSONCtorArgs>(jsonCtorArgs);
			FullscreenShaderParametersType startShaderType = EnumUtilities.Parse<FullscreenShaderParametersType>(args.StartShaderType);
			float startShaderDuration = args.StartShaderDuration;
			FullscreenShaderParametersType loopShaderType = EnumUtilities.Parse<FullscreenShaderParametersType>(args.LoopShaderType);
			return new ApplicationStateLoopFullscreenShader(fsm, stateId, startShaderType, startShaderDuration, loopShaderType);
		}

		// 060006d7 (1.10.1 RVA 0x64C490)
		protected override void DoOnEnter(IGraphUser user, FSMStateChangeAction action)
		{
			base.DoOnEnter(user, action);
			FullscreenShaderManager shaderManager = ProcessManager.GetSystem<FullscreenShaderManager>();
			FullscreenShaderManager.ParametersHandle handle = default;
			shaderManager.StartEffectOverride(m_startShaderType, out handle, m_startShaderDuration, true, false, () =>
			{
				shaderManager.StartEffectOverride(m_loopShaderType, out _, 0f, false, true);
				user.Storage.RemoveValue<FullscreenShaderManager.ParametersHandle>(FSMStorageKeys.ShaderOverride);
			});
			user.Storage.SetValue(FSMStorageKeys.ShaderOverride, handle);
		}
	}
}
