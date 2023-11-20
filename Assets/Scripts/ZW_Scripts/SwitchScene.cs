using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{

    public void LoadScene()
    {

        // Load the next scene
        SceneManager.LoadScene("Geospatial");
    }
    public void BackToMap()
    {
        SceneManager.LoadScene("MapScene");
    }
}
