using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChristmasSoul : MonoBehaviour
{
    public GameObject gift;

    private void Start()
    {
        int giftsCollected = PlayerPrefs.GetInt("Gifts");

        if (giftsCollected >= 3)
        {
            gift.SetActive(true);
        }
    }
}
