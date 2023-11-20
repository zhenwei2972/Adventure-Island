using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchButtonManager : MonoBehaviour
{
    public GameObject Searchbar;
    private bool searchbarVisible = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ToggleSearchBar()
    {
        searchbarVisible = !searchbarVisible;
        Searchbar.SetActive(searchbarVisible);
        
    }
}
