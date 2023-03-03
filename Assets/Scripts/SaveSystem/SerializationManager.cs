using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public static class SerializationManager
{ 
    public static string dir = Application.persistentDataPath + "/saves/";
    public static bool SaveGame(string saveName, object saveData)
    {
        EventManager.onSave?.Invoke();

        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);


        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(dir + saveName + ".save", json);

        return true;
    }

    public static bool LoadGame(string saveName)
    {
        if (!File.Exists(dir + saveName)) return false;

        SaveData temp = new SaveData();

        string data = File.ReadAllText(dir + saveName);
        temp = JsonUtility.FromJson<SaveData>(data);

        SaveData.Current = temp;

        return true;
    }
    public static bool SaveSettings(string saveName, object saveData)
    {
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);


        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(dir + saveName + ".save", json);

        return true;
    }

    public static bool LoadSettings(string saveName)
    {
        if (!File.Exists(dir + saveName)) return false;

        SettingsData temp = new SettingsData();

        string data = File.ReadAllText(dir + saveName);
        temp = JsonUtility.FromJson<SettingsData>(data);

        SettingsData.Current = temp;

        return true;
    }

    public static bool DeleteSave(string saveName)
    {
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        File.Delete(dir + saveName);
        SaveData.Current = new SaveData();
        if (!File.Exists(dir + saveName)) return true;
        else return false;
    }

    public static bool CheckFile(string saveName)
    {
        if (File.Exists(dir + saveName)) return true;
        else return false;
    }

}
