using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Solvers
{
    public class Solver
    {
        private static readonly int[,] _adjacencyMtx = new int[Cube.EDGES_AMOUNT, Cube.EDGES_AMOUNT]
        {
            { 0, 1, 1, 1, 1, 0, 0, 0, 1, 0, 0, 1 },
            { 1, 0, 1, 1, 0, 1, 0, 0, 1, 1, 0, 0 },
            { 1, 1, 0, 1, 0, 0, 1, 0, 0, 1, 1, 0 },
            { 1, 1, 1, 0, 0, 0, 0, 1, 0, 0, 1, 1 },
            { 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 1 },
            { 0, 1, 0, 0, 1, 0, 1, 1, 1, 1, 0, 0 },
            { 0, 0, 1, 0, 1, 1, 0, 1, 0, 1, 1, 0 },
            { 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 1, 1 },
            { 1, 1, 0, 0, 1, 1, 0, 0, 0, 1, 0, 1 },
            { 0, 1, 1, 0, 0, 1, 1, 0, 1, 0, 1, 0 },
            { 0, 0, 1, 1, 0, 0, 1, 1, 0, 1, 0, 1 },
            { 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 1, 0 },
        };

        private static readonly int[,] _orientMtx = new int[Cube.EDGES_AMOUNT, Cube.EDGES_AMOUNT]
        {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1 },
            { 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
            { 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0 },
            { 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0 },
        };

        private static readonly string[,] _moveMtx = new string[Cube.EDGES_AMOUNT, Cube.EDGES_AMOUNT]
        {
            { "", "U", "U2", "U'", "R2", "", "", "", "R'", "", "", "R" },
            { "U'", "", "U", "U2", "", "F2", "", "", "F", "F'", "", "" },
            { "U2", "U'", "", "U", "", "", "L2", "", "", "L", "L'", "" },
            { "U", "U2", "U'", "", "", "", "", "B2", "", "", "B", "B'" },
            { "R2", "", "", "", "", "D'", "D2", "D", "R", "", "", "R'" },
            { "", "F2", "", "", "D", "", "D'", "D2", "F'", "F", "", "" },
            { "", "", "L2", "", "D2", "D", "", "D'", "", "L'", "L", "" },
            { "", "", "", "B2", "D'", "D2", "D", "", "", "", "B'", "B" },
            { "R", "F'", "", "", "R'", "F", "", "", "", "F2", "", "R2" },
            { "", "F", "L'", "", "", "F'", "L", "", "F2", "", "L2", "" },
            { "", "", "L", "B'", "", "", "L'", "B", "", "L2", "", "B2" },
            { "R'", "", "", "B", "R", "", "", "B'", "R2", "", "B2", "" },
        };

        public void SolveCross(Cube cube)
        {
            int e4 = -1;
            int e4o = 0;
            foreach (var cubie in cube.Edges)
            {
                e4++;
                if (cubie.Position == 4)
                {
                    e4o = cubie.Orient;
                    break;
                }
            }
            string move = FindMove(e4, 4, e4o, 0);
            Debug.Log(move);
            cube.Move(move);
        }

        public void Test(int from, int to, int currentOrient, int targetOrient)
        {
            int[] path = FindPath(from, to, currentOrient, targetOrient);
            foreach (var item in path)
            {
                Debug.Log(item);
            }
        }

        private string ReverseCommand(string cmd)
        {
            switch (cmd)
            {
                case "U":
                    return "U'";
                case "U'":
                    return "U";
                case "R":
                    return "R'";
                case "R'":
                    return "R";
                case "F":
                    return "F'";
                case "F'":
                    return "F";
                case "D":
                    return "D'";
                case "D'":
                    return "D";
                case "L":
                    return "L'";
                case "L'":
                    return "L";
                case "B":
                    return "B'";
                case "B'":
                    return "B";
                default:
                    return cmd;
            }
        }

        public string FindMove(int startPoint, int goalPoint, int startOrient, int goalOrient)
        {
            int[] path = FindPath(startPoint, goalPoint, startOrient, goalOrient);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < path.Length - 1; i++)
            {
                sb.Append(_moveMtx[path[i], path[i + 1]]);
                sb.Append(" ");
            }

            return sb.ToString().Trim();
        }

        private int[] FindPath(int startPoint, int goalPoint, int startOrient, int goalOrient, int maxDepth = 5)
        {
            List<int> path = new List<int>(maxDepth);

            NextNode(startPoint, goalPoint, startOrient, goalOrient, maxDepth, new List<int>(maxDepth), ref path);

            return path.ToArray();
        }
        
        private void NextNode(int curPoint, int goalPoint, int curOrient, int goalOrient, int depthLimit, List<int> prevPath, ref List<int> resPath)
        {
            if (depthLimit == 0) return;

            List<int> curPath = new List<int>(prevPath);
            curPath.Add(curPoint);

            if (curPoint == goalPoint && curOrient == goalOrient)
            {
                if (curPath.Count < resPath.Count || resPath.Count == 0)
                {
                    resPath = curPath;
                }
            }

            foreach (var adj in GetAdjacentNodes(curPoint))
            {
                NextNode(adj, goalPoint, (_orientMtx[adj, curPoint] + curOrient) % 2, goalOrient, depthLimit - 1, curPath, ref resPath);
            }
        }

        private int[] GetAdjacentNodes(int point)
        {
            LinkedList<int> adjacentNodes = new LinkedList<int>();

            for (int i = 0; i < Cube.EDGES_AMOUNT; i++)
                if (_adjacencyMtx[point, i] == 1)
                    adjacentNodes.AddLast(i);

            return adjacentNodes.ToArray();
        }
    }
}

