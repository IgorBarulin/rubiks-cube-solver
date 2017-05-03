using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.CubeModel
{
    public class CubeFactory
    {
        #region consts

        private static readonly byte[][] _cornerFacelet = new byte[Cube.CORNERS_AMOUNT][]
        {
            new byte[Cube.FACELETS_ON_CORNER_AMOUNT] {04, 08, 18}, // URF - 0
            new byte[Cube.FACELETS_ON_CORNER_AMOUNT] {06, 16, 26}, // UFL - 1
            new byte[Cube.FACELETS_ON_CORNER_AMOUNT] {00, 24, 34}, // ULB - 2
            new byte[Cube.FACELETS_ON_CORNER_AMOUNT] {02, 32, 10}, // URB - 3
            new byte[Cube.FACELETS_ON_CORNER_AMOUNT] {42, 20, 14}, // DFR - 4
            new byte[Cube.FACELETS_ON_CORNER_AMOUNT] {40, 28, 22}, // DLF - 5
            new byte[Cube.FACELETS_ON_CORNER_AMOUNT] {46, 36, 30}, // DBL - 6
            new byte[Cube.FACELETS_ON_CORNER_AMOUNT] {44, 12, 38}  // DRB - 7
        };

        private static readonly byte[][] _edgeFacelet = new byte[Cube.EDGES_AMOUNT][]
        {
            new byte[Cube.FACELETS_ON_EDGE_AMOUNT] {03, 09}, // UR - 0
            new byte[Cube.FACELETS_ON_EDGE_AMOUNT] {05, 17}, // UF - 1
            new byte[Cube.FACELETS_ON_EDGE_AMOUNT] {07, 25}, // UL - 2
            new byte[Cube.FACELETS_ON_EDGE_AMOUNT] {01, 33}, // UB - 3
            new byte[Cube.FACELETS_ON_EDGE_AMOUNT] {43, 13}, // DR - 4
            new byte[Cube.FACELETS_ON_EDGE_AMOUNT] {41, 21}, // DF - 5
            new byte[Cube.FACELETS_ON_EDGE_AMOUNT] {47, 29}, // DL - 6
            new byte[Cube.FACELETS_ON_EDGE_AMOUNT] {45, 37}, // DB - 7
            new byte[Cube.FACELETS_ON_EDGE_AMOUNT] {19, 15}, // FR - 8
            new byte[Cube.FACELETS_ON_EDGE_AMOUNT] {23, 27}, // FL - 9
            new byte[Cube.FACELETS_ON_EDGE_AMOUNT] {35, 31}, // BL - 10
            new byte[Cube.FACELETS_ON_EDGE_AMOUNT] {39, 11}  // BR - 11
        };

        private static readonly byte[][] _cornerMap = new byte[Cube.CORNERS_AMOUNT][]
        {
            new byte[Cube.FACELETS_ON_CORNER_AMOUNT] {0, 1, 2}, // URF - 0
            new byte[Cube.FACELETS_ON_CORNER_AMOUNT] {0, 2, 3}, // UFL - 1
            new byte[Cube.FACELETS_ON_CORNER_AMOUNT] {0, 3, 4}, // ULB - 2
            new byte[Cube.FACELETS_ON_CORNER_AMOUNT] {0, 4, 1}, // URB - 3
            new byte[Cube.FACELETS_ON_CORNER_AMOUNT] {5, 2, 1}, // DRF - 4
            new byte[Cube.FACELETS_ON_CORNER_AMOUNT] {5, 3, 2}, // DLF - 5
            new byte[Cube.FACELETS_ON_CORNER_AMOUNT] {5, 4, 3}, // DBL - 6
            new byte[Cube.FACELETS_ON_CORNER_AMOUNT] {5, 1, 4}  // DRB - 7 
        };

        private static readonly byte[][] _edgeMap = new byte[Cube.EDGES_AMOUNT][]
        {
            new byte[Cube.FACELETS_ON_EDGE_AMOUNT] {0, 1}, // UR - 0
            new byte[Cube.FACELETS_ON_EDGE_AMOUNT] {0, 2}, // UF - 1
            new byte[Cube.FACELETS_ON_EDGE_AMOUNT] {0, 3}, // UL - 2
            new byte[Cube.FACELETS_ON_EDGE_AMOUNT] {0, 4}, // UB - 3
            new byte[Cube.FACELETS_ON_EDGE_AMOUNT] {5, 1}, // DR - 4
            new byte[Cube.FACELETS_ON_EDGE_AMOUNT] {5, 2}, // DF - 5
            new byte[Cube.FACELETS_ON_EDGE_AMOUNT] {5, 3}, // DL - 6
            new byte[Cube.FACELETS_ON_EDGE_AMOUNT] {5, 4}, // DB - 7
            new byte[Cube.FACELETS_ON_EDGE_AMOUNT] {2, 1}, // FR - 8
            new byte[Cube.FACELETS_ON_EDGE_AMOUNT] {2, 3}, // FL - 9
            new byte[Cube.FACELETS_ON_EDGE_AMOUNT] {4, 3}, // BL - 10
            new byte[Cube.FACELETS_ON_EDGE_AMOUNT] {4, 1}  // BR - 11
        };

        #endregion

        public Cube CreateCube(byte[] faceletColors)
        {
            Cubie[] corners = new Cubie[Cube.CORNERS_AMOUNT];
            Cubie[] edges = new Cubie[Cube.EDGES_AMOUNT];

            if (faceletColors == null)
            {
                for (byte c = 0; c < Cube.CORNERS_AMOUNT; c++)
                {
                    corners[c] = new Cubie(c, 0);
                }

                for (byte e = 0; e < Cube.EDGES_AMOUNT; e++)
                {
                    edges[e] = new Cubie(e, 0);
                }
            }
            else if (faceletColors.Length != Cube.FACELETS_AMOUNT)
            {
                throw new Exception("Incorrect faceletColors");
            }
            else
            {
                for (int i = 0; i < Cube.CORNERS_AMOUNT; i++)
                {
                    byte[] tuple = _cornerFacelet[i];
                    byte[] cubie = tuple.Select(x => faceletColors[x]).ToArray();
                    int position = _cornerMap.Index(cubie, Tools.SetEquals);
                    int orient = cubie.Index(_cornerMap[position][0]);
                    corners[i] = new Cubie((byte)position, (byte)orient);
                }

                for (int i = 0; i < Cube.EDGES_AMOUNT; i++)
                {
                    byte[] tuple = _edgeFacelet[i];
                    byte[] cubie = tuple.Select(x => faceletColors[x]).ToArray();
                    int position = _edgeMap.Index(cubie, Tools.SetEquals);
                    int orient = cubie.Index(_edgeMap[position][0]);
                    edges[i] = new Cubie((byte)position, (byte)orient);
                }
            }

            return new Cube(corners, edges);
        }
    }
}

