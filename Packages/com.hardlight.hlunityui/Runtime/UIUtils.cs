// Original HLUnityUI.Runtime UIUtils research reconstruction.
// Native and source preservation; genuine provider algorithms, current compiler,
// Engine behavior, original authored asset mapping and full-game parity remain held.
using System;
using TMPro;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Hardlight
{
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.NullChecks, false)]
    public static class UIUtils
    {
        // 0x060005d5 UnityEngine.Vector3 ProjectToFrontPlaneish(UnityEngine.Vector3 point, UnityEngine.Camera camera, out System.Boolean isBehind)
        // arm64 0x1b73f14..0x1b7401c; x86_64 0x1b6bb20..0x1b6bc10
        public static Vector3 ProjectToFrontPlaneish(Vector3 point, Camera camera, out bool isBehind)
        {
            Vector3 local = camera.worldToCameraMatrix.MultiplyPoint(point);
            isBehind = local.z > -0.001f;
            if (isBehind)
            {
                local.z = -0.001f;
                point = camera.cameraToWorldMatrix.MultiplyPoint(local);
            }
            return camera.WorldToScreenPoint(point);
        }

        // 0x060005d6 System.Void FitTrimmedImageToBoundsHeight(UnityEngine.UI.Image image, UnityEngine.RectTransform bounds)
        // arm64 0x1b7401c..0x1b7410c; x86_64 0x1b6bc10..0x1b6bd20
        public static void FitTrimmedImageToBoundsHeight(Image image, RectTransform bounds)
        {
            RectTransform target = image.rectTransform;
            float scale = MonoSingleton<HLUIEffectManager>.Instance.ReferencePixelsPerUnit / image.sprite.pixelsPerUnit;
            float height = scale * image.sprite.textureRect.height;
            height = height / image.sprite.rect.height;
            float fitted = bounds.rect.height / height;
            target.sizeDelta = new Vector2(fitted, fitted);
        }

        // 0x060005d7 System.Void ApplySpriteCustomPivotToTransform(UnityEngine.UI.Image image, UnityEngine.RectTransform boundingBox, System.Boolean useSpritesCustomPivot, System.Boolean useNativeSize)
        // arm64 0x1b7410c..0x1b743fc; x86_64 0x1b6bd20..0x1b6bfe0
        public static void ApplySpriteCustomPivotToTransform(Image image, RectTransform boundingBox,
            bool useSpritesCustomPivot, bool useNativeSize)
        {
            RectTransform target = image.rectTransform;
            float scale = MonoSingleton<HLUIEffectManager>.Instance.ReferencePixelsPerUnit / image.sprite.pixelsPerUnit;
            Vector2 size = image.sprite.rect.size * scale;
            // The shipping code divides even the default half-unit pivot by the
            // scaled dimensions. Preserve this unusual value rather than centre it.
            Vector2 pivot = useSpritesCustomPivot ? image.sprite.pivot * scale : new Vector2(0.5f, 0.5f);
            pivot = new Vector2(pivot.x / size.x, pivot.y / size.y);
            if (target != null)
            {
                target.pivot = pivot;
                target.anchoredPosition = Vector2.zero;
            }
            if (boundingBox != null)
            {
                float height = size.y - size.y * pivot.y;
                float boundHeight = boundingBox.rect.height;
                if (height > boundHeight)
                {
                    if (target != null)
                    {
                        target.sizeDelta = size * (boundHeight / height);
                        target.localScale = Vector3.one;
                    }
                    return;
                }
            }
            if (useNativeSize) image.SetNativeSize();
        }

        // 0x060005d8 System.Void SetText(TMPro.TMP_Text target, System.String text)
        // arm64 0x1b743fc..0x1b744b4; x86_64 0x1b6bfe0..0x1b6c060
        public static void SetText(TMP_Text target, string text)
        {
            if (target != null) target.text = text;
        }

        // 0x060005d9 System.Void SetText(TMPro.TMP_Text target, System.String text, System.Boolean active)
        // arm64 0x1b744b4..0x1b74578; x86_64 0x1b6c060..0x1b6c0f0
        public static void SetText(TMP_Text target, string text, bool active)
        {
            SetText(target, text);
            SetActive(target, active);
        }

        // 0x060005da System.Void SetText(TMPro.TMP_Text target, System.String text, UnityEngine.Color color)
        // arm64 0x1b746a0..0x1b74814; x86_64 0x1b6c1b0..0x1b6c2b0
        public static void SetText(TMP_Text target, string text, Color color)
        {
            SetText(target, text);
            SetColor(target, color);
        }

        // 0x060005db System.Void SetSprite(UnityEngine.UI.Image target, UnityEngine.Sprite sprite, System.Boolean enabled = True)
        // arm64 0x1b748fc..0x1b74a08; x86_64 0x1b6c340..0x1b6c3f0
        public static void SetSprite(Image target, Sprite sprite, bool enabled = true)
        {
            if (target != null && sprite != null)
            {
                target.sprite = sprite;
                target.enabled = enabled;
            }
        }

        // 0x060005dc System.Void SetColor(UnityEngine.UI.Graphic target, UnityEngine.Color color)
        // arm64 0x1b74814..0x1b748fc; x86_64 0x1b6c2b0..0x1b6c340
        public static void SetColor(Graphic target, Color color)
        {
            if (target != null) target.color = color;
        }

        // 0x060005dd System.Void SetAlpha(UnityEngine.CanvasGroup target, System.Single alpha)
        // arm64 0x1b74a08..0x1b74ac4; x86_64 0x1b6c3f0..0x1b6c460
        public static void SetAlpha(CanvasGroup target, float alpha)
        {
            if (target != null) target.alpha = alpha;
        }

        // 0x060005de System.Void SetAlpha(UnityEngine.UI.Graphic target, System.Single alpha)
        // arm64 0x1b74ac4..0x1b74be0; x86_64 0x1b6c460..0x1b6c530
        public static void SetAlpha(Graphic target, float alpha)
        {
            // Each component getter is a separate virtual read in both native
            // architectures. Overrides can make this order observable.
            if (target != null) target.color = new Color(target.color.r, target.color.g, target.color.b, alpha);
        }

        // 0x060005df System.Void SetMaterial(UnityEngine.UI.Graphic target, UnityEngine.Material mat)
        // arm64 0x1b74be0..0x1b74c98; x86_64 0x1b6c530..0x1b6c5b0
        public static void SetMaterial(Graphic target, Material mat)
        {
            if (target != null) target.material = mat;
        }

        // 0x060005e0 System.Void SetFill(UnityEngine.UI.Image target, System.Single fillAmount)
        // arm64 0x1b74c98..0x1b74d54; x86_64 0x1b6c5b0..0x1b6c620
        public static void SetFill(Image target, float fillAmount)
        {
            if (target != null) target.fillAmount = fillAmount;
        }

        // 0x060005e1 System.Void SetGradient(Hardlight.Gradient target, UnityEngine.Color startColor, UnityEngine.Color endColor)
        // arm64 0x1b74d54..0x1b74e78; x86_64 0x1b6c620..0x1b6c6c0
        public static void SetGradient(Gradient target, Color startColor, Color endColor)
        {
            if (target != null)
            {
                target.StartColour = startColor;
                target.EndColour = endColor;
            }
        }

        // 0x060005e2 System.Void SetActive(UnityEngine.GameObject target, System.Boolean active)
        // arm64 0x1b74e78..0x1b74f28; x86_64 0x1b6c6c0..0x1b6c730
        public static void SetActive(GameObject target, bool active)
        {
            if (target != null) target.SetActive(active);
        }

        // 0x060005e3 System.Void SetActive(UnityEngine.MonoBehaviour target, System.Boolean active)
        // arm64 0x1b74578..0x1b746a0; x86_64 0x1b6c0f0..0x1b6c1b0
        public static void SetActive(MonoBehaviour target, bool active)
        {
            if (target != null) SetActive(target.gameObject, active);
        }

        // 0x060005e4 System.String FormatInt(System.String formatString, System.Int32 value, System.IFormatProvider numberFormat = null)
        // arm64 0x1b74f28..0x1b74f74; x86_64 0x1b6c730..0x1b6c770
        public static string FormatInt(string formatString, int value, IFormatProvider numberFormat = null)
        {
            // This conversion is present in both native bodies, including its
            // loss of integer precision above the exact range of a float.
            float converted = value;
            return numberFormat != null ? converted.ToString(formatString, numberFormat) : converted.ToString(formatString);
        }

        // 0x060005e5 System.String FormatFloat(System.String formatString, System.Single value, System.IFormatProvider numberFormat = null)
        // arm64 0x1b74f74..0x1b74fb8; x86_64 0x1b6c770..0x1b6c7b0
        public static string FormatFloat(string formatString, float value, IFormatProvider numberFormat = null)
        {
            return numberFormat != null ? value.ToString(formatString, numberFormat) : value.ToString(formatString);
        }

        // 0x060005e6 System.Void SetTrigger(UnityEngine.Animator target, System.String value)
        // arm64 0x1b74fb8..0x1b75068; x86_64 0x1b6c7b0..0x1b6c820
        public static void SetTrigger(Animator target, string value)
        {
            if (target != null) target.SetTrigger(value);
        }

        // 0x060005e7 System.Void SetInteractable(Hardlight.HLSelectable target, System.Boolean interactable)
        // arm64 0x1b75068..0x1b751d4; x86_64 0x1b6c820..0x1b6c960
        public static void SetInteractable(HLSelectable target, bool interactable)
        {
            if (target != null) target.Interactable = interactable;
        }

        // 0x060005e8 System.Void SetInteractable(UnityEngine.UI.Selectable target, System.Boolean interactable)
        // arm64 0x1b752c0..0x1b75370; x86_64 0x1b6ca50..0x1b6cac0
        public static void SetInteractable(Selectable target, bool interactable)
        {
            if (target != null) target.interactable = interactable;
        }

        // 0x060005e9 System.Boolean SetColor(ref UnityEngine.Color currentValue, UnityEngine.Color newValue)
        // arm64 0x1b75370..0x1b753bc; x86_64 0x1b6cac0..0x1b6cb10
        public static bool SetColor(ref Color currentValue, Color newValue)
        {
            // Exact ordered component comparisons retain NaN behavior and differ
            // from Unity's approximate Color operator.
            if (currentValue.r == newValue.r && currentValue.g == newValue.g &&
                currentValue.b == newValue.b && currentValue.a == newValue.a) return false;
            currentValue = newValue;
            return true;
        }

        // 0x060005ea System.Boolean SetStruct(ref T currentValue, T newValue)
        // Open generic definition has no ordinary body; all actual registered shared bodies are preserved separately.
        public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
        {
            // The new value is boxed before constrained Object.Equals dispatch;
            // this does not select an IEquatable<T> overload.
            if (currentValue.Equals((object)newValue)) return false;
            currentValue = newValue;
            return true;
        }

        // 0x060005eb System.Boolean SetClass(ref T currentValue, T newValue)
        // Open generic definition has no ordinary body; all actual registered shared bodies are preserved separately.
        public static bool SetClass<T>(ref T currentValue, T newValue) where T : class
        {
            // Invoke the current object's virtual Equals even for the same
            // reference. Static Object.Equals would introduce an identity shortcut.
            if ((object)currentValue == null && (object)newValue == null) return false;
            if ((object)currentValue != null && currentValue.Equals(newValue)) return false;
            currentValue = newValue;
            return true;
        }
    }
}
