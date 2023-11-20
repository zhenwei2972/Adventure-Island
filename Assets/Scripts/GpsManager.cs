using CesiumForUnity;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Data;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class GpsManager : MonoBehaviour
{
    // Testing code for now. Used for testing Gps related functions
    [SerializeField] private GameObject _playerShip;
    [SerializeField] private GameObject _playerCharacter;
    [SerializeField] private GameObject _droneCharacter;
    [SerializeField] private GameObject _overheadCamTarget;
    private CesiumGlobeAnchor _playerShipAnchor;
    private CesiumGlobeAnchor _playerCharacterAnchor;
    private CesiumGlobeAnchor _droneCharacterAnchor;
    private CesiumGlobeAnchor _overheadCamAnchor;
    
    [Header("Saved Locations")]
    [SerializeField] private PresetLocations _currLocation;

    [Header("Cesium")] 
    [SerializeField] private CesiumGeoreference _georeference;
    [SerializeField] private CesiumSubScene _subScene;

    void Start()
    {
        _playerShipAnchor = _playerShip.GetComponent<CesiumGlobeAnchor>();
        _playerCharacterAnchor = _playerCharacter.GetComponent<CesiumGlobeAnchor>();
        _droneCharacterAnchor = _droneCharacter.GetComponent<CesiumGlobeAnchor>();
        _overheadCamAnchor = _overheadCamTarget.GetComponent<CesiumGlobeAnchor>();

        SetAnchorLocation(_playerShipAnchor, SavedLocations.AnchoredObjects["PlayerShip"]);
        
        //Debug.Log($"_playerShipAnchor.positionGlobeFixed = {_playerShipAnchor.positionGlobeFixed}");
        
        StartCoroutine(GetGpsLocation());
    }
    public void RefreshGPSLocation()
    {
        StartCoroutine(GetGpsLocation());
    }
    IEnumerator GetGpsLocation() 
    {
        // Check if the user has location service enabled.
        if (!Input.location.isEnabledByUser)
        {
            Utils.OnDebugMessage?.Invoke("Location not enabled on device or app does not have permission to access location");
            yield break;
        }
        
        // Starts the location service.
        Input.location.Start();

        // Waits until the location service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1f);
            maxWait--;
        }

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            Utils.OnDebugMessage?.Invoke("Timed out");
            yield break;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Utils.OnDebugMessage?.Invoke("Unable to determine device location");
            yield break;
        }

        //Debug.LogWarning($"Input.location.status={Input.location.status}");

        // If the connection succeeded, this retrieves the device's current location and displays it in the Console window.
        var locationData = Input.location.lastData;
        
        Utils.OnDebugMessage?.Invoke($"Location: " +
            $"longitude={locationData.longitude}, " +
            $"latitude={locationData.latitude}, " +
            $"altitude={locationData.altitude}, " +
            $"horizontalAccuracy={locationData.horizontalAccuracy}, " +
            $"timestamp={locationData.timestamp}");
        
        // Try to get north
        Utils.OnDebugMessage?.Invoke($"Input.compass.trueHeading: {Input.compass.trueHeading}");

#if UNITY_EDITOR
        UpdateLocation(SavedLocations.Attractions[PresetLocations.Default.ToString()]);
        
#else
        var currentLocation = new double3( locationData.longitude, locationData.latitude, locationData.altitude);
        UpdateLocation(currentLocation);

#endif
        // Stops the location service if there is no need to query location updates continuously.
        Input.location.Stop();
    }
    
    private void SetAnchorLocation(CesiumGlobeAnchor anchorToSet, double3 longLatHeight) 
    {
        anchorToSet.longitudeLatitudeHeight = longLatHeight;
    }
    
    private void SetAnchorLocation(CesiumGlobeAnchor anchorToSet, double longitude, double latitude, double altitude) 
    {
        /*
        // Refer to this if conversion between longLatHeight and earthCenteredEarthFixed is needed
        var earthCenteredEarthFixedPos = CesiumWgs84Ellipsoid.LongitudeLatitudeHeightToEarthCenteredEarthFixed(
            new double3(longitude, latitude, altitude));
        */
        anchorToSet.longitudeLatitudeHeight = new double3(longitude, latitude, altitude);
    }

    private void UpdateLocation(double3 longLatHeight)
    {
        _georeference.SetOriginLongitudeLatitudeHeight(longLatHeight.x, longLatHeight.y, longLatHeight.z);
        
        SetAnchorLocation(_playerCharacterAnchor, longLatHeight.x, longLatHeight.y, longLatHeight.z);
        SetAnchorLocation(_droneCharacterAnchor, longLatHeight.x, longLatHeight.y, _droneCharacterAnchor.longitudeLatitudeHeight.z);
        _overheadCamAnchor.longitudeLatitudeHeight = longLatHeight;
    }
    
    // Handles the location selection from dropdown
    public void OnDropDownChanged(TMP_Dropdown dropdown)
    {
        var presetLocation = ((PresetLocations)dropdown.value).ToString();
        UpdateLocation(SavedLocations.Attractions[presetLocation]);
    }
}
