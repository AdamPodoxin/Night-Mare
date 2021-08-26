using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndHouse : MonoBehaviour
{
    public Animator bedroomDoorAnim;
    public Animator basementDoorAnim;
    public Animator basementDoorAudio;

    [Space]

    public Animator fade;

    [Space]

    public GameObject soulsCinematic;
    public GameObject pictureCinematic;

    public void OpenBedroom()
    {
        bedroomDoorAnim.Play("Open");
    }

    public void OpenBasement()
    {
        basementDoorAnim.enabled = true;
        basementDoorAudio.enabled = true;
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        fade.Play("Fade_Out_Slow");

        yield return new WaitForSeconds(1.5f);

        soulsCinematic.SetActive(false);
        pictureCinematic.SetActive(true);

        fade.Play("Fade_In_Slow");
    }
}
