using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsMenu;

    private bool hasLoaded = false;

    private void LoadSettings()
    {
        //TEMP

        hasLoaded = true;
    }

    public void Open()
    {
        if (!hasLoaded) LoadSettings();
        settingsMenu.SetActive(true);
    }

    public void Close()
    {
        settingsMenu.SetActive(false);
    }

    public void ApplySettings()
    {
        //TEMP
        print("Apply settings");
    }
}
