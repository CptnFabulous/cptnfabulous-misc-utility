using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CptnFabulous.MiscUtility
{
    public static class VoxelUtility
    {
        public static readonly Vector3Int[] adjacencies = new Vector3Int[6]
        {
            Vector3Int.left,
            Vector3Int.right,
            Vector3Int.down,
            Vector3Int.up,
            Vector3Int.back,
            Vector3Int.forward
        };
        public static readonly int[][] faceCornersForAdjacentSquares = new int[6][]
        {
            // Ints need to be oriented top left, top right, bottom left, bottom right
            new int[] { 3, 1, 2, 0 }, // Left
            new int[] { 4, 5, 6, 7 }, // Right
            new int[] { 0, 4, 2, 6 }, // Down
            new int[] { 1, 3, 5, 7 }, // Up
            new int[] { 0, 1, 4, 5 }, // Backward
            new int[] { 7, 3, 6, 2 }, // Forward
        };
        public static readonly Vector3Int[] corners = new Vector3Int[8]
        {
            //    3 -------- 7
            //   /|         /|
            //  / |        / |
            // 1 -------- 5  |
            // |  |       |  |
            // Y  2 ------|- 6
            // | Z        | /
            // |/         |/
            // 0 --X----- 4
            new Vector3Int(0, 0, 0),
            new Vector3Int(0, 1, 0),
            new Vector3Int(0, 0, 1),
            new Vector3Int(0, 1, 1),
            new Vector3Int(1, 0, 0),
            new Vector3Int(1, 1, 0),
            new Vector3Int(1, 0, 1),
            new Vector3Int(1, 1, 1),
        };

        public static readonly (int, int)[] edges = new (int, int)[12]
        {
            // The second corner will always be after the first corner on whatever axis they run across.
            (0, 4), // X, 0, 0
            (1, 5), // X, 1, 0
            (2, 6), // X, 0, 1
            (3, 7), // X, 1, 1
            (0, 1), // 0, Y, 0
            (4, 5), // 1, Y, 0
            (2, 3), // 0, Y, 1
            (6, 7), // 1, Y, 1
            (0, 2), // 0, 0, Z
            (4, 6), // 1, 0, Z
            (1, 3), // 0, 1, Z
            (5, 7), // 1, 1, Z
        };

        public static readonly Vector3 globalVertexOffset = new Vector3(0.5f, 0.5f, 0.5f);
    }
}
