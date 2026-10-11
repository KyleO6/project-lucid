// Original HLUnityUI.Runtime Hardlight.HLNavigation 020000e6 and nested NavMode 020000e7.
// Recovered from complete shipping metadata and all 24 arm64/x86_64 ranges; source spelling is inferred.
// DefaultNavigation selects Automatic(3) with null references. Equals short-circuits Mode, Up, Down, Left, Right
// through genuine UnityEngine.Object equality; it does not substitute reference equality or add null guards.
// Current emission, provider algorithm, Unity destroyed-object/fault behavior and gameplay equivalence remain held.
// 0x06000647 get_Mode arm64 0x1b785ec..0x1b785f4; x86_64 0x1b6fd80..0x1b6fd90.
// 0x06000648 set_Mode arm64 0x1b785f4..0x1b785fc; x86_64 0x1b6fd90..0x1b6fda0.
// 0x06000649 get_SelectOnUp arm64 0x1b785fc..0x1b78604; x86_64 0x1b6fda0..0x1b6fdb0.
// 0x0600064a set_SelectOnUp arm64 0x1b78604..0x1b7860c; x86_64 0x1b6fdb0..0x1b6fdd0.
// 0x0600064b get_SelectOnDown arm64 0x1b7860c..0x1b78614; x86_64 0x1b6fdd0..0x1b6fde0.
// 0x0600064c set_SelectOnDown arm64 0x1b78614..0x1b7861c; x86_64 0x1b6fde0..0x1b6fe00.
// 0x0600064d get_SelectOnLeft arm64 0x1b7861c..0x1b78624; x86_64 0x1b6fe00..0x1b6fe10.
// 0x0600064e set_SelectOnLeft arm64 0x1b78624..0x1b7862c; x86_64 0x1b6fe10..0x1b6fe30.
// 0x0600064f get_SelectOnRight arm64 0x1b7862c..0x1b78634; x86_64 0x1b6fe30..0x1b6fe40.
// 0x06000650 set_SelectOnRight arm64 0x1b78634..0x1b7863c; x86_64 0x1b6fe40..0x1b6fe60.
// 0x06000651 get_DefaultNavigation arm64 0x1b7863c..0x1b78658; x86_64 0x1b6fe60..0x1b6fe90.
// 0x06000652 Equals arm64 0x1b78658..0x1b787c4; x86_64 0x1b6fe90..0x1b6ffb0.
using System;
using UnityEngine;

namespace Hardlight
{
    [Serializable]
    public struct HLNavigation : IEquatable<HLNavigation>
    {
        [Flags]
        public enum NavMode
        {
            None = 0,
            Horizontal = 1,
            Vertical = 2,
            Automatic = 3,
            Explicit = 4
        }

        [SerializeField] private NavMode m_mode;
        [SerializeField] private HLSelectable m_selectOnUp;
        [SerializeField] private HLSelectable m_selectOnDown;
        [SerializeField] private HLSelectable m_selectOnLeft;
        [SerializeField] private HLSelectable m_selectOnRight;

        public NavMode Mode
        {
            get { return m_mode; }
            set { m_mode = value; }
        }

        public HLSelectable SelectOnUp
        {
            get { return m_selectOnUp; }
            set { m_selectOnUp = value; }
        }

        public HLSelectable SelectOnDown
        {
            get { return m_selectOnDown; }
            set { m_selectOnDown = value; }
        }

        public HLSelectable SelectOnLeft
        {
            get { return m_selectOnLeft; }
            set { m_selectOnLeft = value; }
        }

        public HLSelectable SelectOnRight
        {
            get { return m_selectOnRight; }
            set { m_selectOnRight = value; }
        }

        public static HLNavigation DefaultNavigation
        {
            get
            {
                HLNavigation navigation = default(HLNavigation);
                navigation.m_mode = NavMode.Automatic;
                return navigation;
            }
        }

        public bool Equals(HLNavigation other)
        {
            return m_mode == other.m_mode
                && m_selectOnUp == other.m_selectOnUp
                && m_selectOnDown == other.m_selectOnDown
                && m_selectOnLeft == other.m_selectOnLeft
                && m_selectOnRight == other.m_selectOnRight;
        }
    }
}
