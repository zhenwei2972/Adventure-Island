using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Vector3 ScreenToWorldOffset(Camera camera, Vector3 screenPos) 
    {
        // x, y are the screen postions. z is the depth
        // Position returned in front of the camera position by the offset value (100f default)
        screenPos.z = camera.nearClipPlane + 100f; 
        return camera.ScreenToWorldPoint(screenPos);
    }

    public static Action<string> OnDebugMessage;
}
