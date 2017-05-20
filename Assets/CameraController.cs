using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraCommand
{
    LEFT,
    RIGHT,
    UP,
    DOWN
}

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform _cube3d;
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private Vector3 _bounds;

    private Vector3[] _top = new Vector3[]
    {
        new Vector3( 1,  1,  1),
        new Vector3( 1,  1, -1),
        new Vector3(-1,  1, -1),
        new Vector3(-1,  1,  1)
    };

    private Vector3[] _bottom = new Vector3[]
    {
        new Vector3( 1, -1,  1),
        new Vector3( 1, -1, -1),
        new Vector3(-1, -1, -1),
        new Vector3(-1, -1,  1)
    };

    private int _current;

    private bool _onBottomNow;
    private bool _nowMoving;


    public void Initialize()
    {
        MoveTo(AdaptToBounds(_top[_current]));
    }

    private Queue<CameraCommand> _cmdQ = new Queue<CameraCommand>();

    public void AddMoveCommand(CameraCommand command)
    {
        _cmdQ.Enqueue(command);
    }

    public void ClearAllCommands()
    {
        _cmdQ.Clear();
        _nowMoving = false;
    }

    private void Update()
    {
        if (!_nowMoving && _cmdQ.Count > 0)
        {
            switch (_cmdQ.Dequeue())
            {
                case CameraCommand.LEFT:
                    _current = --_current < 0 ? 3 : _current;
                    MoveTo(AdaptToBounds(_onBottomNow ? _bottom[_current] : _top[_current]));
                    break;
                case CameraCommand.RIGHT:
                    _current = ++_current > 3 ? 0 : _current;
                    MoveTo(AdaptToBounds(_onBottomNow ? _bottom[_current] : _top[_current]));
                    break;
                case CameraCommand.UP:
                    if (!_onBottomNow) return;
                    _onBottomNow = false;
                    MoveTo(AdaptToBounds(_top[_current]));
                    break;
                case CameraCommand.DOWN:
                    if (_onBottomNow) return;
                    _onBottomNow = true;
                    MoveTo(AdaptToBounds(_bottom[_current]));
                    break;
            }
        }
    }

    private void MoveTo(Vector3 pos)
    {
        if (transform.position == pos) return;

        StartCoroutine(MoveProcess(pos));
        _nowMoving = true;
    }

    private IEnumerator MoveProcess(Vector3 to)
    {
        while (transform.position != to)
        {
            transform.position = Vector3.MoveTowards(transform.position, to, _moveSpeed);
            transform.LookAt(_cube3d);
            yield return new WaitForEndOfFrame();
        }
        _nowMoving = false;
    }

    private Vector3 AdaptToBounds(Vector3 pos)
    {
        pos.x *= _bounds.x / 2;
        pos.y *= _bounds.y / 2;
        pos.z *= _bounds.z / 2;
        return pos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_cube3d.position, _bounds);
    }
}
