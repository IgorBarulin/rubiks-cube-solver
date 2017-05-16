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
    }
}
