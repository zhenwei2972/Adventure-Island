using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DroneController : MonoBehaviour
{
    [SerializeField] Transform _playerTransform;
    [SerializeField] PlayerInput _playerInput;
    private const float DroneMaxSpeed = 50f;
    public const float DroneMaxHeight = 150f;
    private Vector2 _gamepadInput;
    private MeshRenderer _meshRenderer;
    private bool _isAtMaxHeight;

    [SerializeField] private DottedLine _dottedLine;

    private void OnEnable()
    {
        VCameraManager.OnViewModeSwitch += HandleViewModeChange;
    }

    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _gamepadInput = new Vector2(0f,0f);
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.enabled = false;
        _dottedLine.enabled = false;
    }

    void Update()
    {
        if (!_isAtMaxHeight)
            FlyUp();

        if (_gamepadInput.sqrMagnitude >= 0.01f)
        {
            RotateAndMove(_gamepadInput);
        }
    }

    // Hook into the Gamepad's OnMoveGamepad event
    private void OnMoveGamepad(InputValue inputValue)
    {
        _gamepadInput = inputValue.Get<Vector2>();
    }

    private void RotateAndMove(Vector2 gamepadInput) 
    {
        var directionVector = new Vector3(gamepadInput.x, 0f, gamepadInput.y);
        transform.rotation = Quaternion.LookRotation(directionVector);

        // always only move the character forward. Turning is handled by rotation
        transform.position += DroneMaxSpeed * gamepadInput.sqrMagnitude * Time.deltaTime * transform.forward;
    }

    private void FlyUp()
    {
        var currPos = transform.position;
        if (currPos.y < DroneMaxHeight)
        {
            transform.position += 50f * Time.deltaTime * transform.up;
        }
        else
        {
            _isAtMaxHeight = true;
        }
    }

    private void HandleViewModeChange(ViewMode viewMode)
    {
        if (viewMode == ViewMode.Drone)
        {
            transform.position = _playerTransform.position;
            _meshRenderer.enabled = true;
            _isAtMaxHeight = false;
            _dottedLine.enabled = true;
        }
        else
        {
            _meshRenderer.enabled = false;
            _dottedLine.enabled = false;
        }
    }
    
}
