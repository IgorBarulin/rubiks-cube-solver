using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public struct Cubie
    {
        public byte Position { get; private set; }
        public byte Orient { get; private set; }

        public Cubie(byte pos, byte orient)
        {
            Position = pos;
            Orient = orient;
        }

        public override string ToString()
        {
            return "Cubie(" + Position.ToString() + ", " + Orient.ToString() + ")";
        }
    }

    public class Cube
    {

        #region consts

        /* Диаграмма, представляющая модель куба 3х3х3:
         
           Верхняя сторона  (U) (0)
           Правая сторона   (R) (1)
           Передняя сторона (F) (2)
           Левая сторона    (L) (3)
           Задняя сторона   (B) (4)
           Нижняя сторона   (D) (5)
           
                      +---------+
                      | 00 - 07 |
                      |    U    |
                      |         |
            +---------+---------+---------+---------+
            | 24 - 31 | 16 - 23 | 08 - 15 | 32 - 39 |
            |    L    |    F    |    R    |    B    |
            |         |         |         |         |
            +---------+---------+---------+---------+
                      | 40 - 47 |
                      |    D    |
                      |         |
                      +---------+
        */

        public const int CORNERS_AMOUNT = 8;
        public const int FACELETS_ON_CORNER_AMOUNT = 3;
        public const int EDGES_AMOUNT = 12;
        public const int FACELETS_ON_EDGE_AMOUNT = 2;
        public const int FACELETS_AMOUNT = 48;

        private static readonly byte[] _facelets = new byte[FACELETS_AMOUNT]
        {
            00, 01, 02, 03, 04, 05, 06, 07, // U - 0
            08, 09, 10, 11, 12, 13, 14, 15, // R - 1
            16, 17, 18, 19, 20, 21, 22, 23, // F - 2
            24, 25, 26, 27, 28, 29, 30, 31, // L - 3
            32, 33, 34, 35, 36, 37, 38, 39, // B - 4
            40, 41, 42, 43, 44, 45, 46, 47  // D - 5
        };

        private static readonly byte[][] _cornerMap = new byte[CORNERS_AMOUNT][]
        {
            new byte[FACELETS_ON_CORNER_AMOUNT] {0, 1, 2}, // URF - 0
            new byte[FACELETS_ON_CORNER_AMOUNT] {0, 2, 3}, // UFL - 1
            new byte[FACELETS_ON_CORNER_AMOUNT] {0, 3, 4}, // ULB - 2
            new byte[FACELETS_ON_CORNER_AMOUNT] {0, 4, 1}, // URB - 3
            new byte[FACELETS_ON_CORNER_AMOUNT] {5, 2, 1}, // DRF - 4
            new byte[FACELETS_ON_CORNER_AMOUNT] {5, 3, 2}, // DLF - 5
            new byte[FACELETS_ON_CORNER_AMOUNT] {5, 4, 3}, // DBL - 6
            new byte[FACELETS_ON_CORNER_AMOUNT] {5, 1, 4}  // DRB - 7 
        };

        private static readonly byte[][] _cornerFacelet = new byte[CORNERS_AMOUNT][]
        {
            new byte[FACELETS_ON_CORNER_AMOUNT] {04, 08, 18}, // URF - 0
            new byte[FACELETS_ON_CORNER_AMOUNT] {06, 16, 26}, // UFL - 1
            new byte[FACELETS_ON_CORNER_AMOUNT] {00, 24, 34}, // ULB - 2
            new byte[FACELETS_ON_CORNER_AMOUNT] {02, 32, 10}, // URB - 3
            new byte[FACELETS_ON_CORNER_AMOUNT] {42, 20, 14}, // DFR - 4
            new byte[FACELETS_ON_CORNER_AMOUNT] {40, 28, 22}, // DLF - 5
            new byte[FACELETS_ON_CORNER_AMOUNT] {46, 36, 30}, // DBL - 6
            new byte[FACELETS_ON_CORNER_AMOUNT] {44, 12, 38}  // DRB - 7
        };

        private static readonly byte[][] _edgeMap = new byte[EDGES_AMOUNT][]
        {
            new byte[FACELETS_ON_EDGE_AMOUNT] {0, 1}, // UR - 0
            new byte[FACELETS_ON_EDGE_AMOUNT] {0, 2}, // UF - 1
            new byte[FACELETS_ON_EDGE_AMOUNT] {0, 3}, // UL - 2
            new byte[FACELETS_ON_EDGE_AMOUNT] {0, 4}, // UB - 3
            new byte[FACELETS_ON_EDGE_AMOUNT] {5, 1}, // DR - 4
            new byte[FACELETS_ON_EDGE_AMOUNT] {5, 2}, // DF - 5
            new byte[FACELETS_ON_EDGE_AMOUNT] {5, 3}, // DL - 6
            new byte[FACELETS_ON_EDGE_AMOUNT] {5, 4}, // DB - 7
            new byte[FACELETS_ON_EDGE_AMOUNT] {2, 1}, // FR - 8
            new byte[FACELETS_ON_EDGE_AMOUNT] {2, 3}, // FL - 9
            new byte[FACELETS_ON_EDGE_AMOUNT] {4, 3}, // BL - 10
            new byte[FACELETS_ON_EDGE_AMOUNT] {4, 1}  // BR - 11
        };

        private static readonly byte[][] EdgeFacelet = new byte[EDGES_AMOUNT][]
        {
            new byte[FACELETS_ON_EDGE_AMOUNT] {03, 09}, // UR - 0
            new byte[FACELETS_ON_EDGE_AMOUNT] {05, 17}, // UF - 1
            new byte[FACELETS_ON_EDGE_AMOUNT] {07, 25}, // UL - 2
            new byte[FACELETS_ON_EDGE_AMOUNT] {01, 33}, // UB - 3
            new byte[FACELETS_ON_EDGE_AMOUNT] {43, 13}, // DR - 4
            new byte[FACELETS_ON_EDGE_AMOUNT] {41, 21}, // DF - 5
            new byte[FACELETS_ON_EDGE_AMOUNT] {47, 29}, // DL - 6
            new byte[FACELETS_ON_EDGE_AMOUNT] {45, 37}, // DB - 7
            new byte[FACELETS_ON_EDGE_AMOUNT] {19, 15}, // FR - 8
            new byte[FACELETS_ON_EDGE_AMOUNT] {23, 27}, // FL - 9
            new byte[FACELETS_ON_EDGE_AMOUNT] {35, 31}, // BL - 10
            new byte[FACELETS_ON_EDGE_AMOUNT] {39, 11}  // BR - 11
        };

        private static readonly string[] _validMoves = "U U2 U' R R2 R' F F2 F' D D2 D' L L2 L' B B2 B'".Split(' ');

        private static byte[,,] _cornerMoveTable = new byte[6, CORNERS_AMOUNT, 2] 
        // где 6 - количество сторон куба, а 2 - размер массива для хранения позиции и ориентации
        {
            //  0       1       2       3       4       5       6       7
            { {3, 0}, {0, 0}, {1, 0}, {2, 0}, {4, 0}, {5, 0}, {6, 0}, {7, 0} }, // U - 0
            { {4, 2}, {1, 0}, {2, 0}, {0, 1}, {7, 1}, {5, 0}, {6, 0}, {3, 2} }, // R - 1
            { {1, 1}, {5, 2}, {2, 0}, {3, 0}, {0, 2}, {4, 1}, {6, 0}, {7, 0} }, // F - 2
            { {0, 0}, {1, 0}, {2, 0}, {3, 0}, {5, 0}, {6, 0}, {7, 0}, {4, 0} }, // D - 3
            { {0, 0}, {2, 1}, {6, 2}, {3, 0}, {4, 0}, {1, 2}, {5, 1}, {7, 0} }, // L - 4
            { {0, 0}, {1, 0}, {3, 1}, {7, 2}, {4, 0}, {5, 0}, {2, 2}, {6, 1} }  // B - 5
        };

        private static byte[,,] _edgeMoveTable = new byte[6, EDGES_AMOUNT, 2]
        {
            //   0         1         2         3         4         5         6         7         8         9         10        11
            { {03, 00}, {00, 00}, {01, 00}, {02, 00}, {04, 00}, {05, 00}, {06, 00}, {07, 00}, {08, 00}, {09, 00}, {10, 00}, {11, 00} }, // U - 0
            { {08, 00}, {01, 00}, {02, 00}, {03, 00}, {11, 00}, {05, 00}, {06, 00}, {07, 00}, {04, 00}, {09, 00}, {10, 00}, {00, 00} }, // R - 1
            { {00, 00}, {09, 01}, {02, 00}, {03, 00}, {04, 00}, {08, 01}, {06, 00}, {07, 00}, {01, 01}, {05, 01}, {10, 00}, {11, 00} }, // F - 2
            { {00, 00}, {01, 00}, {02, 00}, {03, 00}, {05, 00}, {06, 00}, {07, 00}, {04, 00}, {08, 00}, {09, 00}, {10, 00}, {11, 00} }, // D - 3
            { {00, 00}, {01, 00}, {10, 00}, {03, 00}, {04, 00}, {05, 00}, {09, 00}, {07, 00}, {08, 00}, {02, 00}, {06, 00}, {11, 00} }, // L - 4
            { {00, 00}, {01, 00}, {02, 00}, {11, 01}, {04, 00}, {05, 00}, {06, 00}, {10, 01}, {08, 00}, {09, 00}, {03, 01}, {07, 01} }  // B - 5
        };

        #endregion

        public Cubie[] Corners { get; private set; }
        public Cubie[] Edges { get; private set; }
        
        public Cube(Cubie[] corners = null, Cubie[] edges = null)
        {
            Corners = corners ?? new Cubie[CORNERS_AMOUNT]
            {
                new Cubie(0, 0), new Cubie(1, 0), new Cubie(2, 0), new Cubie(3, 0),
                new Cubie(4, 0), new Cubie(5, 0), new Cubie(6, 0), new Cubie(7, 0)
            };

            Edges = edges ?? new Cubie[EDGES_AMOUNT]
            {
                new Cubie(0, 0), new Cubie(1, 0), new Cubie(02, 0), new Cubie(03, 0),
                new Cubie(4, 0), new Cubie(5, 0), new Cubie(06, 0), new Cubie(07, 0),
                new Cubie(8, 0), new Cubie(9, 0), new Cubie(10, 0), new Cubie(11, 0)
            };
        }

        /// <summary>
        /// Совершает операцию над кубом.
        /// Принимает комбинацию команд, разделенных пробелами.
        /// Доступные команды: U U2 U' R R2 R' F F2 F' D D2 D' L L2 L' B B2 B.
        /// </summary>
        public void Move(string combination)
        {
            byte[] moves = combination.Split(' ').Select(move => (byte)_validMoves.Index(move)).ToArray();

            foreach (var move in moves)
            {
                int sideToMove = move / 3; // 3 возможных движения для каждой из сторон
                int power = move % 3;

                for (int i = 0; i <= power; i++)
                {
                    Cubie[] corners = new Cubie[CORNERS_AMOUNT];
                    Cubie[] edges = new Cubie[EDGES_AMOUNT];

                    for (int cornId = 0; cornId < CORNERS_AMOUNT; cornId++)
                    {
                        Cubie c = Corners[_cornerMoveTable[sideToMove, cornId, 0]];
                        byte newOrient = (byte)((_cornerMoveTable[sideToMove, cornId, 1] + c.Orient) % 3);
                        corners[cornId] = new Cubie(c.Position, newOrient);
                    }

                    for (int edgeId = 0; edgeId < EDGES_AMOUNT; edgeId++)
                    {
                        Cubie c = Edges[_edgeMoveTable[sideToMove, edgeId, 0]];
                        byte newOrient = (byte)((_edgeMoveTable[sideToMove, edgeId, 1] + c.Orient) % 2);
                        edges[edgeId] = new Cubie(c.Position, newOrient);
                    }

                    Corners = corners;
                    Edges = edges;
                }
            }
        }

        /// <summary>
        /// Цвета наклеек в текущей конфигурации куба
        /// </summary>
        public byte[] GetFaceletColors()
        {
            byte[] facelets = new byte[FACELETS_AMOUNT];

            for (int c = 0; c < CORNERS_AMOUNT; c++)
            {
                Cubie corn = Corners[c];
                var curCornFacelets = _cornerFacelet[corn.Position].Rotate(corn.Orient);

                for (int f = 0; f < FACELETS_ON_CORNER_AMOUNT; f++)
                {
                    facelets[_cornerFacelet[c][f]] = curCornFacelets[f];
                }
            }

            for (int e = 0; e < EDGES_AMOUNT; e++)
            {
                Cubie edge = Edges[e];
                var curEdgeFacelets = EdgeFacelet[edge.Position].Rotate(edge.Orient);

                for (int f = 0; f < FACELETS_ON_EDGE_AMOUNT; f++)
                {
                    facelets[EdgeFacelet[e][f]] = curEdgeFacelets[f];
                }
            }

            return facelets.Select(f => (byte)(f / 8)).ToArray(); // где 8 - наклейки одной стороны;
        }
    }
}