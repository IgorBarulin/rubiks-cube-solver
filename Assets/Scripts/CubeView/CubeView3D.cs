using Assets.Scripts.CubeConstruct;
using Assets.Scripts.CubeModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.CubeView
{
    public class CubeView3D : MonoBehaviour
    {
        [SerializeField]
        PaletteColors _paletteColors;
        [SerializeField]
        private MeshRenderer[] _centers;
        [SerializeField]
        private Facelet3D[] _facelets;

        // Use this for initialization
        void Start()
        {
            for (byte cent = 0; cent < _centers.Length; cent++)
            {
                _centers[cent].material.color = _paletteColors.Colors[cent];
            }

            Cube tempCube = new CubeFactory().CreateCube(null);
            byte[] colorIds = tempCube.GetFaceletColors();
            for (int i = 0; i < _facelets.Length; i++)
            {
                _facelets[i].SetColor(colorIds[i], _paletteColors.Colors[colorIds[i]]);
                _facelets[i].OnClick.AddListener(Click);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void Click(byte faceletId)
        {
            Debug.Log("Click on facelet" + faceletId);
        }
    }
}

