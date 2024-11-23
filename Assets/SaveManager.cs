using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Playables;

public class SaveManager : MonoBehaviour
{
    private string saveFilePath;

    void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "flipcard_save.json");
    }

    // Save game data
    public void SaveGame(GameData gameData)
    {
        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game Saved to: " + saveFilePath);
    }

    // Load game data
    public GameData LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            GameData loadedData = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Game Loaded from: " + saveFilePath);
            return loadedData;
        }
        else
        {
            Debug.LogWarning("Save file not found!");
            return null;
        }
    }
}
