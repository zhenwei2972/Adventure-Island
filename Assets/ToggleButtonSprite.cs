using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ToggleButtonSprite : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite playerActiveSprite;
    public Sprite playerUnActiveSprite;
    public Sprite droneActiveSprite;
    public Sprite droneUnActiveSprite;
    public Sprite ActiveMap;
    public Sprite UnActiveMap;
    public bool mapState = false;
    public Button PlayerBtn;
    public Button DroneBtn;
    public Button MapBtn;
    void Start()
    {
        PlayerBtn.onClick.AddListener(SetPlayerBtnActiveSprite);
        DroneBtn.onClick.AddListener(SetDroneBtnActiveSprite);
        MapBtn.onClick.AddListener(ToggleSpriteForMap);
    }
    void SetPlayerBtnActiveSprite()
    {
        PlayerBtn.GetComponent<Image>().sprite = playerActiveSprite;
        DroneBtn.GetComponent<Image>().sprite = droneUnActiveSprite;
        
    }
    void SetDroneBtnActiveSprite()
    {
        PlayerBtn.GetComponent<Image>().sprite = playerUnActiveSprite;
        DroneBtn.GetComponent<Image>().sprite = droneActiveSprite;

    }
    void ToggleSpriteForMap()
    {
        if (mapState)
        {
            MapBtn.GetComponent<Image>().sprite = ActiveMap;
        }
        else
        {
            MapBtn.GetComponent<Image>().sprite = UnActiveMap;
        }
        mapState = !mapState;
    }


}
