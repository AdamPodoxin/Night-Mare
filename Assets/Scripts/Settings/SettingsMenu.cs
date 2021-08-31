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
    [Header("Video")]
    public Dropdown resolutionsDropdown;
    public Toggle fullscreenToggle;
    public Dropdown qualityDropdown;
    public Dropdown framerateDropdown;
    public Toggle vsyncToggle;

    public Toggle bloomToggle;
    public Toggle motionBlurToggle;

    [Header("Audio")]
    public Slider[] volumeSliders;
    public Text[] volumeLabels;

    [Header("Controls")]
    public Slider sensitivitySlider;
    public Text sensitivityLabel;

    [Header("Gameplay")]
    public Toggle subtitlesToggle;

    [SerializeField] private SettingsManager settingsManager;

    private bool hasPopulated = false;

    private Resolution[] resolutions;

    private void Start()
    {
        OpenMenu(0);
    }

    public void Populate()
    {
        //Video
        resolutions = Screen.resolutions;

        List<string> resolutionsOptions = new List<string>();
        foreach (Resolution r in resolutions)
        {
            string option = r.width + "x" + r.height;

            if (resolutionsOptions.Contains(option))
            {
                continue;
            }

            resolutionsOptions.Add(option);
        }

        if (!hasPopulated)
        {
            resolutionsDropdown.ClearOptions();
            resolutionsDropdown.AddOptions(resolutionsOptions);
        }

        string currentResolution = settingsManager.settings.video.width + "x" + settingsManager.settings.video.height;
        for (int i = 0; i < resolutionsOptions.Count; i++)
        {
            if (resolutionsOptions[i].Equals(currentResolution))
            {
                resolutionsDropdown.value = i;
                break;
            }
        }

        fullscreenToggle.isOn = settingsManager.settings.video.fullscreen;

        qualityDropdown.value = settingsManager.settings.video.qualityIndex;

        for (int i = 0; i < framerateDropdown.options.Count; i++)
        {
            if (framerateDropdown.options[i].text.Equals(settingsManager.settings.video.framerate.ToString()))
            {
                framerateDropdown.value = i;
                break;
            }
        }

        vsyncToggle.isOn = settingsManager.settings.video.vsync;

        bloomToggle.isOn = settingsManager.settings.video.bloom;
        motionBlurToggle.isOn = settingsManager.settings.video.motionBlur;

        //Audio
        volumeSliders[0].value = settingsManager.settings.audio.masterVolume;
        volumeSliders[1].value = settingsManager.settings.audio.sfxVolume;
        volumeSliders[2].value = settingsManager.settings.audio.voiceVolume;
        volumeSliders[3].value = settingsManager.settings.audio.musicVolume;

        UpdateVolumeLabel(0);
        UpdateVolumeLabel(1);
        UpdateVolumeLabel(2);
        UpdateVolumeLabel(3);

        //Controls
        sensitivitySlider.value = settingsManager.settings.controls.sensitivity;

        //Gameplay
        subtitlesToggle.isOn = settingsManager.settings.gameplay.subtitles;

        hasPopulated = true;
    }

    public void Open()
    {
        if (!hasPopulated) Populate();
        settingsMenu.SetActive(true);
    }

    public void Close()
    {
        settingsMenu.SetActive(false);
    }

    public void OpenMenu(int menuIndex)
    {
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

    public void UpdateSensitivityLabel()
    {
        sensitivityLabel.text = "Sensitivity: " + (int)(sensitivitySlider.value * 100f) + "%";
    }

    public void ApplySettings()
    {
        int currentResolutionIndex = resolutionsDropdown.value;
        string currentResolution = resolutionsDropdown.options[currentResolutionIndex].text;

        int width = int.Parse(currentResolution.Split('x')[0]);
        int height = int.Parse(currentResolution.Split('x')[1]);


        settingsManager.SetVideoSettings(new VideoSettings(width, height, fullscreenToggle.isOn, qualityDropdown.value, int.Parse(framerateDropdown.options[framerateDropdown.value].text), vsyncToggle.isOn, bloomToggle.isOn, motionBlurToggle.isOn));
        settingsManager.SetAudioSettings(new AudioSettings(volumeSliders[0].value, volumeSliders[1].value, volumeSliders[2].value, volumeSliders[3].value));
        settingsManager.SetControlsSettings(new ControlsSettings(sensitivitySlider.value));
        settingsManager.SetGameplaySettings(new GameplaySettings(subtitlesToggle.isOn));

        settingsManager.ApplySettings();
    }

    public void Revert()
    {
        settingsManager.Revert();
    }

    public void ResetProgress()
    {
        settingsManager.ResetProgress();
    }
}
