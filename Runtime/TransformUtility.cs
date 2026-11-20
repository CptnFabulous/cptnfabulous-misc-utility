using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CptnFabulous.MiscUtility
{
    public static class TransformUtility
    {
        static readonly Vector3[] axes = new Vector3[]
        {
            Vector3.right, Vector3.up, Vector3.forward
        };

        public static float Volume(this Vector3 dimensions) => Mathf.Abs(dimensions.x) * Mathf.Abs(dimensions.y) * Mathf.Abs(dimensions.z);

        #region RectTransforms and screen-space
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
        public static bool WorldToRect(Vector3 worldPosition, Vector2 worldSize, out Rect rect)
        {
            return WorldToRect(worldPosition, worldSize, Camera.current, out rect);
        }
        /// <summary>
        /// Creates a rect framing a specific size and position within the scene.
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <param name="worldSize"></param>
        /// <param name="targetCamera"></param>
        /// <param name="rect"></param>
        /// <returns>Returns false if none of the rect would be visible through the target camera.</returns>
        public static bool WorldToRect(Vector3 worldPosition, Vector2 worldSize, Camera targetCamera, out Rect rect)
        {
            rect = new Rect();
            if (targetCamera == null) return false;

            Vector3 screenPoint = targetCamera.WorldToScreenPoint(worldPosition);

            Vector3 forward = targetCamera.ScreenPointToRay(screenPoint, targetCamera.stereoActiveEye).direction;
            Vector3 up = targetCamera.transform.up;
            Vector3 right = Vector3.Cross(forward, up).normalized;

            // Figure out corners
            Vector2 extents = 0.5f * worldSize;
            Vector3 tr = extents.x * right + extents.y * up;
            Vector3 bl = -tr;
            Vector3 br = extents.x * right + extents.y * -up;
            Vector3 tl = -br;
            // Add onto base position
            bl += worldPosition;
            br += worldPosition;
            tl += worldPosition;
            tr += worldPosition;

            // Calculate rect
            rect.min = targetCamera.WorldToScreenPoint(bl);
            rect.max = targetCamera.WorldToScreenPoint(tr);

            // Determine if this rect is actually on the side of the viewing camera
            Plane p = new Plane(targetCamera.transform.forward, targetCamera.transform.position);
            bool visible = p.GetSide(bl) || p.GetSide(br) || p.GetSide(tl) || p.GetSide(tr);

            return visible;
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

        #region Clamping
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

        #endregion

        #region Quaternions
        public static Quaternion DifferenceBetweenRotations(Quaternion from, Quaternion to) => to * Quaternion.Inverse(from);
        public static Quaternion WorldToLocalRotation(Quaternion worldRotation, Transform target) => Quaternion.Inverse(target.rotation) * worldRotation;
        #endregion
    }
}