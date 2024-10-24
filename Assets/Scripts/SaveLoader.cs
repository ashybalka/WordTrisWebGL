using System;
using System.IO;
using UnityEngine;

public class SaveLoader : MonoBehaviour
{
    [Serializable]
    public class SaveData
    {
        public string Language;
        public int Blocks;
    }

    public static void LoadFromJson()
    {
        string savePath = (Application.persistentDataPath + "/Savefile.Json");

        if (File.Exists(savePath))
        {
            SaveData savedata = JsonUtility.FromJson<SaveData>(File.ReadAllText(savePath));
            
            SaveResources.Language = savedata.Language;
            SaveResources.Blocks = savedata.Blocks;

            Debug.Log("Successfully loaded.");
        }
        else
        {
            Debug.Log("Using default.");
        }
    }

    public static void SaveToJson()
    {
        string savePath = (Application.persistentDataPath + "/Savefile.Json");

        SaveData data = new()
        {
            Language = SaveResources.Language,
            Blocks = SaveResources.Blocks
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        Debug.Log("Data saved to: " + savePath);
    }

    [Serializable]
    public static class SaveResources
    {
        public static string Language;
        public static int Blocks = 0;
    }
}
 