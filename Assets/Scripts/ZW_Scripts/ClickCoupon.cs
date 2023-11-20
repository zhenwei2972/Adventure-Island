using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCoupon : MonoBehaviour
{
    public GameObject couponIcon;
    private bool activeStatus = false;
    public void ShowCoupon()
    {
        if (activeStatus == false)
        {
            couponIcon.SetActive(true);
            activeStatus = true;
        }
        else if (activeStatus ==true)
        {
            couponIcon.SetActive(false);
            activeStatus = false;
        }
    }
}
