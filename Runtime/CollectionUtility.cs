using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollectionUtility
{
    public static bool ArrayContains<T>(IEnumerable<T> array, T data)
    {
        foreach (T t in array)
        {
            if (t.Equals(data)) return true;
        }
        return false;
    }
    public static int IndexOfInArray<T>(IList<T> array, T data) => IndexOfInArray(array, data, 0, array.Count);
    public static int IndexOfInArray<T>(IList<T> array, T data, int start, int length)
    {
        for (int i = start; i < start + length; i++)
        {
            if (array[i].Equals(data)) return i;
        }
        return -1;
    }
    public static Vector3Int IndexOfIn3DArray(Array array, object data)
    {
        int xLength = array.GetLength(0);
        int yLength = array.GetLength(1);
        int zLength = array.GetLength(2);
        for (int x = 0; x < xLength; x++)
        {
            for (int y = 0; y < yLength; y++)
            {
                for (int z = 0; z < zLength; z++)
                {
                    if (array.GetValue(x, y, z).Equals(data)) return new Vector3Int(x, y, z);
                }
            }
        }
        return -Vector3Int.one;
    }
    public static void IterateThroughGrid(Vector3Int dimensions, Action<int, int, int> action)
    {
        for (int x = 0; x < dimensions.x; x++)
        {
            for (int y = 0; y < dimensions.y; y++)
            {
                for (int z = 0; z < dimensions.z; z++)
                {
                    action.Invoke(x, y, z);
                }
            }
        }
    }
    public static void IterateThroughGrid(Vector3Int dimensions, Action<Vector3Int> action)
    {
        for (int x = 0; x < dimensions.x; x++)
        {
            for (int y = 0; y < dimensions.y; y++)
            {
                for (int z = 0; z < dimensions.z; z++)
                {
                    action.Invoke(new Vector3Int(x, y, z));
                }
            }
        }
    }
    public static bool IsIndexOutsideArray(Vector3Int dimensions, Vector3Int indices)
    {
        for (int i = 0; i < 3; i++)
        {
            if (indices[i] < 0) return true;
            if (indices[i] >= dimensions[i]) return true;
        }

        return false;
    }

    public static void IterateThroughGrid(Action<int[]> action, params int[] dimensions)
    {
        int[] indices = new int[dimensions.Length];
        IncrementArrayRank(ref dimensions, 0, ref indices, ref action);

        static void IncrementArrayRank(ref int[] dimensions, int axis, ref int[] indices, ref System.Action<int[]> action)
        {
            // If last axis was assigned, invoke the actual action
            if (axis == dimensions.Length)
            {
                action.Invoke(indices);
                return;
            }

            // If not, assign the current axis, and nest this function to start iterating through the next one
            int nextAxis = axis + 1;
            for (int i = 0; i < dimensions[axis]; i++)
            {
                indices[axis] = i;
                IncrementArrayRank(ref dimensions, nextAxis, ref indices, ref action);
            }
        }
    }
    public static bool IsIndexOutsideArray(int[] dimensions, int[] indices)
    {
        for (int i = 0; i < dimensions.Length; i++)
        {
            if (indices[i] < 0) return true;
            if (indices[i] >= dimensions[i]) return true;
        }

        return false;
    }

    public static void ShuffleList<T>(IList<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int r = UnityEngine.Random.Range(0, list.Count - 1);
            if (r == i) continue;
            T value = list[i];
            list[i] = list[r];
            list[r] = value;
        }
    }
    public static void SortListWithOnePredicate<T>(List<T> list, System.Func<T, IComparable> obtainValue, bool reverse = false)
    {
        list.Sort((a, b) =>
        {
            IComparable _a = obtainValue.Invoke(a);
            IComparable _b = obtainValue.Invoke(b);
            return reverse ? _b.CompareTo(_a) : _a.CompareTo(_b);
        });
    }
    public static void SortListWithNoPredicate<T>(List<T> list, bool reverse = false) where T : IComparable
    {
        list.Sort((_a, _b) => reverse ? _b.CompareTo(_a) : _a.CompareTo(_b));
    }
    public static T GetBest<T>(System.Func<T, IComparable> criteria, bool reverse, params T[] options) => GetBest(options, criteria, reverse);
    public static T GetBest<T>(IList<T> collection, System.Func<T, IComparable> criteria, bool reverse = false)
    {
        //if (options == null || options.Length == 0) return default(T);



        T best = collection[0];
        IComparable bestValue = criteria.Invoke(best);

        for (int i = 1; i < collection.Count; i++)
        {
            T next = collection[i];
            IComparable nextValue = criteria.Invoke(next);

            int comparison = reverse ? nextValue.CompareTo(bestValue) : bestValue.CompareTo(nextValue);
            if (comparison < 0)
            {
                best = next;
                bestValue = nextValue;
            }
        }

        return best;
    }
}
