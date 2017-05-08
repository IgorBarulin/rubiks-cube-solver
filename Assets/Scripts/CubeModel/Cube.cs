using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.CubeModel
{
    public partial class Cube
    {
        #region consts

        /* Диаграмма, представляющая развертку куба 3х3х3:
         
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

        public const byte CORNERS_AMOUNT = 8;
        public const byte EDGES_AMOUNT = 12;

        public const byte FACELETS_AMOUNT = 48;

        public const byte FACELETS_ON_CORNER_AMOUNT = 3;
        public const byte FACELETS_ON_EDGE_AMOUNT = 2;

        private static readonly byte[][] _cornerFacelet = new byte[CORNERS_AMOUNT][]
        {
            new byte[FACELETS_ON_CORNER_AMOUNT] {04, 08, 18}, // URF - 0
            new byte[FACELETS_ON_CORNER_AMOUNT] {06, 16, 26}, // UFL - 1
            new byte[FACELETS_ON_CORNER_AMOUNT] {00, 24, 34}, // ULB - 2
            new byte[FACELETS_ON_CORNER_AMOUNT] {02, 32, 10}, // UBR - 3
            new byte[FACELETS_ON_CORNER_AMOUNT] {42, 20, 14}, // DFR - 4
            new byte[FACELETS_ON_CORNER_AMOUNT] {40, 28, 22}, // DLF - 5
            new byte[FACELETS_ON_CORNER_AMOUNT] {46, 36, 30}, // DBL - 6
            new byte[FACELETS_ON_CORNER_AMOUNT] {44, 12, 38}  // DRB - 7
        };

        private static readonly byte[][] _edgeFacelet = new byte[EDGES_AMOUNT][]
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

        #endregion

        public Cubie[] Corners { get; private set; }
        public Cubie[] Edges { get; private set; }
        
        public Cube(Cubie[] corners, Cubie[] edges)
        {
            Corners = corners;
            Edges = edges;
        }

        /// <summary>
        /// Цвета наклеек текущей конфигурации куба
        /// </summary>
        public byte[] GetFaceletColors() 
        {
            byte[] facelets = new byte[FACELETS_AMOUNT];

            for (byte c = 0; c < CORNERS_AMOUNT; c++)
            {
                Cubie corn = Corners[c];
                var curCornFacelets = _cornerFacelet[corn.Position].Rotate(corn.Orient);

                for (byte f = 0; f < FACELETS_ON_CORNER_AMOUNT; f++)
                {
                    facelets[_cornerFacelet[c][f]] = curCornFacelets[f];
                }
            }

            for (byte e = 0; e < EDGES_AMOUNT; e++)
            {
                Cubie edge = Edges[e];
                byte[] curEdgeFacelets = _edgeFacelet[edge.Position].Rotate(edge.Orient);

                for (byte f = 0; f < FACELETS_ON_EDGE_AMOUNT; f++)
                {
                    facelets[_edgeFacelet[e][f]] = curEdgeFacelets[f];
                }
            }

            return facelets.Select(f => (byte)(f / 8)).ToArray(); // где 8 - наклейки одной стороны без центра;
        }
    }
}