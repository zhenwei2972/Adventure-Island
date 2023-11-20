using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class OrbitCameraOnce : MonoBehaviour
{
    public Transform target; // Transform of the game object to orbit around
    public float orbitDistance = 10.0f; // Distance of the camera from the game object

    private CinemachineFreeLook freelook; // Reference to the Cinemachine Free Look camera
    private bool isOrbiting = false; // Flag to indicate whether the camera is orbiting
    private float orbitAngle = 0.0f; // Current orbit angle
    public float orbitSpeed = 2.0f; // Speed of the camera's orbit
    public float spinSpeed = 1.0f; // Speed of the camera's spin
    private float pitchAngle = 45.0f; // Downward angle of the camera's view
    void Start()
    {
        // Get the Cinemachine Free Look camera component
        freelook = GetComponent<CinemachineFreeLook>();
        OrbitOnce();
    }

    void Update()
    {
        if (isOrbiting)
        {
            transform.RotateAround(target.transform.position, Vector3.up, 20 * Time.deltaTime);
        }

        /*// Check if orbit is complete
        if (orbitAngle >= 2.0f * Mathf.PI)
        {
            isOrbiting = false; // Stop orbiting
        }*/

    }
    public void OrbitOnce()
    {
        // Start orbiting
        isOrbiting = true;
        orbitAngle = 0.0f;
    }
}