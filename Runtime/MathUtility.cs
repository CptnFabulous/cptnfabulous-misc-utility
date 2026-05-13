using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CptnFabulous.MiscUtility
{
    public static class MathUtility
    {
        public static float Min(params float[] values)
        {
            float final = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                final = Mathf.Min(final, values[i]);
            }
            return final;
        }
        public static float Max(params float[] values)
        {
            float final = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                final = Mathf.Max(final, values[i]);
            }
            return final;
        }
        /// <summary>
        /// If the value exceeds the specified range, loop back around to the start.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float Loop(float value, float min, float max)
        {
            if (value > max)
            {
                // Includes difference
                return min + (value - max);
            }
            else if (value < min)
            {
                // Includes difference
                return max - (min - value);
            }
            else
            {
                return value;
            }
        }
        public static int Loop(int value, int min, int max) => (int)Loop((float)value, (float)min, (float)max);
        public static int LoopIndex(int value, int length)
        {
            // If the value has gone backwards, start at the max length and subtract by how far the value has been incremented
            while (value < 0) value = length + value;
            // If the value goes over the length, subtract the length to represent how the value has looped
            while (value >= length) value = value - length;

            return value;
        }
        public static bool WithinRange(float value, float min, float max) => value >= min && value <= max;
        public static bool WithinArray(int index, int arrayLength) => WithinRange(index, 0, arrayLength - 1);
        /// <summary>
        /// Multiply a base value by the return value to figure out how it falls off over distance.
        /// </summary>
        public static float InverseSquareValueMultiplier(float distance) => distance > 0 ? 1 / (distance * distance) : 1;
        public static float RoundToDecimalPlaces(float value, int decimalPlaces)
        {
            decimalPlaces = Mathf.Max(decimalPlaces, 0); // Ensure it's not less than zero
            float multiplier = Mathf.Pow(10, decimalPlaces);
            value *= multiplier;
            value = Mathf.RoundToInt(value);
            value /= multiplier;
            return value;
        }
        public static float LengthOfDiagonal(float width, float height) => Mathf.Sqrt((width * width) + (height * height));
        public static float CubeRoot(float cube) => Mathf.Pow(cube, 1f / 3f);
    }

}

