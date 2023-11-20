using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LetsGoHandler : MonoBehaviour
{
    DirectionsManager directionManager;

    // Start is called before the first frame update
    void Start()
    {
        Button buttonComponent = GetComponent<Button>();
        UnityEvent onClickEvent = buttonComponent.onClick;
        onClickEvent.AddListener(SwitchToAR);
        directionManager = GameObject.FindWithTag("DirectionManager").GetComponent<DirectionsManager>();
    }

    void SwitchToAR()
    {
        Debug.Log("CallDirectionsAPI");
        //test later 
        directionManager.SetupDirectionsFlow();

        // Load the next scene
        //SceneManager.LoadScene("Geospatial");

    }

   

}
