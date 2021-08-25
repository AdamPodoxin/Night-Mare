using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    public Animator fadeAnim;

    [Space]

    public GameObject crouchText;

    [Space]

    public InfoBox momInfoBox;

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

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        fadeAnim.Play("Fade_In");
    }

    private void DoChecks()
    {
        if (interactedWithBoy && interactedWithGirl && gaveGirlBear)
        {
            momInfoBox.UpdateText
                (momInfoBox.hasRead
                ? "You should get to bed now too."
                : "Honey, thank you so much for these flowers! They smell lovely. You should get to bed now too.");

            momInfoBox.Close();
            momInfoBox.infoBoxI.gameObject.SetActive(true);
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
        girlInfoBox.infoBoxI.gameObject.SetActive(true);
        girlInfoBox.Close();

        DoChecks();
    }
}
