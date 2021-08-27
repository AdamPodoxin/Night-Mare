using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsMenu;

    [Space]

    public Button[] headerButtons;
    public GameObject[] menus;

    private bool hasLoaded = false;
    private int menuIndex;

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

    public void OpenMenu(int menuIndex)
    {
        this.menuIndex = menuIndex;

        //headerButtons[menuIndex].Select();
        headerButtons[menuIndex].GetComponent<Image>().color = headerButtons[menuIndex].colors.selectedColor;
        for (int i = 0; i < menus.Length; i++)
        {
            headerButtons[i].enabled = i != menuIndex;
            menus[i].SetActive(i == menuIndex);
        }
    }

    public void ApplySettings()
    {
        //TEMP
        print("Apply settings");
    }
}
