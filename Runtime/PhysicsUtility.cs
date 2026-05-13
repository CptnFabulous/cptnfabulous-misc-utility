using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CptnFabulous.MiscUtility
{
    public static class PhysicsUtility
    {
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
    }
}