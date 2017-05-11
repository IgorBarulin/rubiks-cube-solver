using Assets.Scripts.CubeConstruct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.CubeView
{
    public class Facelet3D : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer _view;

        public byte ColorId { get; private set; }

        public void SetColor(byte colorId, Color color)
        {
            ColorId = colorId;
            _view.material.color = color;
        }
    }
}