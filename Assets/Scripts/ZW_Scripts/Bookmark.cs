using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bookmark
{

    [SerializeField]
    public float Latitude { get; set; }
    [SerializeField]
    public float Longitude { get; set; }
    [SerializeField]
    public string Name { get; set; }

    [SerializeField]
    public Bookmark(float latitude, float longitude, string name)
    {
        Latitude = latitude;
        Longitude = longitude;
        Name = name;
    }


    public override string ToString()
    {
        return $"Location: {Name} ({Latitude}, {Longitude})";
    }
}



