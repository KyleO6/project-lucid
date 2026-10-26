using System;
using UnityEngine;

namespace ProjectLucid.Offline
{
    // Offline hooks that exist only for the Apple Arcade launch (the splash video) apply on every
    // desktop target, macOS included (owner decision D7, 2026-10-22), in the player and the editor.
    public static class OfflineDesktopPlatform
    {
        private static RuntimePlatform? s_override;

        public static bool IsDesktop(RuntimePlatform platform)
        {
            switch (platform)
            {
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.LinuxPlayer:
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.LinuxEditor:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsCurrent => IsDesktop(Application.platform);

        // The release shipped one desktop player, for macOS, so its desktop behaviour sits behind
        // OSXPlayer checks. The Windows and Linux players take those macOS player branches (#503); the
        // editors keep their own, as in the original.
        public static bool TakesMacPlayerBranch(RuntimePlatform platform)
            => platform == RuntimePlatform.WindowsPlayer || platform == RuntimePlatform.LinuxPlayer;

        public static bool IsMacPlayerEquivalent => TakesMacPlayerBranch(s_override ?? Application.platform);

        // Test seam only: substitutes the player platform that IsMacPlayerEquivalent reads. Never call it from product code.
        public static IDisposable OverridePlatform(RuntimePlatform platform)
        {
            RuntimePlatform? previous = s_override;
            s_override = platform;
            return new Restore(previous);
        }

        private sealed class Restore : IDisposable
        {
            private readonly RuntimePlatform? m_previous;
            private bool m_disposed;

            internal Restore(RuntimePlatform? previous) { m_previous = previous; }

            public void Dispose()
            {
                if (m_disposed) return;
                m_disposed = true;
                s_override = m_previous;
            }
        }
    }
}
