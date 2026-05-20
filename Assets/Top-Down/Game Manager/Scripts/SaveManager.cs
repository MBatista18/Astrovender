using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    public List<ISaveable> saveables;

    [Header("Debugging")]
    public bool showDebugLogs = false;

    private void Awake()
    {
       // Debug.Log("Isn't Null");

        // Singleton pattern to ensure only one instance of SaveManager exists
        if (Instance == null)
        {
           // Debug.Log("Set to Isn't Null");
            Instance = this;
            saveables = new List<ISaveable>();
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate SaveManager if another instance already exists
        }
    }

    void OnEnable()
    {
        if (GameManager.Instance != null)
            saveables.Add(GameManager.Instance);
    }
    void OnDisable()
    {
        if (GameManager.Instance != null)
            saveables.Remove(GameManager.Instance);
    }

#if UNITY_EDITOR
    // Adds commands to save/load data from the SaveManager component's context menu (right click the component in the inspector)
    [ContextMenu("Save Game")]
    private void SaveGame() => Save();
    [ContextMenu("Load Data")]
    private void LoadGame() => Load();
    [ContextMenu("Delete Data")]
    private void DeleteGame() => Delete();
#endif

    public void Save(Object source = null)
    {
        if (showDebugLogs)
        {
            if (source == null)
                Debug.Log("Saving new data.");
            else
                Debug.Log($"Saving new data due to " + source.name);
        }
        
        SaveData newData = new SaveData();

        foreach (var saveable in saveables)
        {
            saveable.SaveData(newData);
        }
        SaveSystem.SaveToFile(newData);
    }

    public void Load(Object source = null)
    {
        if (showDebugLogs)
        {
            if (source == null)
                Debug.Log("Loading data.");
            else
                Debug.Log($"Loading data due to " + source.name);
        }

        SaveData data = SaveSystem.LoadFromFile();

        foreach (var saveable in saveables)
        {
            saveable.LoadData(data);
        }
    }

    public void Delete(Object source = null)
    {
        if (showDebugLogs)
        {
            if (source == null)
                Debug.Log("Deleting data.");
            else
                Debug.Log($"Deleting data due to " + source.name);
        }

        SaveSystem.DeleteData();
    }

    public bool SaveFileExists() => SaveSystem.SaveFileExists();
}
