using Assets.Scripts.CubeModel;
using Assets.Scripts.Solver.Tables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Solver
{
    public static class Search
    {
        static byte[] ax = new byte[31];
        static byte[] po = new byte[31];

        static ushort[] flip = new ushort[31];
        static ushort[] twist = new ushort[31];
        static ushort[] udslice = new ushort[31];

        static ushort[] edgeperm = new ushort[31];
        static ushort[] cornperm = new ushort[31];

        static ushort[] udsliceS = new ushort[31];
        static ushort[] usliceS = new ushort[31];
        static ushort[] dsliceS = new ushort[31];

        static byte[] minDistPhase1 = new byte[31];
        static byte[] minDistPhase2 = new byte[31];

        static byte[] axPoToMove(int length)
        {
            byte[] vals = new byte[length];
            for (byte i = 0; i < length; i++)
            {
                vals[i] = (byte)(ax[i] * 3 + po[i] - 1);
            }

            return vals;
        }

        public static byte[] fullSolve(Cube cube, int maxDepth, int timeoutMS = 6000)
        {
            if (cube.IsSolved)
            {
                return null;
            }

            ax[0] = 0;
            po[0] = 0;

            flip[0] = cube.EdgeOrientCoord();
            twist[0] = cube.CornOrientCoord();
            udslice[0] = cube.UDSliceCoord();

            cornperm[0] = cube.CornPermCoord();

            udsliceS[0] = cube.UDSliceCoordS();
            usliceS[0] = cube.USliceCoordS();
            dsliceS[0] = cube.DSliceCoordS();

            minDistPhase1[1] = 1;
            int mv = 0;
            int n = 0;
            int depthPhase1 = 1;

            bool busy = false;

            Stopwatch watch = new Stopwatch();
            watch.Start();

            while (true)
            {
                do
                {
                    if (depthPhase1 - n > minDistPhase1[n + 1] && !busy)
                    {
                        if (ax[n] == 0 || ax[n] == 3)
                        {
                            ax[++n] = 1;
                        }
                        else
                        {
                            ax[++n] = 0;
                        }

                        po[n] = 1;
                    }
                    else if (++po[n] > 3)
                    {
                        do
                        {
                            if (++ax[n] > 5)
                            {
                                if (watch.ElapsedMilliseconds > timeoutMS)
                                {
                                    throw new TimeoutException("Not enough time");
                                }

                                if (n == 0)
                                {
                                    if (depthPhase1 > maxDepth)
                                    {
                                        throw new Exception("Max depth exceeded");
                                    }
                                    else
                                    {
                                        depthPhase1++;
                                        ax[n] = 0;
                                        po[n] = 1;
                                        busy = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    n--;
                                    busy = true;
                                    break;
                                }
                            }
                            else
                            {
                                po[n] = 1;
                                busy = false;
                            }
                        } while (n != 0 && (ax[n - 1] == ax[n] || ax[n - 1] == ax[n] + 3));
                    }
                    else
                    {
                        busy = false;
                    }
                } while (busy);

                mv = ax[n] * 3 + po[n] - 1;

                flip[n + 1] = MoveTables.moveEO[flip[n], mv];
                twist[n + 1] = MoveTables.moveCO[twist[n], mv];
                udslice[n + 1] = MoveTables.moveUD[udslice[n], mv];

                minDistPhase1[n + 1] = Math.Max(
                    PruneTable.pruneEO[Constants.N_UD * flip[n + 1] + udslice[n + 1]],
                    PruneTable.pruneCO[Constants.N_UD * twist[n + 1] + udslice[n + 1]]
                );

                if (minDistPhase1[n + 1] == 0 && n >= depthPhase1 - 1)
                {
                    minDistPhase1[n + 1] = 10;
                    int s = phaseTwo(depthPhase1, maxDepth);
                    if (n == depthPhase1 - 1 && s >= 0)
                    {
                        if (s == depthPhase1 || (ax[depthPhase1 - 1] != ax[depthPhase1] && ax[depthPhase1 - 1] != ax[depthPhase1] + 3))
                        {
                            return axPoToMove(s);
                        }
                    }
                }
            }
        }

        static int phaseTwo(int depthPhase1, int maxDepth)
        {
            int mv = 0;
            byte d1 = 0, d2 = 0;
            int maxDepthPhase2 = Math.Min(12, maxDepth - depthPhase1);

            for (int i = 0; i < depthPhase1; i++)
            {
                mv = 3 * ax[i] + po[i] - 1;
                udsliceS[i + 1] = MoveTables.moveUDS[udsliceS[i], mv];
                usliceS[i + 1] = MoveTables.moveUS[usliceS[i], mv];
                dsliceS[i + 1] = MoveTables.moveDS[dsliceS[i], mv];
                cornperm[i + 1] = MoveTables.moveCP[cornperm[i], mv];
            }

            edgeperm[depthPhase1] = MoveTables.mergeEP2[
                usliceS[depthPhase1],
                dsliceS[depthPhase1] % 24
            ];

            d1 = PruneTable.pruneCP[Constants.N_UD2 * cornperm[depthPhase1] + udsliceS[depthPhase1]];
            d2 = PruneTable.pruneEP2[Constants.N_UD2 * edgeperm[depthPhase1] + udsliceS[depthPhase1]];

            if (d1 > maxDepthPhase2 || d2 > maxDepthPhase2)
            {
                return -1;
            }

            minDistPhase2[depthPhase1] = Math.Max(d1, d2);
            if (minDistPhase2[depthPhase1] == 0)
            {
                return depthPhase1;
            }

            int depthPhase2 = 1, n = depthPhase1;
            bool busy = false;

            po[depthPhase1] = 0;
            ax[depthPhase1] = 0;
            minDistPhase2[n + 1] = 1;

            do
            {
                do
                {
                    if (depthPhase1 + depthPhase2 - n > minDistPhase2[n + 1] && !busy)
                    {
                        if (ax[n] == 0 || ax[n] == 3)
                        {
                            ax[++n] = 1;
                            po[n] = 2;
                        }
                        else
                        {
                            ax[++n] = 0;
                            po[n] = 1;
                        }

                        break;
                    }
                    po[n] += (ax[n] == 0 || ax[n] == 3) ? (byte)1 : (byte)2;
                    if (po[n] > 3)
                    {
                        do
                        {
                            if (++ax[n] > 5)
                            {
                                if (n == depthPhase1)
                                {
                                    if (depthPhase2 >= maxDepthPhase2)
                                    {
                                        return -1;
                                    }
                                    else
                                    {
                                        depthPhase2++;
                                        ax[n] = 0;
                                        po[n] = 1;
                                        busy = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    n--;
                                    busy = true;
                                    break;
                                }
                            }
                            else
                            {
                                po[n] = (ax[n] == 0 || ax[n] == 3) ? (byte)1 : (byte)2;
                                busy = false;
                            }
                        } while (n != depthPhase1 && (ax[n - 1] == ax[n] || ax[n - 1] == ax[n] + 3));
                    }
                    else
                    {
                        busy = false;
                    }
                } while (busy);

                mv = 3 * ax[n] + po[n] - 1;

                cornperm[n + 1] = MoveTables.moveCP[cornperm[n], mv];
                edgeperm[n + 1] = MoveTables.moveEP2[edgeperm[n], mv];
                udsliceS[n + 1] = MoveTables.moveUDS[udsliceS[n], mv];

                minDistPhase2[n + 1] = Math.Max(
                    PruneTable.pruneCP[Constants.N_UD2 * cornperm[n + 1] + udsliceS[n + 1]],
                    PruneTable.pruneEP2[Constants.N_UD2 * edgeperm[n + 1] + udsliceS[n + 1]]
                );
            } while (minDistPhase2[n + 1] != 0);

            return depthPhase1 + depthPhase2;
        }
    }
}

