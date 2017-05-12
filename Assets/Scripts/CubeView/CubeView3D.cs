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
        private float _debugRadius;
        [SerializeField]
        private float _rotateSpeed;
        [SerializeField]
        PaletteColors _paletteColors;
        [SerializeField]
        private Transform[] _centers;
        [SerializeField]
        private MeshRenderer[] _centerFacelets;
        [SerializeField]
        private GameObject[] Corners;
        [SerializeField]
        private GameObject[] Edges;
        [SerializeField]
        private Facelet3D[] _facelets;
        [SerializeField]
        private Handler3D[] _handlers;
        [SerializeField]
        private Transform _cornersParent;
        [SerializeField]
        private Transform _edgesParent;

        // Use this for initialization
        void Start()
        {
            for (byte cent = 0; cent < _centerFacelets.Length; cent++)
            {
                _centerFacelets[cent].material.color = _paletteColors.Colors[cent];
            }

            Cube tempCube = new CubeFactory().CreateCube(null);
            byte[] colorIds = tempCube.GetFaceletColors();
            for (byte i = 0; i < Cube.FACELETS_AMOUNT; i++)
            {
                _facelets[i].SetColor(colorIds[i], _paletteColors.Colors[colorIds[i]]);
                _handlers[i].Initialize(i);
                _handlers[i].OnSwipEnd.AddListener(Move);
            }
        }

        private void RestoreParents()
        {
            foreach (var corner in Corners)
            {
                corner.transform.SetParent(_cornersParent);
            }

            foreach (var edge in Edges)
            {
                edge.transform.SetParent(_edgesParent);
            }
        }

        private void Move(byte faceletId, Vector2 direction)
        {
            RestoreParents();

            string mv = SwipeConverter.GetMove(faceletId, direction);
            Debug.Log(mv);
            Transform rotator;
            Collider[] cubies;
            switch (mv)
            {
                case "U":
                    rotator = _centers[0];
                    cubies = Physics.OverlapSphere(rotator.position, _debugRadius, LayerMask.GetMask("Cubie"));
                    foreach (var cubie in cubies)
                    {
                        cubie.transform.SetParent(rotator);
                    }
                    StartCoroutine(Rotate(rotator, Vector3.up, 90f));
                    break;
                case "F":
                    rotator = _centers[2];
                    cubies = Physics.OverlapSphere(rotator.position, _debugRadius, LayerMask.GetMask("Cubie"));
                    foreach (var cubie in cubies)
                    {
                        cubie.transform.SetParent(rotator);
                    }
                    StartCoroutine(Rotate(rotator, Vector3.right, 90f));
                    break;
                default:
                    return;
            }
        }

        private IEnumerator Rotate(Transform transform, Vector3 axis, float angle)
        {
            float curAngle = 0;
            while (curAngle < angle)
            {
                transform.RotateAround(transform.position, axis, _rotateSpeed);
                curAngle += _rotateSpeed;
                yield return new WaitForEndOfFrame();                
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {                
                Debug.Log("Facelet26 rotation: " + _facelets[26].transform.rotation.eulerAngles);
            }
        }

        private void Click(byte faceletId)
        {
            Debug.Log("Click on facelet" + faceletId);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_centers[0].transform.position, _debugRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_centers[1].transform.position, _debugRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_centers[2].transform.position, _debugRadius);
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(_centers[3].transform.position, _debugRadius);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_centers[4].transform.position, _debugRadius);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(_centers[5].transform.position, _debugRadius);
        }
    }
}