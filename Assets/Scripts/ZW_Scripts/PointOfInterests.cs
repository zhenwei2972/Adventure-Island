using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Google.XR.ARCoreExtensions;
using UnityEngine.XR.ARFoundation;
using Google.XR.ARCoreExtensions.GeospatialCreator.Internal;
using CesiumForUnity;
using Unity.Mathematics;
using Google.XR.ARCoreExtensions.Samples.Geospatial;
using TMPro;


public class PointsOfInterest : MonoBehaviour
{
    //set your api key here
    private string apiKey = "AIzaSyBApRjY39Y4AzMf2NBica2KJT10mZ_H-QY";
    private string baseUrl = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?";
    public int radius = 3000; // Radius in meters (adjust as needed)
    public string searchString = "Sentosa"; // Search Keyword( same as google search ) (e.g., restaurant, museum, etc.)
    private int heightOffset = 15;
    public GameObject LocationIcon;
    private List<GameObject> poiGroup;
    private GeospatialController geospatialController;
    List<Bookmark> bookmarkList;
    public TMP_InputField searchBar;
    public GameObject MockLocationCard;
    public GameObject MockLocationUI;
    public GameObject MockListView;
    //placesResponse is the main placeResponse, updated everytime we fetch new data.
    PlacesResponse placesResponse;
    PlacesResponse placesResponseFood;
    PlacesResponse placesResponseEntertainment;
    PlacesResponse placesResponseShopping;
    public TMP_Text foodCountText;
    public TMP_Text entertainmentCountText;
    public TMP_Text shoppingCountText;
    public GameObject prefabComponent;
    public GameObject rootPosition;
    GameObject AllPoi;
    float userLatitute;
    float userLongitude;
    public GameObject CloseListViewBtn;
    public GameObject prefabBottomBar;
    public int maxPlaces = 3;
    public GameObject OnPanButtons;
    private float[] latLong = new float[2];
    private Place selectedTopPlace;
    public Vector3 targetPlace;
    CesiumGeoreference georeference;
    private GameObject targetFollow;
    public CameraMoveToLocation camMoveScript;
    public VCameraManager vCamManager;

    //tbd selected UI and fly to selected object
    void Awake()
    {
        //initialize a new place of interest group
        poiGroup = new List<GameObject>();

    }

    public Place GetSelectedTopPlace()
    {
        return selectedTopPlace;
    }
    
