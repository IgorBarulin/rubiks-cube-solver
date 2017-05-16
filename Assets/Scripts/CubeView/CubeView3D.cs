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

        private Queue<string> _movesQ = new Queue<string>();

        private bool _rdyToNextRotate = true;

        private void Start()
        {
            for (byte i = 0; i < _centerFacelets.Length; i++)
            {
                _centerFacelets[i].material.color = _paletteColors.Colors[i];
            }

            Cube tempCube = new CubeFactory().CreateCube(null);
            byte[] colorIds = tempCube.GetFaceletColors();
            for (byte i = 0; i < Cube.FACELETS_AMOUNT; i++)
            {
                _facelets[i].SetColor(colorIds[i], _paletteColors.Colors[colorIds[i]]);
            }

            FaceletHandler[] cubieHandlers = gameObject.GetComponentsInChildren<FaceletHandler>();
            foreach (var cubieHandler in cubieHandlers)
            {
                cubieHandler.OnDragCommand.AddListener(AddMoveInQueue);
            }
        }

        private Dictionary<string, int> _rotatorsMap = new Dictionary<string, int>
        {
            { "U", 0 }, { "U'", 0 },
            { "R", 1 }, { "R'", 1 },
            { "F", 2 }, { "F'", 2 },
            { "L", 3 }, { "L'", 3 },
            { "B", 4 }, { "B'", 4 },
            { "D", 5 }, { "D'", 5 },
        };

        private Dictionary<string, Vector3> _axisMap = new Dictionary<string, Vector3>
        {
            { "U", Vector3.up      }, { "U'", Vector3.down },
            { "R", Vector3.forward }, { "R'", Vector3.back },
            { "F", Vector3.right   }, { "F'", Vector3.left },
            { "L", Vector3.back    }, { "L'", Vector3.forward },
            { "B", Vector3.left    }, { "B'", Vector3.right },
            { "D", Vector3.down    }, { "D'", Vector3.up },
        };

        private void AddMoveInQueue(string move)
        {
            _movesQ.Enqueue(move);
        }

        private IEnumerator Rotate(string move)
        {
            foreach (var corner in Corners)
            {
                corner.transform.SetParent(_cornersParent);
            }

            foreach (var edge in Edges)
            {
                edge.transform.SetParent(_edgesParent);
            }

            Transform rotator = _centers[_rotatorsMap[move]];

            Collider[] cubies = Physics.OverlapSphere(rotator.position, _debugRadius, LayerMask.GetMask("Cubie"));
            foreach (var cubie in cubies)
            {
                cubie.transform.SetParent(rotator);                
            }

            Vector3 axis = _axisMap[move];

            float curAngle = 0;
            while (curAngle < 90f)
            {
                rotator.RotateAround(rotator.position, axis, _rotateSpeed);
                curAngle += _rotateSpeed;
                yield return new WaitForEndOfFrame();                
            }

            _rdyToNextRotate = true;
        }

        private void Update()
        {
            if (_rdyToNextRotate && _movesQ.Count > 0)
            {
                StartCoroutine(Rotate(_movesQ.Dequeue()));
                _rdyToNextRotate = false;
            }
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