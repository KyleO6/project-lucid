using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine.Rendering;

namespace ProjectLucid.Tests
{
    // The keyword declarations an imported recovered shader carries follow the editor's active build target;
    // asking GetPassKeywords for ShaderCompilerPlatform.Metal does not switch it (#524). The pinned Core RP
    // 14.0.11 FoveatedRenderingKeywords.hlsl:4 declares _FOVEATED_RENDERING_NON_UNIFORM_RASTER, with its
    // dynamic-branch pragma, only under (SHADER_API_METAL && !UNITY_COMPILER_DXC && (UNITY_PLATFORM_OSX ||
    // UNITY_PLATFORM_IOS || UNITY_PLATFORM_VISIONOS)) || SHADER_API_PS5. On those targets the original Metal
    // tables and masks are the expectation as shipped; on any other target the imported declarations are the
    // same tables without that one name. The context comes from the target alone, never from whether the
    // keyword happens to be present, and every other name, scope and flag stays exact.
    internal static class ImportedKeywordContext
    {
        public const string Foveated = "_FOVEATED_RENDERING_NON_UNIFORM_RASTER";

        public static BuildTarget Target => EditorUserBuildSettings.activeBuildTarget;

        public static bool DeclaresFoveated => DeclaresFoveatedFor(Target);

        // The Apple targets compile Metal without DXC by default, as the shipped Metal programs were.
        public static bool DeclaresFoveatedFor(BuildTarget target) =>
            target == BuildTarget.StandaloneOSX || target == BuildTarget.iOS || target == BuildTarget.VisionOS || target == BuildTarget.PS5;

        public static string Describe(string label) =>
            label + " (active target " + Target + ", requested compiler " + ShaderCompilerPlatform.Metal +
            ", foveated declared " + DeclaresFoveated + ")";

        // The names an original Metal mask leaves declared in this context.
        public static List<string> Imported(IEnumerable<string> original, bool declaresFoveated) =>
            original.Where(name => declaresFoveated || name != Foveated).ToList();

        public static List<string> Imported(IEnumerable<string> original) => Imported(original, DeclaresFoveated);

        // Null when an imported keyword matches its original table entry in this context: the foveated name
        // is absent where the target cannot declare it, and otherwise valid with the original scope and flags.
        public static string Mismatch(string name, bool isValid, bool isDynamic, bool isOverridable, int flag, bool declaresFoveated)
        {
            if (name == Foveated && !declaresFoveated)
                return isValid ? name + " is declared on a target whose Core RP predicate excludes it" : null;
            if (!isValid) return name + " is not declared";
            if (isDynamic != ((flag & 2) != 0)) return name + " dynamic " + isDynamic + ", original flag " + flag;
            if (isOverridable != ((flag & 1) == 0)) return name + " overridable " + isOverridable + ", original flag " + flag;
            return null;
        }

        public static void AssertPass(IEnumerable<string> actual, IEnumerable<string> original, string label) =>
            Assert.That(actual, Is.EquivalentTo(Imported(original)), Describe(label));

        public static void AssertKeyword(UnityEngine.Shader shader, string name, int flag)
        {
            UnityEngine.Rendering.LocalKeyword keyword = shader.keywordSpace.FindKeyword(name);
            Assert.That(Mismatch(name, keyword.isValid, keyword.isDynamic, keyword.isOverridable, flag, DeclaresFoveated),
                Is.Null, Describe(shader.name));
        }
    }
}
