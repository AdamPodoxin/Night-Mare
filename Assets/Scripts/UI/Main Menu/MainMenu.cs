using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject loadingObject;

    [Space]

    public AudioSource globalSFX;
    public AudioClip playClip;

    private DataManager dataManager;

    private void Start()
    {
        dataManager = FindObjectOfType<DataManager>();
    }

    private IEnumerator PlayCoroutine()
    {
        loadingObject.SetActive(true);
        globalSFX.PlayOneShot(playClip);

        yield return new WaitForSeconds(0.1f);

        if (dataManager.GetProgress().hasReadPrompts)
            SceneManager.LoadSceneAsync("2_Game", LoadSceneMode.Single);
        else
            SceneManager.LoadSceneAsync("1_Intro", LoadSceneMode.Single);
    }

    public void Play()
    {
        StartCoroutine(PlayCoroutine());
    }

    public void Credits()
    {
        SceneManager.LoadScene("4_Credits", LoadSceneMode.Single);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
