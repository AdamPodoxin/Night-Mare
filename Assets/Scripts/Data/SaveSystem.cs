using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    //Change these to your preference
    public enum SaveType { Progress, Settings }

    public static void SaveData(SaveType saveType, object saveData)
    {
        try
        {
            string jsonStr = JsonUtility.ToJson(saveData);
            File.WriteAllText(SaveTypeToPath(saveType), jsonStr);
        }
        catch
        {
            Debug.LogError($"There was a problem saving the {saveType} file");
        }
    }

    public static T LoadData<T>(SaveType saveType)
    {
        try
        {
            string path = SaveTypeToPath(saveType);

            if (File.Exists(path))
            {
                string loadStr = File.ReadAllText(path);

                if (string.IsNullOrEmpty(loadStr))
                {
                    Debug.LogWarning($"{saveType} file does not exist");
                    return default;
                }

                T loadData = JsonUtility.FromJson<T>(loadStr);

                return loadData;
            }
            else
            {
                Debug.LogWarning($"{saveType} file does not exist");
                return default;
            }
        }
        catch
        {
            Debug.LogError($"There was a problem loading the {saveType} file");
            return default;
        }
    }

    public static string SaveTypeToPath(SaveType saveType)
    {
        string fileName = saveType.ToString();
        string savePath = Application.persistentDataPath + "/" + fileName + ".json";

        return savePath;
    }
}
