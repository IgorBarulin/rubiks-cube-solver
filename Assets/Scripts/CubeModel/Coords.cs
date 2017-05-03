using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.CubeModel
{
    public partial class Cube
    {
        public bool IsSolved
        {
            get
            {
                return (
                    CornOrientCoord() == 0 &&
                    EdgeOrientCoord() == 0 &&
                    CornPermCoord() == 0 &&
                    EdgePermCoord() == 0
                );
            }
        }

        #region consts

        private static readonly byte[] USlice = new byte[4] { 0, 1, 2, 3 };
        private static readonly byte[] DSlice = new byte[4] { 4, 5, 6, 7 };
        private static readonly byte[] UDSlice = new byte[4] { 8, 9, 10, 11 };

        #endregion

        #region orients

        public ushort CornOrientCoord()
        {
            ushort s = 0;
            for (byte i = 0; i < 7; i++)
            {
                s = (ushort)(3 * s + Corners[i].Orient);
            }

            return s;
        }

        public ushort EdgeOrientCoord()
        {
            ushort s = 0;
            for (byte i = 0; i < 11; i++)
            {
                s = (ushort)(2 * s + Edges[i].Orient);
            }

            return s;
        }

        #endregion

        #region permutations

        public ushort CornPermCoord()
        {
            return PermCoord(Corners);
        }

        public ushort EdgePermCoord()
        {
            return PermCoord(Edges);
        }

        private ushort PermCoord(Cubie[] cubies)
        {
            ushort x = 0;

            for (sbyte i = (sbyte)(cubies.Length - 1); i > 0; i--)
            {
                byte s = 0;
                for (sbyte j = i; j >= 0; j--)
                {
                    if (cubies[j].Position > cubies[i].Position)
                    {
                        s++;
                    }
                }

                x = (ushort)((x + s) * i);
            }

            return x;
        }

        #endregion

        #region combinations

        public ushort UDSliceCoord()
        {
            return SliceCoord(Edges, UDSlice);
        }

        public ushort USliceCoord()
        {
            return SliceCoord(Edges.Reverse().ToArray(), USlice);
        }

        public ushort DSliceCoord()
        {
            return SliceCoord(Edges.Rotate(4).ToArray(), DSlice);
        }

        private ushort SliceCoord(Cubie[] edges, byte[] slice)
        {
            sbyte k = -1;
            ushort s = 0;

            for (byte i = 0; i < Edges.Length; i++)
            {
                if (slice.Contains(edges[i].Position))
                {
                    k++;
                }
                else if (k != -1)
                {
                    s += (ushort)Tools.nChooseK(i, k);
                }
            }

            return s;
        }

        #endregion

        #region sorted combinations

        public ushort UDSliceCoordS()
        {
            return SliceCoordS(UDSlice, UDSliceCoord());
        }

        public ushort USliceCoordS()
        {
            return SliceCoordS(USlice, USliceCoord());
        }

        public ushort DSliceCoordS()
        {
            return SliceCoordS(DSlice, DSliceCoord());
        }

        private ushort SliceCoordS(byte[] slice, ushort sliceCoord)
        {
            List<byte> arr = new List<byte>(4);
            ushort x = 0;

            foreach (Cubie e in Edges)
            {
                if (slice.Contains(e.Position))
                {
                    arr.Add(e.Position);
                }
            }

            for (sbyte i = 3; i > 0; i--)
            {
                byte s = 0;
                for (sbyte j = i; j >= 0; j--)
                {
                    if (arr[j] > arr[i])
                    {
                        s++;
                    }
                }

                x = (ushort)((x + s) * i);
            }

            return (ushort)(sliceCoord * 24 + x);
        }

        #endregion
    }
}
