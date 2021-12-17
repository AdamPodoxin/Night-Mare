using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChristmasInventory : MonoBehaviour
{
    private int giftsCollected;
    public int GiftsCollected
    {
        get { return giftsCollected; }
        set
        {
            giftsCollected = value;
            Notification.instance.DisplayNotification($"{value} / 3 Gifts Collected");

            PlayerPrefs.SetInt("Gifts", value);
        }
    }

    private void Start()
    {
        PlayerPrefs.SetInt("Gifts", 0);
    }
}
