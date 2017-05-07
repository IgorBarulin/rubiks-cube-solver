using Assets.Scripts.CubeConstruct;
using Assets.Scripts.CubeModel;
using Assets.Scripts.Solver;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.CubeView
{
    public class CubeView2D : UIBehaviour
    {
        [SerializeField]
        private Image[] _centers;

        [SerializeField]
        private SwipeableFacelet2D[] _facelets;

        private Cube _cube;

        private Color[] _colors;

        public void Initialize(Color[] colors)
        {
            _colors = colors;

            for (byte cent = 0; cent < _centers.Length; cent++)
            {
                _centers[cent].color = _colors[cent];
            }

            for (byte i = 0; i < _facelets.Length; i++)
            {
                _facelets[i].Initialize(i);
                _facelets[i].OnSwipeEnd.AddListener(Move);
            }

            _cube = new CubeFactory().CreateCube(null);
            UpdateView(_cube);
        }

        private void Move(byte faceletId, Vector2 direction)
        {
            string mv = SwipeConverter.GetMove(faceletId, direction);
            if (mv == null) return;
            _cube.Move(mv);
            UpdateView(_cube);
        }

        public void UpdateView(Cube cube)
        {
            byte[] faceletColors = cube.GetFaceletColors();

            for (int i = 0; i < Cube.FACELETS_AMOUNT; i++)
            {
                _facelets[i].SetColor(faceletColors[i], _colors[faceletColors[i]]);
            }
        }

        #region trash

        private void Update()
        {
            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.U))
            {
                _cube.Move("U2");
                UpdateView(_cube);
            }
            else if (Input.GetKey(KeyCode.Space) && Input.GetKeyDown(KeyCode.U))
            {
                _cube.Move("U'");
                UpdateView(_cube);
            }
            else if (Input.GetKeyDown(KeyCode.U))
            {
                _cube.Move("U");
                UpdateView(_cube);
            }
            else if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.R))
            {
                _cube.Move("R2");
                UpdateView(_cube);
            }
            else if (Input.GetKey(KeyCode.Space) && Input.GetKeyDown(KeyCode.R))
            {
                _cube.Move("R'");
                UpdateView(_cube);
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                _cube.Move("R");
                UpdateView(_cube);
            }
            else if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.F))
            {
                _cube.Move("F2");
                UpdateView(_cube);
            }
            else if (Input.GetKey(KeyCode.Space) && Input.GetKeyDown(KeyCode.F))
            {
                _cube.Move("F'");
                UpdateView(_cube);
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                _cube.Move("F");
                UpdateView(_cube);
            }
            else if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.L))
            {
                _cube.Move("L2");
                UpdateView(_cube);
            }
            else if (Input.GetKey(KeyCode.Space) && Input.GetKeyDown(KeyCode.L))
            {
                _cube.Move("L'");
                UpdateView(_cube);
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                _cube.Move("L");
                UpdateView(_cube);
            }
            else if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.B))
            {
                _cube.Move("B2");
                UpdateView(_cube);
            }
            else if (Input.GetKey(KeyCode.Space) && Input.GetKeyDown(KeyCode.B))
            {
                _cube.Move("B'");
                UpdateView(_cube);
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                _cube.Move("B");
                UpdateView(_cube);
            }
            else if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.D))
            {
                _cube.Move("D2");
                UpdateView(_cube);
            }
            else if (Input.GetKey(KeyCode.Space) && Input.GetKeyDown(KeyCode.D))
            {
                _cube.Move("D'");
                UpdateView(_cube);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                _cube.Move("D");
                UpdateView(_cube);
            }

            else if (Input.GetKeyDown(KeyCode.S))
            {
                string[] moves = "U U2 U' R R2 R' F F2 F' D D2 D' L L2 L' B B2 B'".Split(' ');
                StringBuilder comb = new StringBuilder();
                int len = Random.Range(10, 100);
                for (int i = 0; i < len; i++)
                {
                    for (int j = 0; j < moves.Length; j++)
                    {
                        comb.Append(moves[Random.Range(0, moves.Length - 1)] + " ");
                    }
                }
                _cube.Move(comb.ToString().Trim());                
                UpdateView(_cube);
            }

            else if (Input.GetKeyDown(KeyCode.T))
            {
                string[] moves = "U U2 U' R R2 R' F F2 F' D D2 D' L L2 L' B B2 B'".Split(' ');
                var mvs = Search.fullSolve(_cube, 21);
                StringBuilder result = new StringBuilder();
                foreach (var m in mvs)
                {
                    result.Append(moves[m] + " ");
                }
                Debug.Log("KOCIEMBA: " + result);
                StartCoroutine(Solve(_cube, result.ToString().Trim()));
            }
        }

        private IEnumerator Solve(Cube cube, string comb)
        {
            var cmb = comb.Split(' ');
            foreach (var c in cmb)
            {
                cube.Move(c);
                UpdateView(cube);
                yield return new WaitForSeconds(1f);
            }
        }

        #endregion
    }
}

