using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour, ISaveable
{
    public static GameManager Instance;

    public int CurrentDay { get; private set; } = 0;
    public int CurrentCoins { get; private set; } = 0;
    public int CurrentGems { get; private set; } = 0;

    public int collectedCoins;
    public int collectedGems;

    bool progressSuccessful;
    public bool GetProgressSuccessful() { return progressSuccessful; }

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

    void Start()
    {
        StartDay(); // this should be removed for the final build, otherwise everytime the player starts the game and this object is created, it'll start a new day
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

        progressSuccessful = success;

        //StartDay();
    }

    public void StartDay()
    {
        IncrementDay();
        Debug.Log("A new day has started. Current Day: " + CurrentDay);
        
        collectedCoins = 0;
        collectedGems = 0;

        SaveManager.Instance.Save(this);
    }

    public void IncrementDay(int value = 1)
    {
        CurrentDay += value;
    }

    public void IncrementCoins(int value = 1)
    {
        collectedCoins += value;
        AssetCall.instance.HUDText.RefreshUI();
    }

    public void IncrementGems(int value = 1)
    {
        collectedGems += value;
        AssetCall.instance.HUDText.RefreshUI();
    }

    public void SaveData(SaveData saveData)
    {
        Debug.Log("Saving GameManager data");
        saveData.gameData = new GameData(this);
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
    }
}
