using System;
using System.Collections.Generic;
using System.Reflection;
using Hardlight;
using HardlightProject;
using NUnit.Framework;
using UnityEngine;

namespace ProjectLucid.Tests
{
    // TryGetProjectedConeSurfaceHit's cone test (#525). The original callback (06003a6b) passes the point projected
    // onto the cone axis, not the cone origin, as IsPositionInsideCone's second argument. Each case drives the real
    // caller with a plain managed surface that returns one fixed world point, and compares its hit with the original
    // expression composed from the real projection and containment helpers.
    public sealed class SurfaceTrackingConeOperandTests
    {
        [Test]
        public void InteriorAndExteriorPointsKeepTheirResults()
        {
            var cone = Cone(Quaternion.identity, new Vector2(0.125f, 0.125f), 20);
            using (new ConeTableLease())
            {
                // Depth 10, radius 1.25.
                Vector3 inside = new Vector3(0.5f, 0.25f, 10), outside = new Vector3(5, 0, 10);
                Assert.That(Original(cone, inside), Is.True);
                ExpectHit(cone, inside);
                Assert.That(Original(cone, outside), Is.False);
                ExpectMiss(cone, outside);
            }
        }

        [Test]
        public void RotatedBoundaryPointsFollowTheOriginalOperand()
        {
            // The issue's rotated diagnostic: a point one unit off the axis at depth ten under each yaw, exactly on the
            // inclusive boundary (radius 0.1 * 10). Translating along the axis leaves local XY unchanged in exact
            // arithmetic, so the two operands differ only in float rounding, angle by angle. The production result
            // must match the original expression at every angle, whatever each Boolean is on this runtime, and at
            // least one angle must separate the two operands, so the sweep rejects the old one.
            int separating = 0;
            using (new ConeTableLease())
            {
                for (int angle = 1; angle < 180; angle++)
                {
                    Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
                    var cone = Cone(rotation, new Vector2(0.1f, 0.1f), 20);
                    Vector3 point = rotation * new Vector3(1, 0, 10);
                    bool original = Original(cone, point);
                    if (original != FromOrigin(cone, point)) separating++;
                    if (original) ExpectHit(cone, point, angle + " degrees");
                    else ExpectMiss(cone, point, angle + " degrees");
                }
            }
            Assert.That(separating, Is.GreaterThan(0), "some boundary angle separates the original operand from the cone origin");
        }

        private static ConeVolume.ConeDescription Cone(Quaternion rotation, Vector2 tangents, float distance) =>
            new ConeVolume.ConeDescription { Origin = Vector3.zero, Rotation = rotation, Distance = distance, Step = 1, Tangents = tangents };

        // The original callback's containment: the point measured from its projection on the axis.
        private static bool Original(ConeVolume.ConeDescription cone, Vector3 point)
        {
            Vector3 forward = cone.Rotation * Vector3.forward;
            Vector3 pointOnLine = MathUtilities.GetClosestPointOnLine(cone.Origin, forward, point);
            float depth = Vector3.Dot(pointOnLine - cone.Origin, forward);
            return ConeVolume.IsPositionInsideCone(point, pointOnLine, cone.Tangents, depth, Quaternion.Inverse(cone.Rotation));
        }

        // The replaced expression: the same depth, measured from the cone origin.
        private static bool FromOrigin(ConeVolume.ConeDescription cone, Vector3 point)
        {
            Vector3 forward = cone.Rotation * Vector3.forward;
            Vector3 pointOnLine = MathUtilities.GetClosestPointOnLine(cone.Origin, forward, point);
            float depth = Vector3.Dot(pointOnLine - cone.Origin, forward);
            return ConeVolume.IsPositionInsideCone(point, cone.Origin, cone.Tangents, depth, Quaternion.Inverse(cone.Rotation));
        }

        private static void ExpectHit(ConeVolume.ConeDescription cone, Vector3 point, string label = "")
        {
            var surface = new FixedPointSurface(point);
            SurfaceLocation location = default;
            Assert.That(SurfaceTrackingUtilities.TryGetProjectedConeSurfaceHit(surface.ForEach, cone, null, ref location),
                Is.True, label);
            Assert.That(location.m_surface, Is.SameAs(surface), label);
            Assert.That(location.m_worldPosition, Is.EqualTo(point), label);
        }

        private static void ExpectMiss(ConeVolume.ConeDescription cone, Vector3 point, string label = "")
        {
            var surface = new FixedPointSurface(point);
            // A miss still replaces the caller's location with the empty one.
            var location = new SurfaceLocation { m_surface = new FixedPointSurface(Vector3.one), m_worldPosition = Vector3.one };
            Assert.That(SurfaceTrackingUtilities.TryGetProjectedConeSurfaceHit(surface.ForEach, cone, null, ref location),
                Is.False, label);
            Assert.That(location.m_surface, Is.Null, label);
            Assert.That(location.m_worldPosition, Is.EqualTo(Vector3.zero), label);
        }

        // An active surface whose nearest location is always one world point, inside bounds that contain any cone.
        private sealed class FixedPointSurface : ISurface
        {
            private readonly Vector3 point;
            public FixedPointSurface(Vector3 point) { this.point = point; }
            public void ForEach(Action<ISurface> callback) => callback(this);
            public SurfaceLocation FindNearestSurfaceLocationFromWorld(Vector3 worldPosition) =>
                new SurfaceLocation { m_surface = this, m_worldPosition = point };
            public Bounds GetBoundingBox(bool inWorldSpace = false) => new Bounds(Vector3.zero, Vector3.one * 1000);
            public bool IsActive() => true;
            public DateTime GetTimestamp() => throw new NotSupportedException();
            public MetadataGroups GetSurfaceMetadata() => throw new NotSupportedException();
            public SurfaceLocation FindNearestSurfaceLocationFromLocal(Vector3 localPosition) => throw new NotSupportedException();
            public float Length => throw new NotSupportedException();
            public float LengthInverse => throw new NotSupportedException();
            public Vector3 TransformPoint(Vector3 position) => throw new NotSupportedException();
            public Vector3 InverseTransformPoint(Vector3 position) => throw new NotSupportedException();
        }

        // CalculateConeBounds fills ConeVolume's shared segment table on first use. The borrowed array identity and
        // its contents are put back as found, even when a case fails.
        private sealed class ConeTableLease : IDisposable
        {
            private static readonly FieldInfo Table = typeof(ConeVolume).GetField("s_coneSegmentAngles", BindingFlags.Static | BindingFlags.NonPublic);
            private readonly Vector2[] saved;
            private readonly Vector2[] contents;

            public ConeTableLease()
            {
                Assert.That(Table, Is.Not.Null);
                saved = (Vector2[])Table.GetValue(null);
                contents = saved == null ? null : (Vector2[])saved.Clone();
            }

            public void Dispose()
            {
                Table.SetValue(null, saved);
                if (saved != null) Array.Copy(contents, saved, contents.Length);
            }
        }
    }
}
