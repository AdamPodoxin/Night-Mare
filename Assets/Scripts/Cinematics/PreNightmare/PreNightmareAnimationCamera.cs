using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreNightmareAnimationCamera : MonoBehaviour
{
    public GameObject player;
    public Transform playerCam;

    [Space]

    public GameObject[] infoBoxes;
    public GameObject bedInfoBox;
    public GameObject bedInteractable;

    [Space]

    public GameObject normalWorld;
    public GameObject nightmareWorld;

    [Space]

    public Animator blinkAnim;

    [Space]

    public Vector3 sleepPos = new Vector3(-14.25f, 1f, 0.5f);
    public Vector3 sleepEulerAngles = new Vector3(0f, 180f, 0f);

    public float lerpSpeed = 0.5f;

    private DataManager dataManager;

    private Animator anim;
    private bool isLerpingToSleep = false;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        dataManager = FindObjectOfType<DataManager>();

        anim = GetComponent<Animator>();
        anim.Play("PreNightmare_Wake");

        GetComponent<AudioSource>().Play();
    }

    private void FixedUpdate()
    {
        if (isLerpingToSleep)
        {
            if ((sleepPos - transform.position).sqrMagnitude <= 0.01f)
            {
                isLerpingToSleep = false;
                GoToNightmare();
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, sleepPos, lerpSpeed * Time.fixedDeltaTime);
                transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, sleepEulerAngles, lerpSpeed * Time.fixedDeltaTime * 2f);
            }
        }
    }

    public void SwitchToPlayer()
    {
        player.SetActive(true);

        bool hasReadPrompts = dataManager.GetGameProgress().hasReadPrompts;
        if (hasReadPrompts)
        {
            bedInteractable.SetActive(true);
        }
        else
        {
            foreach (GameObject g in infoBoxes)
            {
                g.SetActive(true);
            }
        }

        gameObject.SetActive(false);
    }

    public void StartLerp()
    {
        anim.enabled = false;

        transform.position = playerCam.position - Vector3.up;
        transform.eulerAngles = playerCam.eulerAngles;

        player.SetActive(false);
        gameObject.SetActive(true);

        bedInfoBox.SetActive(false);
        foreach (GameObject g in infoBoxes)
        {
            g.SetActive(false);
        }

        isLerpingToSleep = true;
    }

    public void GoToNightmare()
    {
        StartCoroutine(GoToNightmareCoroutine());
    }

    private IEnumerator GoToNightmareCoroutine()
    {
        anim.enabled = true;
        anim.Play("PreNightmare_Sleep");
        yield return new WaitForSeconds(3f);
        blinkAnim.Play("Blink_Slow_Close");

        yield return new WaitForSeconds(1f);

        normalWorld.SetActive(false);
        nightmareWorld.SetActive(true);
    }
}
