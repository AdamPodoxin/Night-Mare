using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    public Animator fadeAnim;

    [Space]

    public GameObject crouchTextTrigger;

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

    private bool _hasChangedMom = false;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        fadeAnim.Play("Fade_In");

        FindObjectOfType<PauseMenu>().CanTogglePause = false;
    }

    private void DoChecks()
    {
        if (infoBoxesInteracted == 3 && gaveGirlBear)
        {
            sleepCollider.SetActive(true);
        }

        if (interactedWithBoy && interactedWithGirl && gaveGirlBear && !_hasChangedMom)
        {
            momInfoBox.UpdateText
                (momInfoBox.hasRead
                ? "You should get to bed now too."
                : "Honey, thank you so much for these flowers! They smell lovely. You should get to bed now too.");

            momInfoBox.Close();
            momInfoBox.infoBoxI.gameObject.SetActive(true);

            _hasChangedMom = true;
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
        crouchTextTrigger.SetActive(true);
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
