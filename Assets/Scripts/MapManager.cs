using System;
using CesiumForUnity;
using System.Collections;
using System.Collections.Generic;
using Data;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : Singleton<MapManager>
{
    [SerializeField] private Cesium3DTileset _cesium3DTileset;
    [SerializeField] private CesiumIonRasterOverlay _cesiumIonRasterOverlay;
    [SerializeField] private GameObject _waterQuadsHolder;

    [Header("UI Elements")]
    [SerializeField] private TMP_Dropdown _locationsDropdown;
    [SerializeField] private Toggle _mapOverlayToggle;
    [SerializeField] private Toggle _waterToggle;

    private bool _isShowingMap;
    private bool _isShowingWater;

    private const double SeaLevelOffset = -0.05; // adjust the altitude of all waterQuads by this value
    [SerializeField] private CesiumGlobeAnchor[] _waterQuads;
    private int _seaLevelDecreaseMultiplier;

    private void OnEnable()
    {
        VCameraManager.OnViewModeSwitch += HandleViewSwitch;
        EnhancedTouchManager.OnDroneViewTap += LowerSeaLevel;
    }

    private void OnDisable()
    {
        VCameraManager.OnViewModeSwitch -= HandleViewSwitch;
        EnhancedTouchManager.OnDroneViewTap -= LowerSeaLevel;
    }

    void Start()
    {
        //Utils.OnDebugMessage?.Invoke($"{_cesium3DTileset.url}");

        // Workaround as map shows by default even with the component disabled
        StartCoroutine(ResetMapOverlay());
        
        PopulateDropDown();

        _isShowingWater = true;
    }
    
    public void ToggleMapOverlay() 
    {
        _isShowingMap = !_isShowingMap;
        _cesiumIonRasterOverlay.enabled = _isShowingMap;
    }

    public void ToggleWater()
    {
        _isShowingWater = !_isShowingWater;
        _waterQuadsHolder.SetActive(_isShowingWater);
    }

    private void HandleViewSwitch(ViewMode viewMode)
    {
        if (viewMode is ViewMode.Overhead)
        {
            EnterMapMode();
        }
        else
        {
            ExitMapMode();
        }
    }

    private void EnterMapMode()
    {
        _isShowingMap = true;
        _mapOverlayToggle.interactable = true;
        _mapOverlayToggle.isOn = true;
        _cesiumIonRasterOverlay.enabled = true;
        
        _isShowingWater = false;
        _waterToggle.isOn = false;
        _waterToggle.interactable = false;
        _waterQuadsHolder.SetActive(false);

        ResetSeaLevel();
    }

    private void ExitMapMode()
    {
        _isShowingMap = false;
        _mapOverlayToggle.interactable = false;
        _mapOverlayToggle.isOn = false;
        _cesiumIonRasterOverlay.enabled = false;

        _isShowingWater = true;
        _waterToggle.isOn = true;
        _waterToggle.interactable = true;
        _waterQuadsHolder.SetActive(true);
    }

    IEnumerator ResetMapOverlay()
    {
        _cesiumIonRasterOverlay.enabled = true;
        yield return new WaitForSeconds(0.1f);
        
        // ReSharper disable once Unity.InefficientPropertyAccess
        _cesiumIonRasterOverlay.enabled = false;
        
    }

    // Populate Dropdown options with values from PresetLocations enum
    private void PopulateDropDown()
    {
        var optionsList = new List<TMP_Dropdown.OptionData>();
        for (var i = 0; i < Enum.GetNames(typeof(PresetLocations)).Length; i++)
        {
            var newData = new TMP_Dropdown.OptionData
            {
                text = ((PresetLocations)i).ToString()
            };
            optionsList.Add(newData);      
        }
        _locationsDropdown.options.Clear();
        _locationsDropdown.options = optionsList;
    }

    private void LowerSeaLevel()
    {
        foreach (var globeAnchor in _waterQuads)
        {
            var newLongLatHeight = new double3(
                globeAnchor.longitudeLatitudeHeight.x, 
                globeAnchor.longitudeLatitudeHeight.y, 
                globeAnchor.longitudeLatitudeHeight.z + SeaLevelOffset);
            globeAnchor.longitudeLatitudeHeight = newLongLatHeight;
        }
        
        _seaLevelDecreaseMultiplier++;
    }

    private void ResetSeaLevel()
    {
        foreach (var globeAnchor in _waterQuads)
        {
            var newLongLatHeight = new double3(
                globeAnchor.longitudeLatitudeHeight.x, 
                globeAnchor.longitudeLatitudeHeight.y, 
                globeAnchor.longitudeLatitudeHeight.z - SeaLevelOffset * _seaLevelDecreaseMultiplier);
            globeAnchor.longitudeLatitudeHeight = newLongLatHeight;
        }

        _seaLevelDecreaseMultiplier = 0;
    }
}
