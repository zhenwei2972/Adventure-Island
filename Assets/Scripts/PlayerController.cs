using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerInput _playerInput;
    [SerializeField] float _characterMaxSpeed;

    AnimationState _animationState;
    Vector2 _gamepadInput;
    
    private bool _isGamepadControlEnabled;

    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _animationState = GetComponent<AnimationState>();
        //_characterMaxSpeed = 15f;
        _gamepadInput = new Vector2(0f,0f);
    }

    void Update()
    {
        if (!_isGamepadControlEnabled)
            return;
            
        if (_gamepadInput.sqrMagnitude >= 0.01f)
        {
            RotateAndMove(_gamepadInput);
            _animationState.SetRunSpeed(_gamepadInput.sqrMagnitude);
        }
        else 
        {
            _animationState.SetRunSpeed(0f);
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
        transform.position += _characterMaxSpeed * gamepadInput.sqrMagnitude * Time.deltaTime * transform.forward;
    }
}
