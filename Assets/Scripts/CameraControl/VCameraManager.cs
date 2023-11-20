using System;
using System.Collections;
using System.Collections.Generic;
using CameraControl;
using Cinemachine;
using UnityEngine;

public enum ViewMode
{
    OnFoot,
    Drone,
    Overhead,
    Place
}

public class VCameraManager : Singleton<VCameraManager>
{
    [SerializeField] private CinemachineVirtualCamera _onFootCamera;
    [SerializeField] private CinemachineVirtualCamera _droneCamera;
    [SerializeField] private CinemachineVirtualCamera _overheadCamera;
    [SerializeField] private CinemachineVirtualCamera _placeCamera;
    [SerializeField] private ViewMode _currViewMode;
    public ViewMode CurrViewMode => _currViewMode;

    public static Action<ViewMode> OnViewModeSwitch;

    private const float RotationSensitivity = 10f;
    private const float RotationSpeed = 50f;
    private float _rotationDelta;
    [SerializeField] private Transform _rotationTargetRig;
    [SerializeField] private Transform _camRotationTarget;
    private bool _isRotating;

    private void OnEnable()
    {
        EnhancedTouchManager.OnDragRotate += HandleDragRotate;
    }

    private void OnDisable()
    {
        EnhancedTouchManager.OnDragRotate -= HandleDragRotate;
    }

    private void Start()
    {
        _onFootCamera.Priority = 5;
        _droneCamera.Priority = 2;
        _overheadCamera.Priority = 1;
        _placeCamera.Priority = 3;
        _currViewMode = ViewMode.OnFoot;
    }

    private void Update()
    {
        if (!_isRotating) return;
        
        // Stop rotating if rotation is close to target rotation
        var dotProduct = Vector3.Dot(_onFootCamera.transform.forward, _camRotationTarget.transform.forward);
        if (dotProduct > 0.99995f)
        {
            _isRotating = false;
            _onFootCamera.transform.rotation = _camRotationTarget.rotation;
            return;
        }
        
        _onFootCamera.transform.rotation = Quaternion.RotateTowards(_onFootCamera.transform.rotation, _camRotationTarget.rotation,
                RotationSpeed * Time.deltaTime);
    }

    public void UseOnFootCamera()
    {
        // Reset RotationTargetRig rotation
        _rotationTargetRig.transform.rotation = Quaternion.Euler(0f,0f,0f);
        _onFootCamera.transform.rotation = _camRotationTarget.rotation;
        
        _onFootCamera.Priority = 5;
        _placeCamera.Priority = 3;
        _droneCamera.Priority = 2;
        _overheadCamera.Priority = 1;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f); // reset rotation of the Camera Manager
        _currViewMode = ViewMode.OnFoot;
        OnViewModeSwitch?.Invoke(ViewMode.OnFoot);
    }
    public void UseOnPlaceCamera()
    {
        _onFootCamera.Priority = 3;
        _droneCamera.Priority = 1;
        _overheadCamera.Priority = 2;
        _placeCamera.Priority = 5;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f); // reset rotation of the Camera Manager
        _currViewMode = ViewMode.Place;
        OnViewModeSwitch?.Invoke(ViewMode.Place);
    }
    public void UseDroneCamera() 
    {
        _droneCamera.Priority = 5;
        _placeCamera.Priority = 3;
        _onFootCamera.Priority = 2;
        _overheadCamera.Priority = 1;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f); // reset rotation of the Camera Manager
        _currViewMode = ViewMode.Drone;
        OnViewModeSwitch?.Invoke(ViewMode.Drone);
    }

    public void UseOverheadCamera() 
    {
        _overheadCamera.Priority = 5;
        _onFootCamera.Priority = 1;
        _droneCamera.Priority = 2;
        _placeCamera.Priority = 3;
        _currViewMode = ViewMode.Overhead;
        OnViewModeSwitch?.Invoke(ViewMode.Overhead);
        OverheadCamTarget.Instance.SetToPlayerPosition();
    }
    
    private void HandleDragRotate(Vector2 dragRotateVector)
    {
        // Set the rotation of the rotation target rig. Camera will try to match that rotation over time
        _rotationTargetRig.transform.Rotate(Vector3.up, dragRotateVector.x / RotationSensitivity);
        _isRotating = true;
    }
}
