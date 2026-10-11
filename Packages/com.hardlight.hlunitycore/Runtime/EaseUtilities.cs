// Original shipping EaseUtilities preservation; source form reconstructed from metadata and both native slices.
// No compiler, Unity, provider or original native body executed; current emission and runtime parity held.
// Whole ordinary public class and nested enum retained. Intergral spelling, unknown-type return,
// quad integral discontinuity and strict unordered comparisons remain. Mathf/libm spelling and
// floating-point reassociation, signed zero, NaN payloads and optimizer source form remain held.
// 0x06000f3c Ease: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f3d InQuad: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f3e InQuadIntegral: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f3f OutQuad: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f40 OutQuadIntergral: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f41 InOutQuad: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f42 InOutQuadIntergral: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f43 InCubic: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f44 OutCubic: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f45 InOutCubic: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f46 InQuartic: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f47 OutQuartic: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f48 InOutQuartic: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f49 InQuintic: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f4a OutQuintic: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f4b InOutQuintic: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f4c InSinusoidal: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f4d OutSinusoidal: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f4e InOutSinusoidal: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f4f InExponential: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f50 OutExponential: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f51 InOutExponential: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f52 InElastic: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f53 OutElastic: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f54 InOutElastic: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f55 OutBounce: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f56 InBounce: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f57 InOutBounce: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f58 OutSmallBounce: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f59 InSmallBounce: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f5a InOutSmallBounce: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f5b InBack: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f5c OutBack: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f5d InOutBack: complete arithmetic/branch source below; exact spelling inferred.
// 0x06000f5e .ctor: empty callback/base ctor.
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace Hardlight
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class EaseUtilities
    {
        public enum EaseType
        {
            InQuad = 0,
            OutQuad = 1,
            InOutQuad = 2,
            InCubic = 3,
            OutCubic = 4,
            InOutCubic = 5,
            InQuartic = 6,
            OutQuartic = 7,
            InOutQuartic = 8,
            InQuintic = 9,
            OutQuintic = 10,
            InOutQuintic = 11,
            InSinusoidal = 12,
            OutSinusoidal = 13,
            InOutSinusoidal = 14,
            InElastic = 15,
            OutElastic = 16,
            InOutElastic = 17,
            InBounce = 18,
            OutBounce = 19,
            InOutBounce = 20,
            InSmallBounce = 21,
            OutSmallBounce = 22,
            InOutSmallBounce = 23,
            InBack = 24,
            OutBack = 25,
            InOutBack = 26,
            NONE = 27,
        }

        public static float Ease(EaseType type, float t)
        {
            switch (type)
            {
                case EaseType.InQuad: return InQuad(t);
                case EaseType.OutQuad: return OutQuad(t);
                case EaseType.InOutQuad: return InOutQuad(t);
                case EaseType.InCubic: return InCubic(t);
                case EaseType.OutCubic: return OutCubic(t);
                case EaseType.InOutCubic: return InOutCubic(t);
                case EaseType.InQuartic: return InQuartic(t);
                case EaseType.OutQuartic: return OutQuartic(t);
                case EaseType.InOutQuartic: return InOutQuartic(t);
                case EaseType.InQuintic: return InQuintic(t);
                case EaseType.OutQuintic: return OutQuintic(t);
                case EaseType.InOutQuintic: return InOutQuintic(t);
                case EaseType.InSinusoidal: return InSinusoidal(t);
                case EaseType.OutSinusoidal: return OutSinusoidal(t);
                case EaseType.InOutSinusoidal: return InOutSinusoidal(t);
                case EaseType.InElastic: return InElastic(t);
                case EaseType.OutElastic: return OutElastic(t);
                case EaseType.InOutElastic: return InOutElastic(t);
                case EaseType.InBounce: return InBounce(t);
                case EaseType.OutBounce: return OutBounce(t);
                case EaseType.InOutBounce: return InOutBounce(t);
                case EaseType.InSmallBounce: return InSmallBounce(t);
                case EaseType.OutSmallBounce: return OutSmallBounce(t);
                case EaseType.InOutSmallBounce: return InOutSmallBounce(t);
                case EaseType.InBack: return InBack(t);
                case EaseType.OutBack: return OutBack(t);
                case EaseType.InOutBack: return InOutBack(t);
                default: return t;
            }
        }

        public static float InQuad(float t)
        {
            return t * t;
        }

        public static float InQuadIntegral(float t)
        {
            return t * t * t / 3f;
        }

        public static float OutQuad(float t)
        {
            return (2f - t) * t;
        }

        public static float OutQuadIntergral(float t)
        {
            return (t - 3f) * t * t / -3f;
        }

        public static float InOutQuad(float t)
        {
            t += t;
            if (t < 1f) return 0.5f * t * t;
            t -= 1f;
            return -0.5f * (t * (t - 2f) - 1f);
        }

        public static float InOutQuadIntergral(float t)
        {
            float square = t * t;
            float integral = (square * t + square * t) / 3f;
            return t < 0.5f ? integral : (square + square) - integral - t;
        }

        public static float InCubic(float t)
        {
            return t * t * t;
        }

        public static float OutCubic(float t)
        {
            t -= 1f;
            return t * t * t + 1f;
        }

        public static float InOutCubic(float t)
        {
            t += t;
            if (t < 1f) return 0.5f * t * t * t;
            t -= 2f;
            return 0.5f * (t * t * t + 2f);
        }

        public static float InQuartic(float t)
        {
            return t * t * t * t;
        }

        public static float OutQuartic(float t)
        {
            t -= 1f;
            return 1f - t * t * t * t;
        }

        public static float InOutQuartic(float t)
        {
            t += t;
            if (t < 1f) return 0.5f * t * t * t * t;
            t -= 2f;
            return -0.5f * (t * t * t * t - 2f);
        }

        public static float InQuintic(float t)
        {
            return t * t * t * t * t;
        }

        public static float OutQuintic(float t)
        {
            t -= 1f;
            return t * t * t * t * t + 1f;
        }

        public static float InOutQuintic(float t)
        {
            t += t;
            if (t < 1f) return 0.5f * t * t * t * t * t;
            t -= 2f;
            return 0.5f * (t * t * t * t * t + 2f);
        }

        public static float InSinusoidal(float t)
        {
            if (t <= 0f) return 0f;
            if (t >= 1f) return 1f;
            return 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
        }

        public static float OutSinusoidal(float t)
        {
            if (t <= 0f) return 0f;
            if (t >= 1f) return 1f;
            return Mathf.Sin(t * Mathf.PI * 0.5f);
        }

        public static float InOutSinusoidal(float t)
        {
            if (t <= 0f) return 0f;
            if (t >= 1f) return 1f;
            return (1f - Mathf.Cos(t * Mathf.PI)) * 0.5f;
        }

        public static float InExponential(float t)
        {
            if (t <= 0f) return 0f;
            if (t >= 1f) return 1f;
            return Mathf.Pow(2f, (t - 1f) * 10f);
        }

        public static float OutExponential(float t)
        {
            if (t <= 0f) return 0f;
            if (t >= 1f) return 1f;
            return 1f - Mathf.Pow(2f, t * -10f);
        }

        public static float InOutExponential(float t)
        {
            if (t <= 0f) return 0f;
            if (t >= 1f) return 1f;
            t = t + t - 1f;
            if (t < 0f) return Mathf.Pow(2f, t * 10f) * 0.5f;
            return (2f - Mathf.Pow(2f, t * -10f)) * 0.5f;
        }

        public static float InElastic(float t)
        {
            if (t <= 0f) return 0f;
            if (t >= 1f) return 1f;
            t -= 1f;
            float power = Mathf.Pow(2f, t * 10f);
            return Mathf.Sin((t - 0.075f) * (-2f * Mathf.PI) / 0.3f) * power;
        }

        public static float OutElastic(float t)
        {
            if (t <= 0f) return 0f;
            if (t >= 1f) return 1f;
            float power = Mathf.Pow(2f, t * -10f);
            return power * Mathf.Sin((t - 0.075f) * (2f * Mathf.PI) / 0.3f) + 1f;
        }

        public static float InOutElastic(float t)
        {
            if (t <= 0f) return 0f;
            if (t >= 1f) return 1f;
            t = t + t - 1f;
            float sine = Mathf.Sin((t - 0.1125f) * (2f * Mathf.PI) / 0.45f);
            if (t < 0f) return Mathf.Pow(2f, t * 10f) * sine * -0.5f;
            return Mathf.Pow(2f, t * -10f) * sine * 0.5f + 1f;
        }

        public static float OutBounce(float t)
        {
            if (t < 1f / 2.75f) return 7.5625f * t * t;
            if (t < 2f / 2.75f)
            {
                t -= 1.5f / 2.75f;
                return 7.5625f * t * t + 0.75f;
            }
            if (t < 2.5f / 2.75f)
            {
                t -= 2.25f / 2.75f;
                return 7.5625f * t * t + 0.9375f;
            }
            t -= 2.625f / 2.75f;
            return 7.5625f * t * t + 0.984375f;
        }

        public static float InBounce(float t)
        {
            return 1f - OutBounce(1f - t);
        }

        public static float InOutBounce(float t)
        {
            if (t < 0.5f) return InBounce(t + t) * 0.5f;
            return OutBounce(t + t - 1f) * 0.5f + 0.5f;
        }

        public static float OutSmallBounce(float t)
        {
            if (t < 0.6f) return (1f / (0.6f * 0.6f)) * t * t;
            t -= 0.8f;
            return t * t + 0.96f;
        }

        public static float InSmallBounce(float t)
        {
            return 1f - OutSmallBounce(1f - t);
        }

        public static float InOutSmallBounce(float t)
        {
            if (t < 0.5f) return InSmallBounce(t + t) * 0.5f;
            return OutSmallBounce(t + t - 1f) * 0.5f + 0.5f;
        }

        public static float InBack(float t)
        {
            return t * t * (2.70158f * t - 1.70158f);
        }

        public static float OutBack(float t)
        {
            t -= 1f;
            return t * t * (2.70158f * t + 1.70158f) + 1f;
        }

        public static float InOutBack(float t)
        {
            t += t;
            if (t < 1f) return 0.5f * t * t * (3.5949094f * t - 2.5949094f);
            t -= 2f;
            return 0.5f * t * t * (3.5949094f * t + 2.5949094f) + 1f;
        }

        public EaseUtilities()
        {
        }

    }
}
