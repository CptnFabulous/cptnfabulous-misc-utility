using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CptnFabulous.MiscUtility
{
    public static class TransformUtility
    {
        #region Vector3s
        
        static readonly Vector3[] axes = new Vector3[]
        {
            Vector3.right, Vector3.up, Vector3.forward
        };

        public static Vector3 ScreenToAnchoredPosition(Vector3 screenSpace, RectTransform rt, RectTransform parent)
        {
            Vector3 canvasSpace = screenSpace;
            // Multiplies screen space values to convert them to the canvas' dimensions.
            canvasSpace.x = canvasSpace.x / Screen.width;
            canvasSpace.y = canvasSpace.y / Screen.height;
            canvasSpace *= parent.rect.size;
            // Calculates the rectTransform's anchor reference point
            Vector2 anchorDimensions = rt.anchorMin + ((rt.anchorMax - rt.anchorMin) / 2);
            // Multiplies by canvas rect dimensions to get an offset
            Vector3 anchorOffset = anchorDimensions * parent.rect.size;
            return canvasSpace - anchorOffset; // Adds offset to canvas space to produce an anchored position
        }
        public static Vector3 ClampDirection(Vector3 direction, Vector3 reference, float maxAngle)
        {
            float angle = Vector3.Angle(direction, reference);
            if (angle > maxAngle)
            {
                direction = Vector3.RotateTowards(direction, reference, angle - maxAngle, 0);
            }
            return direction;
        }
        public static Vector3 Vector3Clamp(Vector3 value, Vector3 min, Vector3 max)
        {
            for (int i = 0; i < 3; i++)
            {
                value[i] = Mathf.Clamp(value[i], min[i], max[i]);
            }
            return value;
        }
        public static Vector3Int Vector3IntClamp(Vector3Int value, Vector3Int min, Vector3Int max)
        {
            for (int i = 0; i < 3; i++)
            {
                value[i] = Mathf.Clamp(value[i], min[i], max[i]);
            }
            return value;
        }
        public static int ClosestAxis(Vector3 direction)
        {
            int bestAxis = 0;
            float furthestFromZero = 0;
            for (int i = 0; i < 3; i++)
            {
                //Vector3 axis = Vector3.zero;
                //axis[i] = 1;

                float absDot = Mathf.Abs(Vector3.Dot(direction, axes[i]));
                if (absDot > furthestFromZero)
                {
                    bestAxis = i;
                    furthestFromZero = absDot;
                }
            }

            return bestAxis;
        }
        public static bool PointIsInsideCamera(Camera camera, Vector3 worldPoint)
        {
            Vector3 viewportPoint = camera.WorldToViewportPoint(worldPoint);
            if (viewportPoint.x < 0) return false;
            if (viewportPoint.x > 1) return false;
            if (viewportPoint.y < 0) return false;
            if (viewportPoint.y > 1) return false;
            return true;
        }

        #endregion

        #region Quaternions
        public static Quaternion DifferenceBetweenRotations(Quaternion from, Quaternion to) => to * Quaternion.Inverse(from);
        public static Quaternion WorldToLocalRotation(Quaternion worldRotation, Transform target) => Quaternion.Inverse(target.rotation) * worldRotation;
        #endregion

        #region Bounds
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
        #endregion
    }
}