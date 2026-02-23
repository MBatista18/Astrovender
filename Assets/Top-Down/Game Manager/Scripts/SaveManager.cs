using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    public List<ISaveable> saveables;

    [Header("Debugging")]
    public bool showDebugLogs = false;
    public KeyCode saveData = KeyCode.I;
    public KeyCode loadSave = KeyCode.O;
    public KeyCode deleteSave = KeyCode.P;

    private void Awake()
    {
        Debug.Log("Isn't Null");

        // Singleton pattern to ensure only one instance of SaveManager exists
        if (Instance == null)
        {
            Debug.Log("Set to Isn't Null");
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
            SaveManager.Instance.saveables.Add(GameManager.Instance);
    }
    void OnDisable()
    {
        if (GameManager.Instance != null)
            SaveManager.Instance.saveables.Add(GameManager.Instance);
    }

    private void Update()
    {
       /* if (Input.GetKeyDown(saveData))
        {
            Save();
        }
        else if (Input.GetKeyDown(loadSave))
        {
            Load();
        }*/
    }

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
}
