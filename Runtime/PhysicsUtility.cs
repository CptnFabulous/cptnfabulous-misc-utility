using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CptnFabulous.MiscUtility
{
    public static class PhysicsUtility
    {
        #region Detection

        static Dictionary<int, LayerMask> physicsLayerDictionary = new Dictionary<int, LayerMask>();

        public static bool IsLayerInLayerMask(LayerMask mask, int layer) => mask == (mask | (1 << layer));

        public static LayerMask GetPhysicsLayerMask(int currentLayer)
        {
            if (physicsLayerDictionary.ContainsKey(currentLayer))
            {
                return physicsLayerDictionary[currentLayer];
            }

            int finalMask = 0;
            for (int i = 0; i < 32; i++)
            {
                bool ignore = Physics.GetIgnoreLayerCollision(currentLayer, i);
                if (!ignore) finalMask = finalMask | (1 << i);
            }

            physicsLayerDictionary[currentLayer] = finalMask;

            return finalMask;
        }

        #endregion

        #region Collisions

        public static Vector3 GetAverageCollisionNormal(Collision collision)
        {
            Vector3 normal = Vector3.zero;
            for (int i = 0; i < collision.contactCount; i++)
            {
                normal += collision.GetContact(i).normal;
            }
            normal /= collision.contactCount;
            return normal;
        }
        public static Vector3 GetAverageCollisionPoint(Collision collision)
        {
            Vector3 position = Vector3.zero;
            for (int i = 0; i < collision.contactCount; i++)
            {
                position += collision.GetContact(i).point;
            }
            position /= collision.contactCount;
            return position;
        }

        #endregion

        #region Colliders

        public static Vector3 ClosestPointOnColliders(IList<Collider> colliders, Vector3 point)
        {
            Collider closest = CollectionUtility.GetBest(colliders, (c) => Vector3.Distance(point, c.ClosestPoint(point)), true);
            return closest.ClosestPoint(point);
        }

        #endregion
    }
    /*
    public static class PhysicsCache
    {
        static Dictionary<Rigidbody, Rigidbody> rootDictionary = new Dictionary<Rigidbody, Rigidbody>();
        static Dictionary<Rigidbody, float> massDictionary = new Dictionary<Rigidbody, float>();
        static Dictionary<Rigidbody, Rigidbody[]> childDictionary = new Dictionary<Rigidbody, Rigidbody[]>();
        static Dictionary<Rigidbody, Collider[]> colliderDictionary = new Dictionary<Rigidbody, Collider[]>();

        public static Rigidbody GetRootRigidbody(Rigidbody rb)
        {
            if (rb == null) return null;

            if (rootDictionary.TryGetValue(rb, out Rigidbody r)) return r;

            // Check for a joint, and get its connected body.
            // If neither are found, break the loop.
            // Whatever was last assigned to 'root' is what we need.
            Rigidbody root = rb;
            while (root.TryGetComponent(out Joint j) && j.connectedBody != null)
            {
                // If we found a connected body, re-iterate the check until we reach the end of the chain.
                root = j.connectedBody;
            }
            rootDictionary[rb] = root;

            return root;
        }
        public static float TotalMassOfConnectedRigidbodies(Rigidbody rb)
        {
            if (rb == null) return 1;

            rb = GetRootRigidbody(rb);

            float mass;
            // If a value is already stored, get that
            if (massDictionary.TryGetValue(rb, out mass)) return mass;

            // Calculate total mass from child rigidbodies, and cache it to save processing
            foreach (Rigidbody child in GetChildRigidbodies(rb))
            {
                mass += child.mass;
            }
            massDictionary[rb] = mass;

            return mass;
        }
        public static Vector3 AverageVelocityOfConnectedRigidbodies(Rigidbody rb)
        {
            if (rb == null) return Vector3.zero;

            rb = GetRootRigidbody(rb);

            // Calculate total mass from child rigidbodies, and cache it to save processing
            Vector3 totalVelocity = Vector3.zero;
            int numberOfBodies = 0;
            foreach (Rigidbody child in GetChildRigidbodies(rb))
            {
                totalVelocity += child.velocity;
                numberOfBodies++;
            }
            totalVelocity /= numberOfBodies;

            return totalVelocity;
        }
        public static Rigidbody[] GetChildRigidbodies(Rigidbody target)
        {
            target = GetRootRigidbody(target);
            // Check for pre-cached value
            if (childDictionary.TryGetValue(target, out var array)) return array;
            // Find and cache value
            childDictionary[target] = target.GetComponentsInChildren<Rigidbody>();
            return childDictionary[target];
        }
        public static Collider[] GetChildColliders(Rigidbody target)
        {
            target = GetRootRigidbody(target);
            // Check for pre-cached value
            if (colliderDictionary.TryGetValue(target, out var array)) return array;
            // Find and cache value
            colliderDictionary[target] = target.GetComponentsInChildren<Collider>();
            return colliderDictionary[target];
        }

    }
    */
}