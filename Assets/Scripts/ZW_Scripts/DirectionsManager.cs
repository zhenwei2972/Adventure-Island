using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;
using System;
using TMPro;
public class DirectionsManager : MonoBehaviour
{
    public TMP_Text directionsText;
    public PointsOfInterest poi;
    public List<DirectionStep> currentStepList;
    public List<DirectionStep> GetStepList()
    {
        return currentStepList;
    }
    private void SetStepList(List<DirectionStep> _stepList)
    {
        currentStepList = _stepList;
    }
    
    public string apiKey = "AIzaSyBApRjY39Y4AzMf2NBica2KJT10mZ_H-QY";


    public void SetupDirectionsFlow()
    {
        float _originLat = (float)poi.GetDeviceLatLong()[0];
        float _originLng = (float)poi.GetDeviceLatLong()[1];
        float _destinationLat = (float)poi.GetSelectedTopPlace().geometry.location.lat;
        float _destinationLng = (float)poi.GetSelectedTopPlace().geometry.location.lng;
        GetDirections(_originLat, _originLng,_destinationLat, _destinationLng);
    }
    public void GetDirections(float originLatitude, float originLongitude, float destinationLatitude, float destinationLongitude)
    {
        string url = "https://maps.googleapis.com/maps/api/directions/json?" +
            "mode=walking" +"&" +
             /* "origin=" + "art science museum"+ "&" +
             "destination=" + "flower dome" + "&" +*/
             "origin=" + originLatitude + "," + originLongitude + "&" +
             "destination=" + destinationLatitude + "," + destinationLongitude + "&" +
            "key=" + apiKey;

        StartCoroutine(GetDirectionsRequest(url));
    }

    IEnumerator GetDirectionsRequest(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield

return request.SendWebRequest();

        if (request.isNetworkError)
        {
            Debug.LogError("Error getting directions: " + request.error);
            yield break;
        }

        string json = request.downloadHandler.text;
        DirectionsResponse directionsResponse = JsonUtility.FromJson<DirectionsResponse>(json);

        if (directionsResponse.status != "OK")
        {
            Debug.LogError("Error getting directions: " + directionsResponse.status);
            yield break;
        }

        List<DirectionStep> steps = directionsResponse.routes[0].legs[0].steps;
        SetStepList(steps);
        foreach (DirectionStep step in steps)
        {
            Debug.Log("Step: " + step.html_instructions + " " + step.distance.text);
        }
        //need to use distance to display the correct step
        directionsText.text = steps[0].html_instructions + " " + steps[0].distance.text;
    }
}

[Serializable]
public class DirectionsResponse
{
    public string status;
    public List<Route> routes;
}

[Serializable]
public class Route
{
    public List<Leg> legs;
}

[Serializable]
public class Leg
{
    public List<DirectionStep> steps;
}

[Serializable]
public class DirectionStep
{
    public string html_instructions;
    public Distance distance;
}

[Serializable]public class Distance

{
    public string text;
    public int value;
}