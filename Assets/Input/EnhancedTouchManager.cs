using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class EnhancedTouchManager : Singleton<EnhancedTouchManager>
{
    private Camera _mainCamera;

    public static Action<Vector2> OnSwipeStart;
    public static Action<Vector2, double> OnSwipeEnd;
    public static Action<Vector2> OnDragMove;
    public static Action<Vector2> OnStartDrag;
    public static Action OnStopDrag;
    public static Action<Vector2> OnDragRotate;

    public static Action<Vector2, Vector2> OnPinch;

    public static Action OnDroneViewTap;

    private double _duration;
    
    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        Touch.onFingerDown += OnFingerDown;
        Touch.onFingerMove += OnFingerMove;
        Touch.onFingerUp += OnFingerUp;
    }

    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
        Touch.onFingerDown -= OnFingerDown;
        Touch.onFingerMove -= OnFingerMove;
        Touch.onFingerUp -= OnFingerUp;
    }
    
    void OnFingerDown(Finger finger)
    {
        switch (VCameraManager.Instance.CurrViewMode)
        {
            case ViewMode.Overhead:
                break;

            case ViewMode.Drone:
                OnStartDrag?.Invoke(finger.currentTouch.screenPosition);
                break;
            
            case ViewMode.OnFoot:
                OnSwipeStart?.Invoke(finger.currentTouch.screenPosition);
                break;
        }
    }

    void OnFingerMove(Finger finger)
    {
        switch (VCameraManager.Instance.CurrViewMode)
        {
            case ViewMode.Overhead:
                OnDragMove?.Invoke(finger.currentTouch.delta);
                break;
            
            case ViewMode.Drone:
                var activeTouches = Touch.activeTouches;
                if (activeTouches.Count > 1)
                {
                    // pinch
                    var touch1 = activeTouches[0];
                    var touch2 = activeTouches[1];
                    OnPinch?.Invoke(touch1.screenPosition, touch2.screenPosition);
                }
                break;
            
            case ViewMode.OnFoot:
                OnDragRotate?.Invoke(finger.currentTouch.delta);
                break;
        }
    }

    void OnFingerUp(Finger finger)
    {
        switch (VCameraManager.Instance.CurrViewMode)
        {
            case ViewMode.Overhead:
                break;
            
            case ViewMode.Drone:
                OnStopDrag?.Invoke();
                
                /*
                _duration = finger.currentTouch.time - finger.currentTouch.startTime;
                if (_duration <= 0.25f)
                    OnDroneViewTap?.Invoke();
                */
                break;
            
            case ViewMode.OnFoot:
                _duration = finger.currentTouch.time - finger.currentTouch.startTime;
                OnSwipeEnd?.Invoke(finger.currentTouch.screenPosition, _duration);
                break;
        }
    }
}
