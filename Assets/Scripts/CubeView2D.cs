using Assets.Scripts.Model;
using Assets.Scripts.Solvers;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CubeView2D : UIBehaviour
{
    [SerializeField]
    private Color[] _colors;

    [SerializeField]
    private Image[] _centers;

    [SerializeField]
    private Image[] _facelets;

    private Cube _cube;

    protected override void Awake()
    {
        for (int i = 0; i < _centers.Length; i++)
        {
            _centers[i].color = _colors[i];
        }

        _cube = new Cube();
        UpdateView(_cube);

        Shuffle();
    }

    public void UpdateView(Cube cube)
    {
        var faceletColors = cube.GetFaceletColors();

        for (int i = 0; i < Cube.FACELETS_AMOUNT; i++)
        {
            _facelets[i].color = _colors[faceletColors[i]];
        }
    }

    private void Shuffle()
    {
        _cube.Move("R F' L' D' U' L2 R2 B L' B2");//"R F' U' L B D");
        UpdateView(_cube);
    }

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
            new Solver().SolveCross(_cube);
        }

        else if (Input.GetKeyDown(KeyCode.Backspace)) 
        {
            new Solver().SolveCross(_cube);
            UpdateView(_cube);
        }

        else if (Input.GetKeyDown(KeyCode.T))
        {
            
        }
    }
}
