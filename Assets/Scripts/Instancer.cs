using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instancer : Singleton<Instancer>
{
    [SerializeField]
    private Canvas _canvas;

    [SerializeField]
    private GameObject _cubeView2D;
    [SerializeField]
    private GameObject _palette;
    [SerializeField]
    private GameObject _constructor2D;

    public CubeView2D CreateCubeView2D()
    {
        CubeView2D cubeView2D = Instantiate(_cubeView2D).GetComponent<CubeView2D>();
        cubeView2D.transform.SetParent(_canvas.transform, false);
        return cubeView2D;
    }

    public Palette CreatePalette()
    {
        Palette palette = Instantiate(_palette).GetComponent<Palette>();
        palette.transform.SetParent(_canvas.transform, false);
        return palette;
    }

    public Constructor2D CreateConstructor2D()
    {
        Constructor2D constructor2D = Instantiate(_constructor2D).GetComponent<Constructor2D>();
        constructor2D.transform.SetParent(_canvas.transform, false);
        return constructor2D;
    }
}
