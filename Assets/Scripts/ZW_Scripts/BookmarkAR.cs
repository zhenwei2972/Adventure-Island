using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookmarkAR : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LoadBookmarks();
    }
    void LoadBookmarks()
    {
       /* if (BookmarkSaveSystem.Load() != null)
        {
            List<Bookmark> bookmarkList = JsonConvert.DeserializeObject<List <Bookmark>>(BookmarkSaveSystem.Load());
            foreach (Bookmark currentBookmark in bookmarkList)
            {
                Debug.Log("SAVE FILE READ "+currentBookmark.Name);
            }
        }*/
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
