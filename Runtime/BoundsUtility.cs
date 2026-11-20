using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CptnFabulous.MiscUtility
{
    public static class BoundsUtility
    {
        public static float Volume(this Bounds bounds) => bounds.size.Volume();

        public static Bounds LocalBounds(this Collider c)
        {
            if (c is BoxCollider boxCollider)
            {
                return new Bounds(boxCollider.center, boxCollider.size);
            }
            else if (c is SphereCollider sphereCollider)
            {
                Vector3 extents = new Vector3(sphereCollider.radius, sphereCollider.radius, sphereCollider.radius);
                return new Bounds(sphereCollider.center, 2 * extents);
            }
            else if (c is CapsuleCollider capsuleCollider)
            {
                // Determine which vector axes represent the height and diameter of the capsule
                VoxelUtility.DetermineDifferentOf3(0, 1, 2, out int direction, out int radius0, out int radius1, out _, (i) => i == capsuleCollider.direction);
                // Generate the size
                float diameter = 2 * capsuleCollider.radius;
                Vector3 size = new Vector3();
                size[direction] = capsuleCollider.height;
                size[radius0] = diameter;
                size[radius1] = diameter;
                // Create the final bounds
                return new Bounds(capsuleCollider.center, size);
            }
            else if (c is MeshCollider meshCollider)
            {
                return meshCollider.sharedMesh.bounds;
            }

            // If there isn't a solution for this type of collider, just return an empty one
            // (Local space means it'll still represent the transform position when converted to other spaces)
            return new Bounds();
        }

        public static Vector3 WorldSpaceDimensions(this Collider c) => WorldSpaceDimensions(c.LocalBounds(), c.transform.localToWorldMatrix);
        public static Vector3 WorldSpaceDimensions(Bounds localBounds, Matrix4x4 localToWorldMatrix)
        {
            Vector3 min = localBounds.min;
            Vector3 max = localBounds.max;

            // Create vectors that shift from min on each axis
            Vector3 offsetX = min;
            Vector3 offsetY = min;
            Vector3 offsetZ = min;
            offsetX.x = max.x;
            offsetY.y = max.y;
            offsetZ.z = max.z;

            // Transform points to world space
            min = localToWorldMatrix.MultiplyPoint3x4(min);
            offsetX = localToWorldMatrix.MultiplyPoint3x4(offsetX);
            offsetY = localToWorldMatrix.MultiplyPoint3x4(offsetY);
            offsetZ = localToWorldMatrix.MultiplyPoint3x4(offsetZ);

            // Get the distances from min to each offsetPoint
            float x = Vector3.Distance(min, offsetX);
            float y = Vector3.Distance(min, offsetY);
            float z = Vector3.Distance(min, offsetZ);

            // We have the dimensions!
            return new Vector3(x, y, z);
        }













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
        public static Bounds CombineBoundsInTransformSpace(IList<Collider> colliders, Transform transformSpace)
        {
            if (colliders.Count == 0) return new Bounds();

            Bounds final = colliders[0].ConsistentBoundsInTransformSpace(transformSpace);
            for (int i = 1; i < colliders.Count; i++)
            {
                Bounds b = colliders[i].ConsistentBoundsInTransformSpace(transformSpace);
                final.Encapsulate(b);
            }
            return final;
        }
        static Bounds ConsistentBoundsInTransformSpace(this Collider c, Transform t) => c.ConsistentBounds().TransformBounds(t.worldToLocalMatrix);
        static Bounds ConsistentBounds(this Collider c)
        {
            Matrix4x4 localToWorld = c.transform.localToWorldMatrix;
            return c.LocalBounds().TransformBounds(localToWorld);
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
