using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlaceAPI_Manager : MonoBehaviour
{
    public TMP_Text shopNameTMP;
    public TMP_Text ratingTMP;
    public TMP_Text openStatusTMP;
    public TMP_Text distanceTMP;
    public Image placeImage;
    private string apiKey = "AIzaSyAzphIBAueM3WnETHq6Vn6Ai5HGG1SiIeU";
    public void SetTextFromAPI(PointsOfInterest.Place _placeData, float _distance)
    {
        if (_placeData != null)
        {
            if (_placeData.name != null)
            {
                shopNameTMP.text = _placeData.name;
            }

            
                ratingTMP.text = "Rating:" + _placeData?.rating;
            

            if (_placeData.opening_hours != null && _placeData.opening_hours?.open_now != null)
            {
                openStatusTMP.text = _placeData.opening_hours.open_now ? "Open" : "Closed";
            }
            //scale distance to meters 
            _distance = _distance * 1000;
            //if >1000m , change to km, else display as meters
            if (_distance > 1000)
            {

                distanceTMP.text = (_distance + " km") ;
            }
            else
            {
                distanceTMP.text = _distance + " m";
            }
            

            if (_placeData.photos != null && _placeData.photos.Count > 0 && _placeData.photos[0].photo_reference != null)
            {
                StartCoroutine(GetImage(_placeData.photos[0].photo_reference));
            }
        }
    }

    public IEnumerator GetImage(string _photoReference)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture("https://maps.googleapis.com/maps/api/place/photo?maxwidth=1024&photoreference=" + _photoReference + "&key=" + apiKey);

        // Send the UnityWebRequest
        yield return request.SendWebRequest();

        // Check for errors
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError("Error fetching place photo: " + request.error);
            yield break;
        }

        // Extract the texture from the UnityWebRequest
        Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

        // Set the texture on the Image component
        placeImage.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero);
    }


}
