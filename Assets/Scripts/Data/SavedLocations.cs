using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Data
{
    public class SavedLocations : MonoBehaviour
    {
        // x = longitude, y = latitude, z = altitude
        // be sure to swap x, y when copying from Google Maps
        public static readonly Dictionary<string, double3> Attractions = new Dictionary<string, double3>()
        {
            {"Default", new(103.85929848,1.286509773, 11.0259)},
            {"GuocoTower", new (103.84581836, 1.2771762483, 11.0259)},
            {"Merlion", new (103.85451723,1.28693720, 11.0259)},
            {"OlaBeachClub", new(103.8151333,1.2533666080,  11.0259)},
            {"MarinaBaySands", new( 103.85914941,1.284014926, 11.0259)},
            {"FaberPeak", new( 103.8195667,1.2713927178, 11.0259)},
            {"SentosaStation", new( 103.81714077,1.2555976731, 11.0259)},
            {"ArtScienceMuseum", new( 103.85929848,1.286509773, 11.0259)},
            {"FlowerDome", new( 103.86471895,1.2846721032, 11.0259)}
        };

        public static Dictionary<string, double3> AnchoredObjects = new Dictionary<string, double3>()
        {
            {"PlayerShip", new(103.81493707,1.25124203, 7.5364)},
            {"ShipWreck", new(103.85929848,1.286509773, 11.0259)},
        };
    }

    internal enum PresetLocations
    {
        Default,
        GuocoTower,
        Merlion,
        OlaBeachClub,
        MarinaBaySands,
        FaberPeak,
        SentosaStation,
        ArtScienceMuseum,
        FlowerDome
    }
}
