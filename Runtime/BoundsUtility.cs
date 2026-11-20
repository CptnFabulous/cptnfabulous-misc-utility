using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CptnFabulous.MiscUtility
{
    public static class BoundsUtility
    {
        public static Bounds CombinedBounds(Vector3[] positions)
        {
            Bounds b = new Bounds(positions[0], Vector3.zero);
            for (int i = 1; i < positions.Length; i++)
            {
                b.Encapsulate(positions[i]);
            }
            return b;
        }
        public static Bounds CombinedBounds(Bounds[] bounds)
        {
            Bounds b = bounds[0];
            for (int i = 1; i < bounds.Length; i++)
            {
                b.Encapsulate(bounds[i]);
            }
            return b;
        }
        public static Bounds CombinedBounds(IList<Collider> colliders)
        {
            Bounds b = colliders[0].bounds;
            for (int i = 1; i < colliders.Count; i++)
            {
                b.Encapsulate(colliders[i].bounds);
            }
            return b;
        }

        public static Bounds TransformBounds(this Bounds bounds, Matrix4x4 matrix) => TransformBounds(bounds.center, bounds.size, matrix);
        public static Bounds TransformBounds(Vector3 centre, Vector3 size, Matrix4x4 matrix)
        {
            // Convert position and size values from local to world space
            centre = matrix.MultiplyPoint3x4(centre);
            size = matrix.MultiplyVector(size);
            // Return final bounds
            return new Bounds(centre, size);
        }

        public static Vector3 FurthestPointOnBounds(this Bounds bounds, Vector3 position)
        {
            // Get direction from position to bounds, then add onto bounds so we get 2x that distance (i.e. exactly opposite relative to bounds centre)
            Vector3 difference = bounds.center - position;
            Vector3 oppositeOfPosition = bounds.center + difference;
            return bounds.ClosestPoint(oppositeOfPosition);
        }
    }
}