    private void SetSelectedTopPlace(Place _place)
    {
        selectedTopPlace = _place;
    }
    // calculate the straight line distance between 2 lat,long points 
    public static float CalculateHaversineDistance(float latitude1, float longitude1, float latitude2, float longitude2)
    {
        // Convert latitude and longitude to radians.
        float lat1 = Mathf.Deg2Rad * latitude1;
        float lon1 = Mathf.Deg2Rad * longitude1;
        float lat2 = Mathf.Deg2Rad * latitude2;
        float lon2 = Mathf.Deg2Rad * longitude2;

        // Calculate the distance between the two points using the Haversine formula.
        float distance = 6371 * 2 * Mathf.Asin(Mathf.Sqrt(Mathf.Pow(Mathf.Sin((lat2 - lat1) / 2), 2) + Mathf.Cos(lat1) * Mathf.Cos(lat2) * Mathf.Pow(Mathf.Sin((lon2 - lon1) / 2), 2)));

        distance = Mathf.Round(distance * 100.0f) * 0.01f;
       
        return distance;
    }
    private void Start()
    {
        georeference = GetComponent<CesiumGeoreference>();
        float[] currLatLong = GetDeviceLatLong();
        userLatitute = currLatLong[0];
        userLongitude = currLatLong[1];
        geospatialController = this.GetComponent<GeospatialController>();
        // Add listener to detect when 'enter' is pressed

        searchBar.onEndEdit.AddListener(OnSearch);


    }
    //most of activity is triggered onSearch
    public void OnSearch(string arg0)
    {
        prefabBottomBar.SetActive(true);
        string place = searchBar.text;
        searchString = place;
        //After API request successful, state change to trigger UI changes
        StartCoroutine(MakeAPIRequest(searchString));
/*        if (searchString == "Flower dome")
        {
            MockLocationCard.SetActive(true);
            MockLocationUI.SetActive(true);
        }*/
        StartCoroutine(MakeAPIRequestByType("restaurant", placesResponseFood));
        StartCoroutine(MakeAPIRequestByType("tourist_attraction", placesResponseEntertainment));
        StartCoroutine(MakeAPIRequestByType("shopping_mall", placesResponseShopping));

    }
    public void MoveCam()
    {
        camMoveScript.MoveToSelectedPlace();
        vCamManager.UseOnPlaceCamera();
    }
    public void DisablePanButtons()
    {
        OnPanButtons.SetActive(false);
    }
    public void spawnFoodPOI()
    {
        spawnPlaces(placesResponseFood);
    }
    public void spawnEntertainmentPOI()
    {
        spawnPlaces(placesResponseEntertainment);
    }
    public void spawnShoppingPOI()
    {
        spawnPlaces(placesResponseShopping);
    }
    public void DisableTopBar()
    {
        prefabBottomBar.SetActive(false);
    }
    public void ToggleOffMocks()
    {
        MockLocationCard.SetActive(false);
        MockLocationUI.SetActive(false);
        MockListView.SetActive(false);
    }
    public void TurnOnMockListView()
    {
        MockListView.SetActive(true);
    }
    public float[] GetDeviceLatLong()
    {
    #if UNITY_ANDROID && !UNITY_EDITOR
                LocationInfo tempLocationInfo = Input.location.lastData;
                latLong[0] = tempLocationInfo.latitude;
                latLong[1] = tempLocationInfo.longitude;
    #endif
    #if UNITY_EDITOR
            latLong[0] = 1.2861879896465431f;
            latLong[1] = 103.85996367181922f;
    #endif

        return latLong;
    }
    //call places api with keyword from Input Field
    public IEnumerator MakeAPIRequest(string _searchString)
    {
       float[] currLatLong = GetDeviceLatLong();
       string url = baseUrl + "location=" + currLatLong[0] + "," + currLatLong[1] + "&radius=" + radius + "&keyword=" + _searchString + "&key=" + apiKey;

        Debug.Log(url);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                string response = webRequest.downloadHandler.text;
                Debug.Log("API Response: " + response);

                // Deserialize the JSON response into C# objects
                placesResponse = JsonConvert.DeserializeObject<PlacesResponse>(response);
                spawnPlaces(placesResponse);
                SetSelectedTopPlace(placesResponse.results[0]);
                //only set pan button to active when panning.
                //OnPanButtons.SetActive(true);
                MoveCam();
            }

        }

    }

    public IEnumerator MakeAPIRequestByType(string _type, PlacesResponse _savedPlacesResponse)
    {
        
        float[] currLatLong = GetDeviceLatLong();
        string url = baseUrl + "location=" + currLatLong[0] + "," + currLatLong[1] + "&radius=" + 800 + "&type=" + _type + "&key=" + apiKey;

        Debug.Log(url);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                string response = webRequest.downloadHandler.text;
                Debug.Log("API Response: " + response);

                // Deserialize the JSON response into C# objects
                placesResponse = JsonConvert.DeserializeObject<PlacesResponse>(response);
                SetPlaceResponseByType(_type, placesResponse);
                Debug.Log(placesResponse.results.Count + _type);



            }

        }

    }
    private void SetPlaceResponseByType(string _type, PlacesResponse _placesResponse)
    {
        if(_type == "restaurant")
        {
            placesResponseFood = _placesResponse;
            foodCountText.text = placesResponseFood.results.Count.ToString();
        }
        else if (_type == "tourist_attraction")
        {
            placesResponseEntertainment = _placesResponse;
            entertainmentCountText.text = placesResponseEntertainment.results.Count.ToString();
        }
        else if(_type == "shopping_mall")
        {
            placesResponseShopping = _placesResponse;
            shoppingCountText.text = placesResponseShopping.results.Count.ToString();
        }
    }
    public Vector3 ConvertFromLatLngToUnity(double3 _latLng)
    {
        
        double3 ecef = CesiumWgs84Ellipsoid.LongitudeLatitudeHeightToEarthCenteredEarthFixed(_latLng);
        double3 unityPos = georeference.TransformEarthCenteredEarthFixedPositionToUnity(ecef);
        Vector3 unityVector3Pos = new Vector3((float)unityPos.x, 50f, (float)unityPos.z);
        return unityVector3Pos;
    }
    public Vector3 GetSelectedPlaceVector3()
    {
        return targetPlace;
    }
    public GameObject GetTargetFollow()
    {
        return targetFollow;
    }
    public void spawnPlaces(PlacesResponse placesResponse)
    {
        if (placesResponse.results != null)
        {
            float[] currLatLong = GetDeviceLatLong();
            userLatitute = currLatLong[0];
            userLongitude = currLatLong[1];
            Destroy(AllPoi);
            DeleteListViewChild();
            AllPoi = new GameObject();
            int count = 0;
            foreach (Place place in placesResponse.results)
            {
                //constraint to maximum 3 'places' signboards on the map
                //observed that it's very messy if too many signboards.
                if(count==maxPlaces)
                {
                    break;
                }
                CesiumGeoreference georeference = GetComponent<CesiumGeoreference>();
                heightOffset = 40;
                double3 longlatheight = new double3(place.geometry.location.lng, place.geometry.location.lat, heightOffset);
                Vector3 unityVector3Pos = ConvertFromLatLngToUnity(longlatheight);
                
                // Instantiate your prefab at the anchor's position
                GameObject iconPOI = Instantiate(LocationIcon, unityVector3Pos, Quaternion.identity);
                if (count == 0)
                {
                    targetFollow = iconPOI;
                    targetPlace = unityVector3Pos;
                }
                float distance = CalculateHaversineDistance(userLatitute, userLongitude, (float)place.geometry.location.lat, (float)place.geometry.location.lng);
                iconPOI.GetComponent<PlaceAPI_Manager>().SetTextFromAPI(place, distance);
                poiGroup.Add(iconPOI);

                iconPOI.transform.parent = AllPoi.transform;
                count++;

            }
        }
        else
        {
            Debug.LogWarning("No results found.");
        }
    }
    //using string matching is not ideal.(WARNING)
    public void populateListView(string _type)
    {
        PlacesResponse tempPlacesResponse = new PlacesResponse();
        DeleteListViewChild();
        if (_type == "restaurant")
        {
            tempPlacesResponse = placesResponseFood;

        }
        else if (_type == "tourist_attraction")
        {
            tempPlacesResponse = placesResponseEntertainment;
        }
        else if (_type == "shopping_mall")
        {
            tempPlacesResponse = placesResponseShopping;
        }
        CloseListViewBtn.SetActive(true);
        float verticalOffset = 213f;
        if (tempPlacesResponse.results != null)
        {
            float[] currLatLong = GetDeviceLatLong();
            userLatitute = currLatLong[0];
            userLongitude = currLatLong[1];

            //fix issues (BUG) if click more than once it will keep inserting..
            if (GetSelectedTopPlace() != tempPlacesResponse.results[0])
            {
                tempPlacesResponse.results.Insert(0, GetSelectedTopPlace());
            }
            int i = 1;
            foreach (Place place in tempPlacesResponse.results)
            {
              
                if (i <= 7)
                {
                    Vector3 listPosition = new Vector3(rootPosition.transform.position.x, rootPosition.transform.position.y - (verticalOffset*i), rootPosition.transform.position.z);
                    // Instantiate your prefab at the anchor's position
                    GameObject iconPOI = Instantiate(prefabComponent, listPosition, Quaternion.identity);
                    float distance = CalculateHaversineDistance(userLatitute, userLongitude, (float)place.geometry.location.lat, (float)place.geometry.location.lng);
                    //cleanup distance add KM and M

                    iconPOI.GetComponent<PlaceAPI_Manager>().SetTextFromAPI(place, distance);
                    iconPOI.transform.parent = rootPosition.transform;
                    i++;
                }


            }
            //reset placesResponse
            tempPlacesResponse = null;
        }
        else
        {
            Debug.LogWarning("No results found.");
        }
    }

    public void DeleteListViewChild()
    {
        foreach (Transform child in rootPosition.transform)
        {
            Destroy(child.gameObject);
        }
       
    }


    //Extras do not use.
    public void spawnPlacesAR(PlacesResponse placesResponse)
    {
        foreach (Place place in placesResponse.results)
        {
            Debug.Log("Running AR mode");
            CesiumGeoreference georeference = GetComponent<CesiumGeoreference>();
            Vector3 origin = new Vector3(0, 0, 0);
            // Instantiate your prefab at the anchor's position
            GameObject iconPOI = Instantiate(LocationIcon, origin, Quaternion.identity);
            iconPOI.AddComponent<ARGeospatialCreatorAnchor>();
            iconPOI.GetComponent<ARGeospatialCreatorAnchor>().Latitude = place.geometry.location.lat;
            iconPOI.GetComponent<ARGeospatialCreatorAnchor>().Longitude = place.geometry.location.lng;
            float haversineDistance = CalculateHaversineDistance((float)place.geometry.location.lat, (float)place.geometry.location.lng, (float)georeference.latitude, (float)georeference.longitude);
            iconPOI.GetComponent<POI_Manager>().SetText(place.name + " " + haversineDistance);
            poiGroup.Add(iconPOI);
        }
    }
    public void FootMode()
    {
        /*if (AllPoi != null)
        {
            
            foreach (Transform child in AllPoi.transform)
            {
                child.GetComponent<POI_Manager>().FootMode();
            }
        }*/
    }
    [Serializable]
    public class Place
    {
        public string name;
        public Geometry geometry;
        public string vicinity;
        public float rating;
        public OpeningHours opening_hours;
        public List<Photos> photos { get; set; }

    }
    public class OpeningHours
    {
        public bool open_now { get; set; }
    }
    [Serializable]
    public class Geometry
    {
        public Location location;
    }
    [Serializable]
    public class Location
    {
        public double lat;
        public double lng;
    }
    public class Photos
    {
        public int height { get; set; }
        public List<string> html_attributions { get; set; }
        public string photo_reference { get; set; }
        public int width { get; set; }
    }
    [Serializable]
    public class PlacesResponse
    {
        public List<Place> results;
    }
}