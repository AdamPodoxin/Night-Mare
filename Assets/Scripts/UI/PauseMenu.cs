using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool isPaused = false;
    public KeyCode pauseKey = KeyCode.Escape;

    [Space]

    public GameObject pauseMenu;
    public GameObject quitMenu;

    [SerializeField] private bool canTogglePause = true;
    public bool CanTogglePause { get { return canTogglePause; } set { canTogglePause = value; } }

    [Space]

    [SerializeField] private ArtifactSpawner artifactSpawner;

    private string quitAction;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
    }

    private void UpdatePauseElements()
    {
        Time.timeScale = isPaused ? 0f : 1f;
        pauseMenu.SetActive(isPaused);

        Cursor.visible = isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void ResetSeed()
    {
        try
        {
            artifactSpawner.ResetSeed();
        }
        catch
        {
            Debug.LogWarning("No ArtifactSpawner in scene");
        }
    }

    public void Pause()
    {
        if (!canTogglePause) return;

        isPaused = true;
        UpdatePauseElements();
    }

    public void UnPause()
    {
        if (!canTogglePause) return;

        isPaused = false;
        UpdatePauseElements();
    }

    public void TogglePause()
    {
        if (isPaused) UnPause();
        else Pause();
    }

    public void OpenQuitMenu(string quitAction)
    {
        this.quitAction = quitAction;
        quitMenu.SetActive(true);
        CanTogglePause = false;
    }

    public void QuitAction()
    {
        if (quitAction.Equals("Menu"))
        {
            GoToMenu();
        }
        else if (quitAction.Equals("Quit"))
        {
            Quit();
        }
    }

    public void GoToMenu()
    {
        ResetSeed();
        Time.timeScale = 1f;
        SceneManager.LoadScene("0_Menu", LoadSceneMode.Single);
    }

    public void Quit()
    {
        ResetSeed();
        Time.timeScale = 1f;
        Application.Quit();
    }
}
