using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Solver.Tables
{
    public class PruneTable
    {
        byte[] bytes;

        public PruneTable(byte[] bytes)
        {
            this.bytes = bytes;
        }

        public byte this[int index]
        {
            get
            {
                if ((index & 1) == 1)
                {
                    return (byte)((bytes[index / 2] & 0xf0) >> 4);
                }
                else
                {
                    return (byte)(bytes[index / 2] & 0x0f);
                }
            }
            set
            {
                if ((index & 1) == 1)
                {
                    bytes[index / 2] &= (byte)(0x0f | (value << 4));
                }
                else
                {
                    bytes[index / 2] &= (byte)(value | 0xf0);
                }
            }
        }

        // Prune tables

        public static readonly PruneTable pruneCO = BinLoad.loadPruneTable("Assets\\Tables\\prune\\co_ud_prun");
        public static readonly PruneTable pruneEO = BinLoad.loadPruneTable("Assets\\Tables\\prune\\eo_ud_prun");

        public static readonly PruneTable pruneCP = BinLoad.loadPruneTable("Assets\\Tables\\prune\\cp_ud2_prun");
        public static readonly PruneTable pruneEP2 = BinLoad.loadPruneTable("Assets\\Tables\\prune\\ep2_ud2_prun");
    }
}