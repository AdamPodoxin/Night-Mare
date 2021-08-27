using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    public string choice;

    [Space]

    public Animator blueFlash;
    public Animator greenFlash;

    [Space]

    public GameObject choiceScene;
    public GameObject houseScene;

    [Space]

    public Animator camAnim;
    public GameObject dadSoul;
    public GameObject[] familySouls;

    [Space]

    public AudioSource sfxSource;
    public AudioClip igniteClip;

    [Space]

    public MeshRenderer pictureRenderer;
    public Material dadDead;
    public Material familyDead;

    public void MakeChoice(string choice)
    {
        StartCoroutine(MakeChoiceCoroutine(choice));
    }

    private IEnumerator MakeChoiceCoroutine(string choice)
    {
        this.choice = choice;

        switch (choice)
        {
            case "L":
                //Sacrifice family
                blueFlash.gameObject.SetActive(true);
                blueFlash.Play("Flash_Out");

                pictureRenderer.sharedMaterial = familyDead;
                break;
            case "R":
                //Sacrifice dad
                greenFlash.gameObject.SetActive(true);
                greenFlash.Play("Flash_Out");

                pictureRenderer.sharedMaterial = dadDead;
                break;
        }

        choiceScene.SetActive(false);
        houseScene.SetActive(true);

        sfxSource.PlayOneShot(igniteClip);

        yield return new WaitForSeconds(1f);

        camAnim.enabled = true;

        switch (choice)
        {
            case "L":
                //Sacrifice family
                foreach (GameObject soul in familySouls)
                {
                    soul.SetActive(true);
                    yield return new WaitForSeconds(0.35f);
                }
                break;
            case "R":
                //Sacrifice dad
                dadSoul.SetActive(true);
                break;
        }
    }
}
