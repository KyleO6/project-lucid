// Original HLUnityUI.Runtime Hardlight.Gradient 02000060, complete 19-method owner.
// Reconstructed from complete ARM64 and x86_64 native listings and original metadata.
// Original C# spelling/local names are inferred; optimizer, Unity helper, floating-point,
// NaN-to-byte conversion and native fault equivalence remain unqualified.
// Serialized former names are deliberately swapped as shipping annotations specify.
// Setters do not normalize/clamp: Awake alone repairs direction and clamps scale.
// Awake hides the inherited virtual callback (raw flags132/slot65535), and calls base.
// Projection reads each vertex twice; equal projection span and zero scale stay unguarded.
// End-to-start Color32 interpolation replaces prior vertex color; uv1 carries two controls.
// Compiler, provider algorithms, Engine/assets/runtime and public admission remain pending.
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Unity.IL2CPP.CompilerServices;

namespace Hardlight
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [AddComponentMenu("Hardlight UI/Effects/Gradient", 3)]
    public class Gradient : BaseMeshEffect
    {
        [SerializeField]
        [FormerlySerializedAs("m_end")]
        private Color m_startColor = Color.white;
        [SerializeField]
        [FormerlySerializedAs("m_start")]
        private Color m_endColor = Color.white;
        [SerializeField]
        private Vector2 m_direction = Vector2.up;
        [SerializeField]
        private float m_offset;
        [SerializeField]
        private float m_scale = 1f;
        [SerializeField]
        [Range(0f, 1f)]
        private float m_greyscaleFactor;
        [Range(0f, 1f)]
        [SerializeField]
        private float m_brightness;

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive()) return;
            List<UIVertex> verts = new List<UIVertex>();
            vh.GetUIVertexStream(verts);
            ApplyGradient(verts);
            vh.Clear();
            vh.AddUIVertexTriangleStream(verts);
        }

        public Color StartColour
        {
            get { return m_startColor; }
            set { m_startColor = value; if (graphic != null) graphic.SetVerticesDirty(); }
        }
        public Color EndColour
        {
            get { return m_endColor; }
            set { m_endColor = value; if (graphic != null) graphic.SetVerticesDirty(); }
        }
        public float Offset
        {
            get { return m_offset; }
            set { m_offset = value; if (graphic != null) graphic.SetVerticesDirty(); }
        }
        public float Scale
        {
            get { return m_scale; }
            set { m_scale = value; if (graphic != null) graphic.SetVerticesDirty(); }
        }
        public Vector2 Direction
        {
            get { return m_direction; }
            set { m_direction = value; if (graphic != null) graphic.SetVerticesDirty(); }
        }
        public float GreyscaleFactor
        {
            get { return m_greyscaleFactor; }
            set { m_greyscaleFactor = value; if (graphic != null) graphic.SetVerticesDirty(); }
        }
        public float Brightness
        {
            get { return m_brightness; }
            set { m_brightness = value; if (graphic != null) graphic.SetVerticesDirty(); }
        }
        public void SetGreyscaleFactorAndBrightness(float greyscaleFactor, float brightness)
        {
            m_greyscaleFactor = greyscaleFactor;
            m_brightness = brightness;
            if (graphic != null) graphic.SetVerticesDirty();
        }

        protected new void Awake()
        {
            base.Awake();
            if (m_direction == Vector2.zero) m_direction = Vector2.up;
            else m_direction.Normalize();
            m_scale = Mathf.Max(m_scale, 0f);
        }

        private void ApplyGradient(List<UIVertex> verts)
        {
            if (verts.Count == 0) return;
            float[] projections = new float[verts.Count];
            float minimum = float.MaxValue;
            float maximum = float.MinValue;
            for (int i = 0; i < verts.Count; ++i)
            {
                float projection = verts[i].position.x * m_direction.x + verts[i].position.y * m_direction.y;
                projections[i] = projection;
                if (projection < minimum) minimum = projection;
                if (projection > maximum) maximum = projection;
            }
            float span = maximum - minimum;
            for (int i = 0; i < verts.Count; ++i)
            {
                float amount = ((projections[i] - minimum) / span + m_offset - 0.5f) / m_scale + 0.5f;
                UIVertex vertex = verts[i];
                vertex.color = Color32.Lerp(m_endColor, m_startColor, amount);
                vertex.uv1 = new Vector4(m_greyscaleFactor, m_brightness, 0f, 0f);
                verts[i] = vertex;
            }
        }

        public Gradient() { }
    }
}
