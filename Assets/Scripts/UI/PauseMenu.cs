using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool isPaused = false;
    public KeyCode pauseKey = KeyCode.Escape;

    [Space]

    public GameObject pauseMenu;

    [SerializeField] private bool canTogglePause = true;
    public bool CanTogglePause { get { return canTogglePause; } set { canTogglePause = value; } }

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
}
