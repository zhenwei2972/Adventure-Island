using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMenus : MonoBehaviour
{
    public GameObject topmenu;
    public GameObject bottommenu;
    // Start is called before the first frame update
    void Start()
    {
        topmenu.SetActive(false);
        bottommenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
