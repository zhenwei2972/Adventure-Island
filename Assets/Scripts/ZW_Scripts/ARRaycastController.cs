using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARRaycastController : MonoBehaviour
{
    private Vector2 touchPosition = default;
    Camera m_MainCamera;
    public ClickCoupon couponHandler;
    private void Start()
    {
        m_MainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;

            //Checking to see if the position of the touch is over a UI object in case of UI overlay on screen.


            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = m_MainCamera.ScreenPointToRay(touchPosition);
                RaycastHit hitObject;

                if (Physics.Raycast(ray, out hitObject))
                {

                    //Do whatever you want to do with the hitObject, which in this case would be your, well, case. Identify it either through name or tag, for instance below.
                    if (hitObject.transform.CompareTag("DrinkCoupon"))
                    {

                        Destroy(hitObject.transform.gameObject);
                        couponHandler.ShowCoupon();
                        //Do something with the case
                    }
                }
            }

        }
    }
}
        