using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] PlayerInput _playerInput;

    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    public void OnTouch(InputAction.CallbackContext context)
    {
        Debug.Log(context.phase);
    }

    public void OnTouchPosition(InputAction.CallbackContext context)
    {
        Debug.Log($"Phase {context.phase}, Value = {context.ReadValue<Vector2>()}");

    }

    public void OnPointerPosition(InputAction.CallbackContext context)
    {
        Debug.Log($"Phase = {context.phase}, position = {context.ReadValue<Vector2>()}");
    }


}
