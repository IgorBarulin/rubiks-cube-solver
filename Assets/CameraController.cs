using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private Vector3 _bounds;

    private GameObject _testObj;

    private Vector3[] _top = new Vector3[]
    {
        new Vector3(1, 1, 1),
        new Vector3(1, 1, -1),
        new Vector3(-1, 1, -1),
        new Vector3(-1, 1, 1)
    };

    private Vector3[] _bottom = new Vector3[]
    {
        new Vector3(1, -1, 1),
        new Vector3(1, -1, -1),
        new Vector3(-1, -1, -1),
        new Vector3(-1, -1, 1)
    };

    private bool _onBottomNow;

    private int _current;

    private bool _nowMoving;

    private void Start()
    {
        Move();
    }

    private void Update()
    {
        if (!_nowMoving)
        {
            if (Input.GetMouseButton(1))
            {
                float horizontal = Input.GetAxis("Mouse X");
                float vertical = Input.GetAxis("Mouse Y");

                float absHorizontal = Mathf.Abs(horizontal);
                float absVertical = Mathf.Abs(vertical);

                if (absHorizontal > absVertical)
                {
                    if (horizontal > 0)
                    {
                        _current--;
                        if (_current < 0)
                        {
                            _current = 3;
                        }
                        Move();
                    }
                    else if (horizontal < 0)
                    {
                        _current++;
                        if (_current > 3)
                        {
                            _current = 0;
                        }
                        Move();
                    }
                }
                else if (absVertical > absHorizontal)
                {
                    if (vertical > 0)
                    {
                        if (_onBottomNow)
                        {
                            _onBottomNow = false;
                            Move();
                        }
                    }
                    else if (vertical < 0)
                    {
                        if (!_onBottomNow)
                        {
                            _onBottomNow = true;
                            Move();
                        }
                    }
                }
            }
        }
    }

    private void Move()
    {
        Vector3 pos = _onBottomNow ? _bottom[_current] : _top[_current];
        pos.x *= _bounds.x / 2;
        pos.y *= _bounds.y / 2;
        pos.z *= _bounds.z / 2;

        StartCoroutine(MoveProcess(pos));
        _nowMoving = true;
    }

    private IEnumerator MoveProcess(Vector3 to)
    {
        while (transform.position != to)
        {
            transform.position = Vector3.MoveTowards(transform.position, to, _moveSpeed);
            transform.LookAt(_target);
            yield return new WaitForEndOfFrame();
        }
        _nowMoving = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_target.position, _bounds);
    }
}
