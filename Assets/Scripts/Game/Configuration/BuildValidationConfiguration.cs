// Preservation reconstruction of original Game.Runtime.dll HardlightProject.BuildValidationConfiguration.
// All sixteen methods in its original natural family are represented, including the
// ErrorSeverityLookup getters/constructor and the captured GetSeverity predicate/constructor.
// Native evidence establishes behavior and evaluation order; local names and C# spelling are inferred.
// Current compiler closure ordinals must be discovered independently. No original provider is executed.
using System;
using System.Collections.Generic;
using Hardlight;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace HardlightProject
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [CreateAssetMenu(fileName = "BuildValidationConfiguration", menuName = "HardlightProject/Config/Build Validation")]
    public class BuildValidationConfiguration : SystemConfigurationAsset
    {
        // Original02000412 is a public Serializable nested reference class (flag8192);
        // its physical Serializable custom-attribute payload is absent.
        [Serializable]
        public class ErrorSeverityLookup
        {
            [SerializeField] private ValidationErrorTypes m_errorType;
            [SerializeField] private ValidationErrorSeverity m_errorSeverity;
            // Original060017ea/060017eb: direct signed Int32 field reads at16/20.
            public ValidationErrorTypes ErrorType => m_errorType;
            public ValidationErrorSeverity Severity => m_errorSeverity;
            // Original060017ec: genuine Object base-only constructor, no enum defaults.
            public ErrorSeverityLookup() { }
        }

        public const string AssetMenu = "HardlightProject/Config/";
        public const ValidationErrorSeverity DefaultErrorSeverity = ValidationErrorSeverity.Error;

        [SerializeField] private bool m_validatePrefabs;
        [Tooltip("Allow builds to complete with prefab errors.")]
        [SerializeField]
        [ShowIf("m_validatePrefabs", null)]
        private bool m_warnOnPrefabErrors;
        [SerializeField] private string[] m_excludedPrefabPaths;
        [SerializeField] private bool m_validateScenes;
        [SerializeField] private string[] m_excludedScenePaths = Array.Empty<string>();
        [Tooltip("Allow builds to complete with scene errors.")]
        [ShowIf("m_validateScenes", null)]
        [SerializeField]
        private bool m_warnOnSceneErrors;
        [SerializeField] private string m_resultOutputPath = "";
        [SerializeField]
        [Tooltip("List of severity levels which are considered as a fail.")]
        public List<ValidationErrorSeverity> m_validationFailAtSeverityLevel = new List<ValidationErrorSeverity>();
        [Tooltip("Allow a custom severity to be applied to a validation issue.")]
        [SerializeField]
        private List<ErrorSeverityLookup> m_errorSeverityLookups = new List<ErrorSeverityLookup>();

        // Original060017df..060017e4: retain declared getter order and exact fields.
        public bool ValidatePrefabs => m_validatePrefabs;
        public bool WarnOnPrefabErrors => m_warnOnPrefabErrors;
        public string[] ExcludedPrefabPaths => m_excludedPrefabPaths;
        public bool ValidateScenes => m_validateScenes;
        public bool WarnOnSceneErrors => m_warnOnSceneErrors;
        public string ResultOutputPath => m_resultOutputPath;

        // Original060017e5: GetSeverity completes before the public list is reread.
        // Writing Contains(GetSeverity(...)) would capture that list earlier.
        public bool IsValidationFail(ValidationErrorTypes errorTypes)
        {
            ValidationErrorSeverity severity = GetSeverity(errorTypes);
            return m_validationFailAtSeverityLevel.Contains(severity);
        }

        // Original060017e6: allocate the genuine captured errorType predicate, FindIndex,
        // return Error for a negative index, otherwise reread the field and index its first match.
        // Original displayclass02000413/060017ed is Object base-only;060017ee reads
        // errorLookup.ErrorType before the captured signed enum, faulting on a null lookup.
        // Keep normal lambda compilation instead of declaring a replacement closure/helper.
        public ValidationErrorSeverity GetSeverity(ValidationErrorTypes errorType)
        {
            int index = m_errorSeverityLookups.FindIndex(errorLookup => errorLookup.ErrorType == errorType);
            if (index < 0)
                return DefaultErrorSeverity;
            return m_errorSeverityLookups[index].Severity;
        }

        // Original060017e7: blank input is rejected before reading the exclusion array.
        // The array is captured once; ordered String.Contains tests preserve original
        // empty/null element behavior and use neither filesystem checks nor ValidateScenes.
        public bool ShouldValidateScene(string scenePath)
        {
            if (string.IsNullOrWhiteSpace(scenePath))
                return false;
            string[] excludedScenePaths = m_excludedScenePaths;
            for (int i = 0; i < excludedScenePaths.Length; i++)
                if (scenePath.Contains(excludedScenePaths[i]))
                    return false;
            return true;
        }

        // Original060017e8 is a genuine ARM64 RET and empty x86_64 return body.
        public override void Validate() { }
        // Original060017e9 initializes excluded scenes via Array.Empty<string>(), then
        // the empty result string, severity list and lookup list before the genuine base.
        // Prefab exclusions remain null; bool and nested enum defaults are untouched.
        public BuildValidationConfiguration() { }
    }
}
