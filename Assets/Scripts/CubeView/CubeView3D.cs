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
                //_handlers[i].Initialize(i);
                //_handlers[i].OnSwipEnd.AddListener(Move);
            }

            FaceletHandler[] cubieHandlers = gameObject.GetComponentsInChildren<FaceletHandler>();
            foreach (var cubieHandler in cubieHandlers)
            {
                cubieHandler.OnDragCommand.AddListener(Move);
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

        private void Move(string move)
        {
            RestoreParents();

            Transform rotator;
            Vector3 axis;
            switch (move)
            {
                case "U":
                    rotator = _centers[0];
                    axis = Vector3.up;
                    break;
                case "U'":
                    rotator = _centers[0];
                    axis = Vector3.down;
                    break;
                case "R":
                    rotator = _centers[1];
                    axis = Vector3.forward;
                    break;
                case "R'":
                    rotator = _centers[1];
                    axis = Vector3.back;
                    break;
                case "F":
                    rotator = _centers[2];
                    axis = Vector3.right;
                    break;
                case "F'":
                    rotator = _centers[2];
                    axis = Vector3.left;
                    break;
                case "L":
                    rotator = _centers[3];
                    axis = Vector3.back;
                    break;
                case "L'":
                    rotator = _centers[3];
                    axis = Vector3.forward;
                    break;
                case "B":
                    rotator = _centers[4];
                    axis = Vector3.left;
                    break;
                case "B'":
                    rotator = _centers[4];
                    axis = Vector3.right;
                    break;
                case "D":
                    rotator = _centers[5];
                    axis = Vector3.down;
                    break;
                case "D'":
                    rotator = _centers[5];
                    axis = Vector3.up;
                    break;
                default:
                    return;
            }

            Collider[] cubies = Physics.OverlapSphere(rotator.position, _debugRadius, LayerMask.GetMask("Cubie"));
            foreach (var cubie in cubies)
            {
                cubie.transform.SetParent(rotator);
            }
            StartCoroutine(Rotate(rotator, axis));
        }

        private void Move(byte faceletId, Vector2 direction)
        {
            RestoreParents();

            string mv = SwipeConverter.GetMove(faceletId, direction);
            Debug.Log(mv);
            Transform rotator;
            Collider[] cubies;
            Vector3 axis;
            switch (mv)
            {
                case "U":
                    rotator = _centers[0];
                    axis = Vector3.up;
                    break;
                case "U'":
                    rotator = _centers[0];
                    axis = Vector3.down;
                    break;
                case "R":
                    rotator = _centers[1];
                    axis = Vector3.forward;
                    break;
                case "R'":
                    rotator = _centers[1];
                    axis = Vector3.back;
                    break;
                case "F":
                    rotator = _centers[2];
                    axis = Vector3.right;
                    break;
                case "F'":
                    rotator = _centers[2];
                    axis = Vector3.left;
                    break;
                case "L":
                    rotator = _centers[3];
                    axis = Vector3.back;
                    break;
                case "L'":
                    rotator = _centers[3];
                    axis = Vector3.forward;
                    break;
                case "B":
                    rotator = _centers[4];
                    axis = Vector3.left;
                    break;
                case "B'":
                    rotator = _centers[4];
                    axis = Vector3.right;
                    break;
                case "D":
                    rotator = _centers[5];
                    axis = Vector3.down;
                    break;
                case "D'":
                    rotator = _centers[5];
                    axis = Vector3.up;
                    break;
                default:
                    return;
            }

            cubies = Physics.OverlapSphere(rotator.position, _debugRadius, LayerMask.GetMask("Cubie"));
            foreach (var cubie in cubies)
            {
                cubie.transform.SetParent(rotator);
            }
            StartCoroutine(Rotate(rotator, axis));
        }

        private IEnumerator Rotate(Transform transform, Vector3 axis)
        {
            float curAngle = 0;
            while (curAngle < 90f)
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

        private int _sideLabelFontSize = 40;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_centers[0].transform.position, _debugRadius);
            GizmosUtils.DrawText(GUI.skin, "U", _centers[0].transform.position + Vector3.up * _debugRadius, Color.white, _sideLabelFontSize);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_centers[1].transform.position, _debugRadius);
            GizmosUtils.DrawText(GUI.skin, "R", _centers[1].transform.position + Vector3.forward * _debugRadius, Color.white, _sideLabelFontSize);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_centers[2].transform.position, _debugRadius);
            GizmosUtils.DrawText(GUI.skin, "F", _centers[2].transform.position + Vector3.right * _debugRadius, Color.white, _sideLabelFontSize);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_centers[3].transform.position, _debugRadius);
            GizmosUtils.DrawText(GUI.skin, "L", _centers[3].transform.position + Vector3.back * _debugRadius, Color.white, _sideLabelFontSize);

            Gizmos.color = new Color(255f, 56f, 0f); //ff8000
            Gizmos.DrawWireSphere(_centers[4].transform.position, _debugRadius);
            GizmosUtils.DrawText(GUI.skin, "B", _centers[4].transform.position + Vector3.left * _debugRadius, Color.white, _sideLabelFontSize);

            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(_centers[5].transform.position, _debugRadius);
            GizmosUtils.DrawText(GUI.skin, "D", _centers[5].transform.position + Vector3.down * _debugRadius, Color.white, _sideLabelFontSize);
        }
    }
}