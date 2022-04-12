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
    public Dropdown shadowsDropdown;
    public Dropdown framerateDropdown;
    public Toggle vsyncToggle;

    public Toggle bloomToggle;
    public Toggle motionBlurToggle;

    public Slider brightnessSlider;
    public Text brightnessLabel;

    [Header("Audio")]
    public Slider[] volumeSliders;
    public Text[] volumeLabels;

    [Header("Controls")]
    public Slider sensitivitySlider;
    public Text sensitivityLabel;

    [Header("Gameplay")]
    public Toggle subtitlesToggle;
    public Toggle crosshairToggle;

    [SerializeField] private SettingsManager settingsManager;

    private bool hasPopulated = false;

    private Resolution[] _resolutions;

    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        StartCoroutine(AwakeSFXCoroutine());
    }

    private void Start()
    {
        OpenMenu(0);
    }

    private IEnumerator AwakeSFXCoroutine()
    {
        _source.volume = 0f;
        yield return new WaitForSeconds(0.25f);
        _source.volume = 1f;
    }

    private void PopulateVideo()
    {
        _resolutions = Screen.resolutions;

        List<string> resolutionsOptions = new List<string>();
        foreach (Resolution r in _resolutions)
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

        shadowsDropdown.value = settingsManager.settings.video.shadowsIndex;

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

        brightnessSlider.value = settingsManager.settings.video.brightness;
        brightnessLabel.text = "Brightness: " + (int)(settingsManager.settings.video.brightness * 100f) + "%";
    }

    private void PopulateAudio()
    {
        volumeSliders[0].value = settingsManager.settings.audio.masterVolume;
        volumeSliders[1].value = settingsManager.settings.audio.sfxVolume;
        volumeSliders[2].value = settingsManager.settings.audio.voiceVolume;
        volumeSliders[3].value = settingsManager.settings.audio.musicVolume;

        UpdateVolumeLabel(0);
        UpdateVolumeLabel(1);
        UpdateVolumeLabel(2);
        UpdateVolumeLabel(3);
    }

    private void PopulateControls()
    {
        sensitivitySlider.value = settingsManager.settings.controls.sensitivity;
    }

    private void PopulateGameplay()
    {
        subtitlesToggle.isOn = settingsManager.settings.gameplay.subtitles;
        crosshairToggle.isOn = settingsManager.settings.gameplay.showCrosshair;

        hasPopulated = true;
    }

    public void Populate()
    {
        PopulateVideo();
        PopulateAudio();
        PopulateControls();
        PopulateGameplay();
    }

    public void Open()
    {
        if (!hasPopulated) Populate();
        settingsMenu.SetActive(true);
    }

    public void Close() => settingsMenu.SetActive(false);

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

    public void UpdateSensitivityLabel() => sensitivityLabel.text = "Sensitivity: " + (int)(sensitivitySlider.value * 100f) + "%";

    public void UpdateBrightness()
    {
        float brightness = brightnessSlider.value;

        brightnessLabel.text = "Brightness: " + (int)(brightness * 100f) + "%";
        settingsManager.UpdateBrightness(brightness);
    }

    public void ApplySettings()
    {
        int currentResolutionIndex = resolutionsDropdown.value;
        string currentResolution = resolutionsDropdown.options[currentResolutionIndex].text;

        int width = int.Parse(currentResolution.Split('x')[0]);
        int height = int.Parse(currentResolution.Split('x')[1]);

        settingsManager.SetVideoSettings(new VideoSettings(width, height, fullscreenToggle.isOn, qualityDropdown.value, shadowsDropdown.value, int.Parse(framerateDropdown.options[framerateDropdown.value].text), vsyncToggle.isOn, bloomToggle.isOn, motionBlurToggle.isOn, brightnessSlider.value));
        settingsManager.SetAudioSettings(new AudioSettings(volumeSliders[0].value, volumeSliders[1].value, volumeSliders[2].value, volumeSliders[3].value));
        settingsManager.SetControlsSettings(new ControlsSettings(sensitivitySlider.value));
        settingsManager.SetGameplaySettings(new GameplaySettings(subtitlesToggle.isOn, crosshairToggle.isOn));

        settingsManager.ApplySettings();
    }

    public void Revert() => settingsManager.Revert();
    public void ResetProgress() => settingsManager.ResetProgress();
}
