using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CptnFabulous.MiscUtility
{
    public static class DebugUtility
    {
        public static void DebugLogMultiple<T>(params T[] values)
        {
            DebugLogMultiple(values, 0, values.Length);
        }
        public static void DebugLogMultiple<T>(IList<T> values, int start, int length)
        {
            string s = "";
            for (int i = start; i < start + length; i++)
            {
                s += values[i].ToString();
                s += ", ";
            }
            Debug.Log(s);
        }
        public static void DrawDebugWireCube(Vector3 position, Vector3 size, Color colour, float duration = 0)
        {
            for (int e = 0; e < 12; e++)
            {
                Vector3 pos1 = VoxelUtility.corners[VoxelUtility.edges[e].Item1];
                Vector3 pos2 = VoxelUtility.corners[VoxelUtility.edges[e].Item2];

                Vector3 min = position - (0.5f * size);
                for (int a = 0; a < 3; a++)
                {
                    pos1[a] = min[a] + pos1[a] * size[a];
                    pos2[a] = min[a] + pos2[a] * size[a];
                }
                Debug.DrawLine(pos1, pos2, colour, duration);
            }
        }
        public static void DrawAngledGizmoFrustum(Vector3 position, float horizontal, float vertical, float minRange, float maxRange)
        {
            horizontal = Mathf.Max(horizontal, 0.001f);
            vertical = Mathf.Max(vertical, 0.001f);
            Gizmos.DrawFrustum(position, vertical, maxRange, minRange, horizontal / vertical);
        }
    }
}
