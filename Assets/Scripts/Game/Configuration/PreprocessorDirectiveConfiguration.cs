// Preservation reconstruction of original Game.Runtime.dll HardlightProject.PreprocessorDirectiveConfiguration.
// All three original methods are represented. Native Clear is inlined for the value-enum list;
// the managed spelling delegates to the real BCL List without inventing duplicate removal or null defaults.
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Hardlight;
using UnityEngine;

namespace HardlightProject
{
    [CreateAssetMenu(fileName = "PreprocessorDirectiveConfiguration", menuName = "HardlightProject/Config/PreprocessorDirectiveConfiguration")]
    public class PreprocessorDirectiveConfiguration : SystemConfigurationAsset
    {
        [SerializeField] private BuildFeatures[] m_buildFeatures;
        [SerializeField] private BuildFeatures[] m_additionalBuildFeatures;
        private readonly List<BuildFeatures> m_allBuildFeatures = new List<BuildFeatures>();

        // Original06001861: Clear, primary AddRange, additional AddRange, then AsReadOnly.
        // Each field is reread in its original order. Arrays remain authored nullable fields;
        // returned views share the same cache, preserving order and duplicate feature values.
        public ReadOnlyCollection<BuildFeatures> GetBuildFeatures()
        {
            m_allBuildFeatures.Clear();
            m_allBuildFeatures.AddRange(m_buildFeatures);
            m_allBuildFeatures.AddRange(m_additionalBuildFeatures);
            return m_allBuildFeatures.AsReadOnly();
        }

        // Original06001862 is a genuine ARM64 RET and empty x86_64 return body.
        public override void Validate() { }
        // Original06001863 initializes only the private readonly list before the genuine
        // SystemConfigurationAsset constructor. It does not populate either serialized array.
        public PreprocessorDirectiveConfiguration() { }
    }
}
