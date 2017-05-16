using Assets.Scripts.CubeModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.CubeView
{
    public class CubeView3D : MonoBehaviour
    {
        [SerializeField]
        private float _rotationSpeed;
        [SerializeField]
        CubeColorSheme _colorSheme;
        [SerializeField]
        private Transform _centersParent;
        [SerializeField]
        private Transform _cornersParent;
        [SerializeField]
        private Transform _edgesParent;

        private const float _overlapRadius = 0.8f;

        private Transform[] _centers;
        private Transform[] _corners;
        private Transform[] _edges;

        private MeshRenderer[] _facelets;

        private Queue<string> _cmdQ = new Queue<string>();

        private bool _rdyToNextTurn = true;

        private void Awake()
        {
            _centers = new Transform[6];
            for (int i = 0; i < _centers.Length; i++)
            {
                _centers[i] = _centersParent.GetChild(i).transform;
            }

            _corners = new Transform[8];
            for (int i = 0; i < _corners.Length; i++)
            {
                _corners[i] = _cornersParent.GetChild(i).transform;
            }

            _edges = new Transform[12];
            for (int i = 0; i < _edges.Length; i++)
            {
                _edges[i] = _edgesParent.GetChild(i).transform;
            }

            _facelets = new MeshRenderer[48];
            for (int i = 0; i < _corners.Length; i++)
            {
                MeshRenderer[] thisFacelets = _corners[i].GetComponentsInChildren<MeshRenderer>().
                    Where(x => x.gameObject.tag == "Facelet").ToArray();
                for (int j = 0; j < thisFacelets.Length; j++)
                {
                    _facelets[Cube.CornerFacelet[i][j]] = thisFacelets[j];
                }
            }
            for (int i = 0; i < _edges.Length; i++)
            {
                MeshRenderer[] thisFacelets = _edges[i].GetComponentsInChildren<MeshRenderer>().
                    Where(x => x.gameObject.tag == "Facelet").ToArray();
                for (int j = 0; j < thisFacelets.Length; j++)
                {
                    _facelets[Cube.EdgeFacelet[i][j]] = thisFacelets[j];
                }
            }
        }

        private void Start()
        {
            for (int i = 0; i < _centers.Length; i++)
            {
                _centers[i].GetComponentsInChildren<MeshRenderer>().
                    Where(x => x.gameObject.tag == "Facelet").First().material.color =
                    _colorSheme.GetColor(i);
            }

            for (int i = 0; i < 48; i++)
            {
                _facelets[i].material.color = _colorSheme.GetColor(i / 8);
            }

            FaceletHandler[] faceletHandlers = gameObject.GetComponentsInChildren<FaceletHandler>();
            foreach (var faceletHandler in faceletHandlers)
            {
                faceletHandler.OnFaceletDrag.AddListener(AddCommandInQueue);
            }
        }

        private void Update()
        {
            if (_rdyToNextTurn && _cmdQ.Count > 0)
            {
                StartCoroutine(Rotate(_cmdQ.Dequeue()));
                _rdyToNextTurn = false;
            }
        }

        public void AddCommandInQueue(string cmd)
        {
            _cmdQ.Enqueue(cmd);
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

        private IEnumerator Rotate(string cmd)
        {
            Transform rotator = _centers[_rotatorsMap[cmd]];
            Vector3 axis = _axisMap[cmd];

            Collider[] cubies = Physics.OverlapSphere(rotator.position, _overlapRadius, LayerMask.GetMask("Cubie"));
            foreach (var cubie in cubies)
            {
                cubie.transform.SetParent(rotator);                
            }

            float curAngle = 0;
            while (curAngle < 90f)
            {
                rotator.RotateAround(rotator.position, axis, _rotationSpeed);
                curAngle += _rotationSpeed;
                yield return new WaitForEndOfFrame();                
            }

            RestoreCubieParents();

            _rdyToNextTurn = true;
        }

        private void RestoreCubieParents()
        {
            foreach (var corner in _corners)
            {
                corner.SetParent(_cornersParent);
            }

            foreach (var edge in _edges)
            {
                edge.transform.SetParent(_edgesParent);
            }
        }
    }
}