using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static string savePath = Application.persistentDataPath + "/saveData.json";

    public static void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Данные сохранены: " + savePath);
    }

    public static SaveData Load()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            Debug.LogWarning("Файл сохранения не найден.");
            return null;
        }
    }

    public static void ClearSave()
    {
        string savePath = Application.persistentDataPath + "/saveData.json";
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Сохраненные данные удалены.");
        }
        else
        {
            Debug.LogWarning("Файл сохранения не найден.");
        }
    }
}