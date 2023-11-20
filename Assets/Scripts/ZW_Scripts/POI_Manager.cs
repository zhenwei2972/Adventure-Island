using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.XR.ARCoreExtensions;
using Google.XR.ARCoreExtensions.GeospatialCreator.Internal;
using TMPro;

public class POI_Manager : MonoBehaviour
    {
    private Vector3 scaleChange,overheadModeRotation,heightChange,footModeRotation;

    public void SetText(string _txt)
    {
        GameObject childObject = transform.GetChild(0).gameObject;
        childObject = childObject.GetComponent<Transform>().GetChild(0).gameObject;

        // Get the TextMeshPro component from the child object.
        TextMeshPro textMeshPro = childObject.GetComponent<TextMeshPro>();

        textMeshPro.text = _txt;


    }
    public void Start()
    {
        scaleChange = new Vector3(10f, 10f, 10f);
        overheadModeRotation = new Vector3(90f, 0f, 0f);
        heightChange = new Vector3(0f, 40f, 0f);
        footModeRotation = new Vector3(-90f, 0f, 0f);
        OverheadMode();
    }
    public void OverheadMode()
    {
        
        transform.localScale += scaleChange;
        transform.localPosition += heightChange;
        transform.Rotate(overheadModeRotation);
    }
    public void FootMode()
    {
        transform.localScale -= scaleChange;
        transform.localPosition -= heightChange;
        transform.Rotate(footModeRotation);
    }
    }

