// Original Sonic Dream Team 1.10.1 HLUnityUI.Runtime Hardlight.ActiveSelectables.
// Full ARM64/x86_64 registered native bodies support the preserved registry order.
// 0x0600042b Add arm64 0x1b632a0..0x1b634dc / x86_64 0x1b5ac30..0x1b5ae70.
// 0x0600042c Remove arm64 0x1b634dc..0x1b6363c / x86_64 0x1b5ae70..0x1b5afe0.
// 0x0600042d GetSelectables arm64 0x1b6363c..0x1b637e0 / x86_64 0x1b5afe0..0x1b5b170.
// 0x0600042e .cctor arm64 0x1b637e0..0x1b638d4 / x86_64 0x1b5b170..0x1b5b250.
// Global AddUnique precedes the scene-name check; per-scene Add allows duplicates.
// Remove removes one entry per list and retains empty scene keys. GetSelectables
// returns the live List through IReadOnlyCollection and creates missing scene lists.
// Readonly field initializers preserve original BeforeFieldInit and constructor order.
// Local C# spelling inferred; provider algorithms, faults, current emission and Unity remain held.
using System.Collections.Generic;

namespace Hardlight
{
    public static class ActiveSelectables
    {
        private static readonly Dictionary<string, List<HLSelectable>> s_selectables =
            new Dictionary<string, List<HLSelectable>>();
        private static readonly List<HLSelectable> s_allSelectables = new List<HLSelectable>();

        public static void Add(HLSelectable selectable, string sceneName = null)
        {
            s_allSelectables.AddUnique(selectable);
            if (string.IsNullOrWhiteSpace(sceneName)) return;
            if (!s_selectables.ContainsKey(sceneName))
                s_selectables[sceneName] = new List<HLSelectable>();
            s_selectables[sceneName].Add(selectable);
        }

        public static void Remove(HLSelectable selectable, string sceneName = null)
        {
            s_allSelectables.Remove(selectable);
            if (string.IsNullOrWhiteSpace(sceneName)) return;
            if (s_selectables.ContainsKey(sceneName))
                s_selectables[sceneName].Remove(selectable);
        }

        public static IReadOnlyCollection<HLSelectable> GetSelectables(string sceneName = null)
        {
            if (string.IsNullOrWhiteSpace(sceneName)) return s_allSelectables;
            if (!s_selectables.ContainsKey(sceneName))
                s_selectables[sceneName] = new List<HLSelectable>();
            return s_selectables[sceneName];
        }
    }
}
