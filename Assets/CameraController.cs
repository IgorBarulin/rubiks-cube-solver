using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _radius;
    [SerializeField]
    private Transform _y;
    [SerializeField]
    private Transform _x;

    private float _savRad = 0;

    private void Update()
    {
        if (_radius != _savRad)
        {
            transform.position = new Vector3(0f, 0f, _radius);
            _savRad = _radius;

            transform.LookAt(_target.position);
        }



        float horizontal = Input.GetAxis("Mouse X");
        float vertical = Input.GetAxis("Mouse Y");

        if (Input.GetKey(KeyCode.Space))
        {
            if (Input.GetMouseButton(0) && horizontal != 0)
            {
                _x.RotateAround(_target.position, Vector3.up, _speed * horizontal * Time.deltaTime);
            }
            if (Input.GetMouseButton(0) && vertical != 0)
            {
                _y.RotateAround(_target.position, Vector3.right, _speed * vertical * Time.deltaTime);
            }

            _x.transform.LookAt(_target.position);
            _y.transform.LookAt(_target.position);
            transform.LookAt(_target.position);
        }

        //float horizontal = Input.GetAxis("Horizontal");
        //float vertical = Input.GetAxis("Vertical");

        //if (horizontal != 0)
        //{
        //    _x.RotateAround(_target.position, Vector3.up, _speed * horizontal * Time.deltaTime);
        //}
        //if (vertical != 0)
        //{
        //    _y.RotateAround(_target.position, Vector3.right, _speed * vertical * Time.deltaTime);
        //}
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_target.transform.position, _radius);
    }
}
