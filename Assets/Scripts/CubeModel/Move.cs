using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Assets.Scripts.CubeModel
{
    public partial class Cube
    {
        #region consts

        private static byte[,,] _cornerMoveTable = new byte[6, CORNERS_AMOUNT, 2] 
        // где 6 - количество сторон куба, а 2 - размер массива для хранения позиции и ориентации
        {
            //  0       1       2       3       4       5       6       7       // corner id
            { {3, 0}, {0, 0}, {1, 0}, {2, 0}, {4, 0}, {5, 0}, {6, 0}, {7, 0} }, // U - 0
            { {4, 2}, {1, 0}, {2, 0}, {0, 1}, {7, 1}, {5, 0}, {6, 0}, {3, 2} }, // R - 1
            { {1, 1}, {5, 2}, {2, 0}, {3, 0}, {0, 2}, {4, 1}, {6, 0}, {7, 0} }, // F - 2
            { {0, 0}, {1, 0}, {2, 0}, {3, 0}, {5, 0}, {6, 0}, {7, 0}, {4, 0} }, // D - 3
            { {0, 0}, {2, 1}, {6, 2}, {3, 0}, {4, 0}, {1, 2}, {5, 1}, {7, 0} }, // L - 4
            { {0, 0}, {1, 0}, {3, 1}, {7, 2}, {4, 0}, {5, 0}, {2, 2}, {6, 1} }  // B - 5
        };

        private static byte[,,] _edgeMoveTable = new byte[6, EDGES_AMOUNT, 2]
        {
            //   0         1         2         3         4         5         6         7         8         9         10        11       // edge id
            { {03, 00}, {00, 00}, {01, 00}, {02, 00}, {04, 00}, {05, 00}, {06, 00}, {07, 00}, {08, 00}, {09, 00}, {10, 00}, {11, 00} }, // U - 0
            { {08, 00}, {01, 00}, {02, 00}, {03, 00}, {11, 00}, {05, 00}, {06, 00}, {07, 00}, {04, 00}, {09, 00}, {10, 00}, {00, 00} }, // R - 1
            { {00, 00}, {09, 01}, {02, 00}, {03, 00}, {04, 00}, {08, 01}, {06, 00}, {07, 00}, {01, 01}, {05, 01}, {10, 00}, {11, 00} }, // F - 2
            { {00, 00}, {01, 00}, {02, 00}, {03, 00}, {05, 00}, {06, 00}, {07, 00}, {04, 00}, {08, 00}, {09, 00}, {10, 00}, {11, 00} }, // D - 3
            { {00, 00}, {01, 00}, {10, 00}, {03, 00}, {04, 00}, {05, 00}, {09, 00}, {07, 00}, {08, 00}, {02, 00}, {06, 00}, {11, 00} }, // L - 4
            { {00, 00}, {01, 00}, {02, 00}, {11, 01}, {04, 00}, {05, 00}, {06, 00}, {10, 01}, {08, 00}, {09, 00}, {03, 01}, {07, 01} }  // B - 5
        };

        private static readonly string[] _validMoves = "U U2 U' R R2 R' F F2 F' D D2 D' L L2 L' B B2 B'".Split(' ');

        #endregion consts

        /// <summary>
        /// Совершает операцию над кубом.
        /// Принимает комбинацию команд, разделенных пробелами.
        /// Доступные команды: U U2 U' R R2 R' F F2 F' D D2 D' L L2 L' B B2 B.
        /// </summary>
        public void Move(string combination)
        {
            byte[] moves = combination.Split(' ').Select(move => (byte)_validMoves.Index(move)).ToArray();

            foreach (byte move in moves)
            {
                byte sideToMove = (byte)(move / 3); // 3 возможных движения для каждой из сторон
                byte power = (byte)(move % 3);

                for (byte i = 0; i <= power; i++)
                {
                    Cubie[] corners = new Cubie[CORNERS_AMOUNT];
                    Cubie[] edges = new Cubie[EDGES_AMOUNT];

                    for (byte cornId = 0; cornId < CORNERS_AMOUNT; cornId++)
                    {
                        Cubie c = Corners[_cornerMoveTable[sideToMove, cornId, 0]];
                        byte newOrient = (byte)((_cornerMoveTable[sideToMove, cornId, 1] + c.Orient) % 3);
                        corners[cornId] = new Cubie(c.Position, newOrient);
                    }

                    for (byte edgeId = 0; edgeId < EDGES_AMOUNT; edgeId++)
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
    }
}