using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

/// <summary>
/// Used to visualize a swipe
/// </summary>
public class SwipeVisualCursor : MonoBehaviour
{
    private Camera _mainCamera;
    private TrailRenderer _trailRenderer;
    private MeshRenderer _meshRenderer;

    private bool _isMoving;
    private Vector3 _startPos;
    private Vector3 _targetPos;
    private const float Speed = 1000f;
    private float _duration;
    private float _moveStep;

    void Start()
    {
        _mainCamera = Camera.main;
        _trailRenderer = GetComponent<TrailRenderer>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _trailRenderer.enabled = false;
        _meshRenderer.enabled = false;
        _targetPos = transform.position;
        _isMoving = false;
    }

    void OnEnable()
    {
        EnhancedTouchManager.OnSwipeStart += HandleSwipeStart;
        EnhancedTouchManager.OnSwipeEnd += HandleSwipeEnd;
    }

    void OnDisable()
    {
        EnhancedTouchManager.OnSwipeStart -= HandleSwipeStart;
        EnhancedTouchManager.OnSwipeEnd -= HandleSwipeEnd;
    }

    void Update()
    {
        if (!_isMoving) return;
        
        MoveCursor();
    }

    private void HandleSwipeStart(Vector2 startScreenPos)
    {
        _trailRenderer.enabled = false;
        _meshRenderer.enabled = false;
        _isMoving = false;

        _startPos = Utils.ScreenToWorldOffset(_mainCamera, startScreenPos);
    }

    private void HandleSwipeEnd(Vector2 endScreenPos, double duration)
    {
        _trailRenderer.enabled = true;
        _meshRenderer.enabled = true;
        _isMoving = true;

        _duration = (float)duration;
        _targetPos = Utils.ScreenToWorldOffset(_mainCamera, endScreenPos);
        transform.position = _startPos;
    }

    private void MoveCursor()
    {
        if (!_isMoving) return;

        _moveStep = Speed * _duration * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, _targetPos, _moveStep);

        if (Vector3.Distance(transform.position, _targetPos) < 0.001f)
        {
            transform.position = _targetPos;
            _trailRenderer.enabled = false;
            _meshRenderer.enabled = false;
            _isMoving = false;
        }
    }
}
