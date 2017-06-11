using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Solver.Tables
{
    public static class MoveTables
    {
        // Move tables, max size 8! * 18 = 725760

        public static readonly ushort[,] moveCO = BinLoad.loadShortTable2D("Assets/Tables/move/co_move");
        public static readonly ushort[,] moveEO = BinLoad.loadShortTable2D("Assets/Tables/move/eo_move");
        public static readonly ushort[,] moveUD = BinLoad.loadShortTable2D("Assets/Tables/move/ud_move");

        public static readonly ushort[,] moveCP = BinLoad.loadShortTable2D("Assets/Tables/move/cp_move");
        public static readonly ushort[,] moveEP2 = BinLoad.loadShortTable2D("Assets/Tables/move/ep2_move");
        public static readonly ushort[,] moveUDS = BinLoad.loadShortTable2D("Assets/Tables/move/uds_move");

        public static readonly ushort[,] moveUS = BinLoad.loadShortTable2D("Assets/Tables/move/us_move");
        public static readonly ushort[,] moveDS = BinLoad.loadShortTable2D("Assets/Tables/move/ds_move");

        // Merge table (US-DS to EP2), size 11880 * 24 = 285120

        public static readonly ushort[,] mergeEP2 = BinLoad.loadShortTable2D("Assets/Tables/other/ep2_merge", 24);
    }
}