using Assets.Scripts.CubeModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.CubeView
{
    public struct DefaultState
    {
        public Quaternion CubeRotation;

        public Quaternion[] CentersRotation;

        public Quaternion[] CornersRotation;
        public Quaternion[] EdgesRotation;

        public Vector3[] CornersPosition;
        public Vector3[] EdgesPosition;
    }

    public class CubeView3D : MonoBehaviour
    {
        [Range(1f, 5f)]
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
        public bool AnimNow { get { return !_rdyToNextTurn; } }

        private OnCommand _onFaceletDrag = new OnCommand();
        public OnCommand OnFaceletDrag { get { return _onFaceletDrag; } }

        private DefaultState _defaultState = new DefaultState();

        public void SetConfiguration(byte[] facelets)
        {
            StartCoroutine(SetConfigurationProcess(facelets));
        }

        private IEnumerator SetConfigurationProcess(byte[] facelets)
        {
            while (true)
            {
                if (_rdyToNextTurn)
                {
                    break;
                }

                yield return null;
            }

            RestoreDefaultState();

            for (int i = 0; i < 48; i++)
            {
                _facelets[i].material.color = _colorSheme.GetColor(facelets[i]);
            }
        }

        public void AddCommandInQueue(string cmd)
        {
            _cmdQ.Enqueue(cmd);
        }

        public void Awake()
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

            for (int i = 0; i < _centers.Length; i++)
            {
                _centers[i].GetComponentsInChildren<MeshRenderer>().
                    Where(x => x.gameObject.tag == "Facelet").First().material.color =
                    _colorSheme.GetColor(i);
            }

            FaceletHandler[] faceletHandlers = gameObject.GetComponentsInChildren<FaceletHandler>();
            foreach (var faceletHandler in faceletHandlers)
            {
                faceletHandler.OnFaceletDrag.AddListener(AddCommandInQueue);
            }

            SaveDefaultState();
        }

        private void SaveDefaultState()
        {
            _defaultState.CubeRotation = transform.rotation;

            _defaultState.CentersRotation = new Quaternion[_centers.Length];
            for (int i = 0; i < _centers.Length; i++)
            {
                _defaultState.CentersRotation[i] = _centers[i].rotation;
            }

            _defaultState.CornersRotation = new Quaternion[_corners.Length];
            _defaultState.CornersPosition = new Vector3[_corners.Length];
            for (int i = 0; i < _corners.Length; i++)
            {
                _defaultState.CornersRotation[i] = _corners[i].rotation;
                _defaultState.CornersPosition[i] = _corners[i].position;
            }

            _defaultState.EdgesRotation = new Quaternion[_edges.Length];
            _defaultState.EdgesPosition = new Vector3[_edges.Length];
            for (int i = 0; i < _edges.Length; i++)
            {
                _defaultState.EdgesRotation[i] = _edges[i].rotation;
                _defaultState.EdgesPosition[i] = _edges[i].position;
            }
        }

        private void RestoreDefaultState()
        {
            transform.rotation = _defaultState.CubeRotation;

            for (int i = 0; i < _centers.Length; i++)
            {
                _centers[i].rotation = _defaultState.CentersRotation[i];
            }

            for (int i = 0; i < _corners.Length; i++)
            {
                _corners[i].rotation = _defaultState.CornersRotation[i];
                _corners[i].position = _defaultState.CornersPosition[i];
            }

            for (int i = 0; i < _edges.Length; i++)
            {
                _edges[i].rotation = _defaultState.EdgesRotation[i];
                _edges[i].position = _defaultState.EdgesPosition[i];
            }
        }

        private void Update()
        {
            if (_rdyToNextTurn && _cmdQ.Count > 0)
            {
                string command = _cmdQ.Dequeue();
                StartCoroutine(Turn(command));
                _onFaceletDrag.Invoke(command);
                _rdyToNextTurn = false;
            }
        }

        private IEnumerator Turn(string combination)
        {
            string[] commands = combination.Split(' ');

            List<Transform> rotators = new List<Transform>();
            List<Vector3> axis = new List<Vector3>();

            for (int i = 0; i < commands.Length; i++)
            {
                Transform rotator = GetRotatorByCmd(commands[i]);
                rotators.Add(rotator);
                bool isPositivCommand = commands[i].Length == 1;
                axis.Add(isPositivCommand ? rotator.transform.forward : -rotator.transform.forward);

                Collider[] cubies = Physics.OverlapSphere(rotator.position, _overlapRadius, LayerMask.GetMask("Cubie"));
                foreach (var cubie in cubies)
                {
                    cubie.transform.SetParent(rotator);
                }
            }

            bool isMultipleCombination = commands.Length > 1;
            if (isMultipleCombination)
            {
                rotators.Add(this.transform);
                axis.Add(-axis.First());
            }

            float curAngle = 0;
            while (curAngle < 90f)
            {
                for (int i = 0; i < rotators.Count; i++)
                {
                    rotators[i].RotateAround(rotators[i].position, axis[i], _rotationSpeed);
                }

                curAngle += _rotationSpeed;
                yield return new WaitForEndOfFrame();

                for (int i = 0; i < rotators.Count; i++)
                {
                    Vector3 euler = rotators[i].rotation.eulerAngles;
                    euler.x = Mathf.Round(euler.x);
                    euler.y = Mathf.Round(euler.y);
                    euler.z = Mathf.Round(euler.z);
                    rotators[i].rotation = Quaternion.Euler(euler);
                }    
            }

            RestoreCubieParents();

            _rdyToNextTurn = true;
        }

        private Transform GetRotatorByCmd(string cmd)
        {
            switch (cmd)
            {
                case "U":
                case "U'":
                case "U2":
                    return _centers[0];
                case "R":
                case "R'":
                case "R2":
                    return _centers[1];
                case "F":
                case "F'":
                case "F2":
                    return _centers[2];
                case "L":
                case "L'":
                case "L2":
                    return _centers[3];
                case "B":
                case "B'":
                case "B2":
                    return _centers[4];
                case "D":
                case "D'":
                case "D2":
                    return _centers[5];
            }

            return null;
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