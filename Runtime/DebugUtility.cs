using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CptnFabulous.MiscUtility
{
    public static class DebugUtility
    {
        #region Logging
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
        #endregion

        #region Gizmos
        public static void DrawGizmoLineSequence(int positionCount, System.Func<int, Vector3> getPosition, bool loop)
        {
            for (int i = 1; i < positionCount; i++)
            {
                Vector3 from = getPosition.Invoke(i - 1);
                Vector3 to = getPosition.Invoke(i);
                Gizmos.DrawLine(from, to);
            }

            if (loop)
            {
                Vector3 final = getPosition.Invoke(positionCount - 1);
                Vector3 first = getPosition.Invoke(0);
                Gizmos.DrawLine(final, first);
            }
        }

        public static void DrawAngledGizmoFrustum(Vector3 position, float horizontal, float vertical, float minRange, float maxRange)
        {
            horizontal = Mathf.Max(horizontal, 0.001f);
            vertical = Mathf.Max(vertical, 0.001f);
            Gizmos.DrawFrustum(position, vertical, maxRange, minRange, horizontal / vertical);
        }

        #endregion

        #region Drawing debug lines

        #region Copied from a Unity library

        private static readonly Vector4[] s_UnitSphere = MakeUnitSphere(16);

        private static Vector4[] MakeUnitSphere(int len)
        {
            Debug.Assert(len > 2);
            var v = new Vector4[len * 3];
            for (int i = 0; i < len; i++)
            {
                var f = i / (float)len;
                float c = Mathf.Cos(f * (float)(Math.PI * 2.0));
                float s = Mathf.Sin(f * (float)(Math.PI * 2.0));
                v[0 * len + i] = new Vector4(c, s, 0, 1);
                v[1 * len + i] = new Vector4(0, c, s, 1);
                v[2 * len + i] = new Vector4(s, 0, c, 1);
            }
            return v;
        }

        public static void DrawDebugWireSphere(Vector4 pos, float radius, Color color, float duration = 0)
        {
            Vector4[] v = s_UnitSphere;
            int len = s_UnitSphere.Length / 3;
            for (int i = 0; i < len; i++)
            {
                var sX = pos + radius * v[0 * len + i];
                var eX = pos + radius * v[0 * len + (i + 1) % len];
                var sY = pos + radius * v[1 * len + i];
                var eY = pos + radius * v[1 * len + (i + 1) % len];
                var sZ = pos + radius * v[2 * len + i];
                var eZ = pos + radius * v[2 * len + (i + 1) % len];
                Debug.DrawLine(sX, eX, color, duration);
                Debug.DrawLine(sY, eY, color, duration);
                Debug.DrawLine(sZ, eZ, color, duration);
            }
        }

        #endregion
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
        #endregion
    }
}
