using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DottedLine : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _target1;
    [SerializeField] private Transform _target2;

    [SerializeField] private Material _lineMaterial;

    [SerializeField] private float _tilingValue;
    private static readonly int Tiling = Shader.PropertyToID("_Tiling");
    private float _startTilingValue;
    private float _currDistance;
    private float _scale;

    void OnEnable()
    {
        var target1Pos = _target1.position;
        var target2Pos = _target2.position;
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.SetPosition(0, target1Pos);
        _lineRenderer.SetPosition(1, target2Pos);
        _lineRenderer.enabled = true;
        _lineMaterial = _lineRenderer.material;
        _tilingValue = _lineMaterial.GetFloat(Tiling);
        _startTilingValue = _tilingValue;
    }

    private void OnDisable()
    {
        _lineRenderer.enabled = false;
    }

    void Update()
    {
        var target1Pos = _target1.position;
        var target2Pos = _target2.position;
        
        _currDistance = Vector3.Distance(target1Pos, target2Pos);

        _scale = _currDistance / DroneController.DroneMaxHeight;
        _lineRenderer.SetPosition(0, target1Pos);
        _lineRenderer.SetPosition(1, target2Pos);
        _lineMaterial.SetFloat(Tiling, _startTilingValue * _scale);
    }
}
