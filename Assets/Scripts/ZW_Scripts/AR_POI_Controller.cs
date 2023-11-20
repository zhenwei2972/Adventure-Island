using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.XR.ARCoreExtensions.Samples.Geospatial;
using Google.XR.ARCoreExtensions.GeospatialCreator.Internal;
using TMPro;
public class AR_POI_Controller : MonoBehaviour
{
    public float lat;
    public float lon;
    public string name;
    // Start is called before the first frame update
    void Start()
    {
       this.gameObject.AddComponent<ARGeospatialCreatorAnchor>();
        this.GetComponent<ARGeospatialCreatorAnchor>().Latitude = lat;
        this.GetComponent<ARGeospatialCreatorAnchor>().Longitude = lon;
        SetText(name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetText(string _txt)
    {
        GameObject childObject = transform.GetChild(0).gameObject;
        childObject = childObject.GetComponent<Transform>().GetChild(0).gameObject;

        // Get the TextMeshPro component from the child object.
        TextMeshPro textMeshPro = childObject.GetComponent<TextMeshPro>();

        textMeshPro.text = _txt;


    }

}
