using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour, ISaveable
{
    public static GameManager Instance;

    [SerializeField] GameObject player;
    [SerializeField] TextMeshProUGUI dayText;
    [SerializeField] TextMeshProUGUI resourceCountText;

    public int CurrentDay { get; private set; } = 0;
    public int Resources { get; private set; } = 0;

    public int currentResources;
    private PlayerStatusTest playerDebug;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate GameManager if another instance already exists
        }

        // Debug Purposes
        if (player != null)
        {
            playerDebug = player.GetComponent<PlayerStatusTest>();
            playerDebug.OnPlayerStatusUpdate += Progress;
        }
    }

    void OnEnable()
    {
        SaveManager.Instance.saveables.Add(this);
    }
    void OnDisable()
    {
        SaveManager.Instance.saveables.Add(this);
    }

    void Start()
    {
        StartDay();
    }

    void Progress(bool success)
    {
        if (success)
        {
            Resources += currentResources;
        }
        else
        {
            Debug.Log("Day was failed. Progress lost");
        }
        
        StartDay();
    }

    void StartDay()
    {
        IncrementDay();
        IncrementResources(0);
        Debug.Log("A new day has started. Current Day: " + CurrentDay);
        currentResources = 0;
        SaveManager.Instance.Save(this);
    }

    public void IncrementDay(int value = 1)
    {
        CurrentDay += value;
        RefreshUI();
    }
    public void IncrementResources(int value = 1)
    {
        currentResources += value;
        RefreshUI();
    }

    public void SaveData(SaveData saveData)
    {
        Debug.Log("Saving GameManager data");
        saveData.gameData = new GameData(this);

        RefreshUI();
    }

    public void LoadData(SaveData saveData)
    {
        if (saveData.gameData == null) return;

        Debug.Log("Loading GameManager data");
        GameData data = saveData.gameData;
        if (data != null)
        {
            CurrentDay = data.day;
            Resources = data.resources;
        }

        RefreshUI();
    }

    void RefreshUI()
    {
        resourceCountText.text = "# Resources: " + (Resources + currentResources);
        dayText.text = "Day: " + CurrentDay;
    }
}
