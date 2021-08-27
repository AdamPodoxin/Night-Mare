using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject loadingObject;

    [Space]

    public AudioSource globalSFX;
    public AudioSource globalMusic;
    public AudioClip playClip;

    private DataManager dataManager;

    private bool _isLoading = false;

    private void Start()
    {
        dataManager = FindObjectOfType<DataManager>();
    }

    private void Update()
    {
        if (_isLoading)
        {
            globalMusic.volume -= Time.deltaTime * 2f;
        }
    }

    private IEnumerator PlayCoroutine()
    {
        loadingObject.SetActive(true);
        globalSFX.PlayOneShot(playClip);
        _isLoading = true;

        yield return new WaitForSeconds(0.65f);

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
