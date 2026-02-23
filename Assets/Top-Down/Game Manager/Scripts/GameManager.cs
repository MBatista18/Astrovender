using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour, ISaveable
{
    public static GameManager Instance;

    [SerializeField] GameObject player;

    SetHUDText hudText;

    public int CurrentDay { get; private set; } = 0;
    public int CurrentCoins { get; private set; } = 0;
    public int CurrentGems { get; private set; } = 0;

    public int collectedCoins;
    public int collectedGems;

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
        /* if (player != null)
        {
            playerDebug = player.GetComponent<PlayerStatusTest>();
            playerDebug.OnPlayerStatusUpdate += Progress;
        }*/
    }

    void OnEnable()
    {
        Debug.Log(SaveManager.Instance);
        Debug.Log(SaveManager.Instance.saveables);

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

    public void Progress(bool success)
    {
        if (success)
        {
            CurrentGems += collectedGems;
            CurrentCoins += collectedCoins;
        }
        else
        {
            Debug.Log("Day was failed. Progress lost");
        }
        
        //StartDay();
    }

    public void StartDay()
    {
        IncrementDay();
        Debug.Log("A new day has started. Current Day: " + CurrentDay);
        
        collectedCoins = 0;
        collectedGems = 0;
        RefreshUI();

        SaveManager.Instance.Save(this);
    }

    public void IncrementDay(int value = 1)
    {
        CurrentDay += value;
        RefreshUI();
    }

    public void IncrementCoins(int value = 1)
    {
        collectedCoins += value;
        RefreshUI();
    }

    public void IncrementGems(int value = 1)
    {
        collectedGems += value;
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
            CurrentCoins = data.coins;
            CurrentGems = data.gems;
        }

        RefreshUI();
    }

    void RefreshUI()
    {
        if (hudText == null) { hudText = FindFirstObjectByType<SetHUDText>(); }

        hudText?.SetDayText(CurrentDay);
        hudText?.SetCoinText(collectedCoins);
        hudText?.SetGemText(collectedGems);
    }
}
