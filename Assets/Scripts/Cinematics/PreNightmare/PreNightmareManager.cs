using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreNightmareManager : MonoBehaviour
{
    public GameObject bedInfoBox;

    [Space]

    public Animator blinkAnim;

    private int infoBoxesInteracted;
    public int InfoBoxesInteracted
    {
        get { return infoBoxesInteracted; }
        set
        {
            infoBoxesInteracted = value;
            if (infoBoxesInteracted >= 3)
            {
                bedInfoBox.SetActive(true);
            }
        }
    }

    private void Start()
    {
        blinkAnim.Play("Blink_Fast_Open");
    }
}
