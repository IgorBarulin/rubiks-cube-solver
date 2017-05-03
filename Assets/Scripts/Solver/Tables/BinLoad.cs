using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Assets.Scripts.Solver.Tables
{
    static class BinLoad
    {
        public static ushort[,] loadShortTable2D(string path, int chunksize = 18)
        {
            var bytes = File.ReadAllBytes(path);
            int len1d = bytes.Length / chunksize / 2;
            ushort[,] values = new ushort[len1d, chunksize];
            int i, j;

            for (i = 0; i < len1d; i++)
            {
                for (j = 0; j < chunksize; j++)
                {
                    values[i, j] = (ushort)(
                        (bytes[(chunksize * i + j) * 2] << 8) +
                        bytes[(chunksize * i + j) * 2 + 1]
                    );
                }
            }

            return values;
        }

        public static PruneTable loadPruneTable(string path)
        {
            return new PruneTable(File.ReadAllBytes(path));
        }
    }
}
