using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosCubeCenter : MonoBehaviour
{
    [SerializeField]
    private bool _showSphere = true;
    [SerializeField]
    private float _radius = 1;
    [SerializeField]
    private Color _sphereColor = Color.white;

    [SerializeField]
    private bool _showLabel = true;
    [SerializeField]
    private string _label;
    [SerializeField]
    private Color _labelColor;
    [SerializeField]
    private int _labelFontSize;
    [SerializeField]
    private Vector3 _labelOffset;

    [SerializeField]
    private bool _showLocalCoordinates = true;
    [SerializeField]
    private float _axisLen = 1;

    private void OnDrawGizmos()
    {
        Gizmos.color = _sphereColor;

        if (_showSphere)
        {
            Gizmos.DrawWireSphere(transform.position, _radius);
        }

        if (_showLabel)
        {
            GizmosUtils.DrawText(GUI.skin, _label, transform.position + _labelOffset, _labelColor, _labelFontSize);
        }

        if (_showLocalCoordinates)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.right * _axisLen);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.up * _axisLen);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * _axisLen);
        }
    }
}
