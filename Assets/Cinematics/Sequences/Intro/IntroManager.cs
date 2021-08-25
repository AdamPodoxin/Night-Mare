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

    public GameObject sleepCollider;

    private bool interactedWithBoy = false;
    private bool interactedWithGirl = false;

    private int infoBoxesInteracted;
    public int InfoBoxesInteracted
    {
        get { return infoBoxesInteracted; }
        set
        {
            infoBoxesInteracted = value;

            if (interactedWithBoy && interactedWithGirl)
            {
                momInfoBox.UpdateText(momTextNew);
            }

            if (infoBoxesInteracted == 3)
            {
                sleepCollider.SetActive(true);
            }
        }
    }

    public void InteractWithBoy()
    {
        interactedWithBoy = true;
    }

    public void InteractWithGirl()
    {
        interactedWithGirl = true;
        StartCoroutine(ShowTextCoroutine());
    }

    private IEnumerator ShowTextCoroutine()
    {
        yield return new WaitForSeconds(3f);
        crouchText.SetActive(true);
    }
}
