using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private GameProgress gameProgress = null;
    private Settings settings;

    private void SaveProgress()
    {
        SaveSystem.SaveData(SaveSystem.SaveType.Progress, gameProgress);
    }

    public GameProgress GetProgress()
    {
        gameProgress = SaveSystem.LoadData<GameProgress>(SaveSystem.SaveType.Progress);
        if (gameProgress == null) gameProgress = new GameProgress();

        return gameProgress;
    }

    public void SetHasReadPrompts(bool hasReadPrompts)
    {
        gameProgress.hasReadPrompts = hasReadPrompts;
        SaveProgress();
    }

    public void SetSeed(string seed)
    {
        gameProgress.seed = seed;
        SaveProgress();
    }

    private void SaveSettings()
    {
        SaveSystem.SaveData(SaveSystem.SaveType.Settings, settings);
    }

    public Settings GetSettings()
    {
        settings = SaveSystem.LoadData<Settings>(SaveSystem.SaveType.Settings);
        if (settings == null) settings = Settings.Default();

        return settings;
    }

    public void SetSettings(Settings settings)
    {
        this.settings = settings;
        SaveSettings();
    }
}
