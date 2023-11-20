using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CameraControl;
using Cinemachine;
public class CameraMoveToLocation : MonoBehaviour
{
    [SerializeField] private GameObject _followTarget;
    [SerializeField] private PointsOfInterest poiComponent;
    [SerializeField] private CinemachineVirtualCamera placeVCam;
    public void MoveToSelectedPlace()
    {
        // _followTarget.transform.SetPositionAndRotation(poiComponent.GetSelectedPlaceVector3(), Quaternion.identity);;
        placeVCam.transform.SetPositionAndRotation(poiComponent.GetSelectedPlaceVector3(), Quaternion.identity);
        placeVCam.Follow = poiComponent.GetTargetFollow().transform;
    }

}
