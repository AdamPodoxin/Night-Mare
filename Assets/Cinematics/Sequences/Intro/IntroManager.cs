using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    public GameObject crouchText;

    [Space]

    public InfoBox momInfoBox;
    public string momTextNew;

    [Space]

    public GameObject bearPedestal;
    public InfoBox girlInfoBox;

    [Space]

    public GameObject sleepCollider;

    private bool gaveGirlBear = false;

    private bool interactedWithBoy = false;
    private bool interactedWithGirl = false;

    private int infoBoxesInteracted;
    public int InfoBoxesInteracted
    {
        get { return infoBoxesInteracted; }
        set
        {
            infoBoxesInteracted = value;

            DoChecks();
        }
    }

    private void DoChecks()
    {
        if (interactedWithBoy && interactedWithGirl && gaveGirlBear)
        {
            momInfoBox.UpdateText(momTextNew);
        }

        if (infoBoxesInteracted == 3)
        {
            sleepCollider.SetActive(true);
        }
    }

    public void InteractWithBoy()
    {
        interactedWithBoy = true;
    }

    public void InteractWithGirl()
    {
        interactedWithGirl = true;

        bearPedestal.SetActive(true);
        StartCoroutine(ShowTextCoroutine());
    }

    private IEnumerator ShowTextCoroutine()
    {
        yield return new WaitForSeconds(3f);
        crouchText.SetActive(true);
    }

    public void GaveGirlBear()
    {
        gaveGirlBear = true;
        girlInfoBox.UpdateText("Yay, thank you Daddy!");

        DoChecks();
    }
}
