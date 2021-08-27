using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroAnimationCamera : MonoBehaviour
{
    public GameObject player;
    public Transform playerCam;

    [Space]

    public GameObject momInfoBox;
    public Animator blinkAnim;
    public GameObject loadingText;

    [Space]

    public Vector3 sleepPos = new Vector3(-14.25f, 1f, 0.5f);
    public Vector3 sleepEulerAngles = new Vector3(0f, 180f, 0f);

    public float lerpSpeed = 0.5f;

    private Animator anim;
    private bool isLerpingToSleep = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.enabled = false;

        player.SetActive(false);
        momInfoBox.SetActive(false);

        transform.position = playerCam.position - Vector3.up;
        transform.eulerAngles = playerCam.eulerAngles;

        isLerpingToSleep = true;
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

    private void GoToNightmare()
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

        loadingText.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("2_Game", LoadSceneMode.Single);
    }
}
