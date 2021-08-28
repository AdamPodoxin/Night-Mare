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

    [Space]
    [Header("Audio")]
    public Slider[] volumeSliders;
    public Text[] volumeLabels;

    private bool hasPopulated = false;
    private int menuIndex;

    [SerializeField] private SettingsManager settingsManager;

    private void PopulateSettings()
    {
        //TEMP

        hasPopulated = true;
    }

    public void Open()
    {
        if (!hasPopulated) PopulateSettings();
        settingsMenu.SetActive(true);
    }

    public void Close()
    {
        settingsMenu.SetActive(false);
    }

    public void OpenMenu(int menuIndex)
    {
        this.menuIndex = menuIndex;

        headerButtons[menuIndex].GetComponent<Image>().color = headerButtons[menuIndex].colors.selectedColor;
        for (int i = 0; i < menus.Length; i++)
        {
            headerButtons[i].enabled = i != menuIndex;
            menus[i].SetActive(i == menuIndex);
        }
    }

    public void UpdateVolumeLabel(int index)
    {
        switch (index)
        {
            case 0:
                volumeLabels[index].text = "Master: " + (int)(volumeSliders[index].value * 100f) + "%";
                break;
            case 1:
                volumeLabels[index].text = "Sounds: " + (int)(volumeSliders[index].value * 100f) + "%";
                break;
            case 2:
                volumeLabels[index].text = "Voice: " + (int)(volumeSliders[index].value * 100f) + "%";
                break;
            case 3:
                volumeLabels[index].text = "Music: " + (int)(volumeSliders[index].value * 100f) + "%";
                break;
        }
    }

    public void ApplySettings()
    {
        settingsManager.SetAudioSettings(new AudioSettings(volumeSliders[0].value, volumeSliders[1].value, volumeSliders[2].value, volumeSliders[3].value));

        settingsManager.ApplySettings();
    }
}
