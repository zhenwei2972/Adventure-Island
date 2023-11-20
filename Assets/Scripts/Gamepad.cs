using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gamepad : MonoBehaviour
{
    [SerializeField] private Image _gamePadImage;
    [SerializeField] private Image _backingImage;
    private void OnEnable()
    {
        EnhancedTouchManager.OnStartDrag += Show;
        EnhancedTouchManager.OnStopDrag += Hide;
        VCameraManager.OnViewModeSwitch += HandleViewModeSwitch;
    }

    private void OnDisable()
    {
        EnhancedTouchManager.OnStartDrag -= Show;
        EnhancedTouchManager.OnStopDrag -= Hide;
        VCameraManager.OnViewModeSwitch -= HandleViewModeSwitch;
    }

    private void Start()
    {
        _backingImage = GetComponent<Image>();
        Hide();
    }

    private void HandleViewModeSwitch(ViewMode viewMode)
    {
        
    }

    private void Show(Vector2 touchPosition)
    {
        _gamePadImage.enabled = true;
        _backingImage.enabled = true;
        transform.position = new Vector3(touchPosition.x, touchPosition.y, 0f);
    }
    
    private void Hide()
    {
        _gamePadImage.enabled = false;
        _backingImage.enabled = false;
    }
}
