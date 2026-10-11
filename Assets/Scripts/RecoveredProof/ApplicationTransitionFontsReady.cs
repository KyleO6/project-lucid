// Original Game.Runtime.dll HardlightProject.ApplicationTransitionFontsReady 0x02000171; complete three-method reconstruction.
// 0x0600084b .ctor: ARM64 0x63d714..0x63d71c; x86_64 0x663490..0x6634a0.
// 0x0600084c ConstructInstance: ARM64 0x63d71c..0x63d79c; x86_64 0x6634a0..0x663500.
// 0x0600084d DoUpdate: ARM64 0x63d79c..0x63d9bc; x86_64 0x663500..0x663740.
// The factory ignores jsonCtorArgs; the private constructor uses the original FSMTransition registration path.
// DoUpdate ignores user/context, gets one auto-registering App SystemRef, then re-reads its App/Storage for each key in the original order.
// Eager boolean & evaluates FontsFallbackPreloadReady then FontsChangeLanguageReady even if the first is false; both GetValue calls use default false and storeDefault true.
// No handle status, font-resource validity, missing-App guard or substitute readiness is introduced. Provider callbacks/mutations and native fault/source-form equivalence remain held.
// Original generated/emitted identities, current compiler, actual fonts/Addressables and Engine/bootstrap readiness require separate qualification.
using Hardlight;
using Unity.IL2CPP.CompilerServices;

namespace HardlightProject
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [GraphNodeMenuFormat("Application/{0}")]
    public class ApplicationTransitionFontsReady : FSMTransition
    {
        private ApplicationTransitionFontsReady(FiniteStateMachine fsm, FSMIdentifier transitionId)
            : base(fsm, transitionId)
        {
        }

        public static IFSMTransition ConstructInstance(FiniteStateMachine fsm, FSMIdentifier transitionId, string jsonCtorArgs)
        {
            return new ApplicationTransitionFontsReady(fsm, transitionId);
        }

        protected override bool DoUpdate(IGraphUser user, FSMUpdateContext context)
        {
            var appRef = ProcessManager.GetSystemRef<App>();
            return appRef.Get().Storage.GetValue<bool>(AppFSMKeys.FontsFallbackPreloadReady)
                & appRef.Get().Storage.GetValue<bool>(AppFSMKeys.FontsChangeLanguageReady);
        }
    }
}
