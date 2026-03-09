using System;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour, ISaveable
{
    public static GameManager Instance;

    [HideInInspector] public int CurrentDay { get; private set; } = 0;
    [HideInInspector] public int CurrentCoins { get; private set; } = 0;
    [HideInInspector] public int CurrentGems { get; private set; } = 0;

    [HideInInspector] public bool PermaCollectedBombs;
    [HideInInspector] public bool PermaCollectedGun;
    [HideInInspector] public bool PermaCollectedShield;

    [HideInInspector] public bool PermaDefeatedBombsBoss;
    [HideInInspector] public bool PermaDefeatedGunsBoss;

    [HideInInspector] public int collectedCoins;
    [HideInInspector] public int collectedGems;

    [HideInInspector] public bool collectedBombs;
    [HideInInspector] public bool collectedGun;
    [HideInInspector] public bool collectedShield;

    [HideInInspector] public bool defeatedBombsBoss;
    [HideInInspector] public bool defeatedGunsBoss;

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

            if (PermaCollectedBombs == false) { PermaCollectedBombs = collectedBombs; }
            if (PermaCollectedGun == false) { PermaCollectedGun = collectedGun; }
            if (PermaCollectedShield == false) { PermaCollectedShield = collectedShield; }

            if (PermaDefeatedBombsBoss == false) { PermaDefeatedBombsBoss = defeatedBombsBoss; }
            if (PermaDefeatedGunsBoss == false) { PermaDefeatedGunsBoss = defeatedGunsBoss; }
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

        collectedBombs = PermaCollectedBombs;
        collectedGun = PermaCollectedGun;
        collectedShield = PermaCollectedShield;

        defeatedBombsBoss = PermaDefeatedBombsBoss;
        defeatedGunsBoss = PermaDefeatedGunsBoss;

        PlayerManager.ResetPlayerValues();
        PlayerManager.playerWorldSpawn = new Vector3(10, 23);
        SaveManager.Instance.Save(this);
    }

    public void IncrementDay(int value = 1)
    {
        CurrentDay += value;
    }

    public void IncrementCoins(int value = 1)
    {
        collectedCoins += value;
        AssetCall.instance.HUDText.RefreshDailyValuesUI();
    }

    public void IncrementGems(int value = 1)
    {
        collectedGems += value;
        AssetCall.instance.HUDText.RefreshDailyValuesUI();
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

            PermaCollectedShield = data.hasShield;
            PermaCollectedGun = data.hasGun;
            PermaCollectedBombs = data.hasBombs;

            PermaDefeatedBombsBoss = data.deafeatedBombsBoss;
            PermaDefeatedGunsBoss = data.deafeatedGunsBoss;
        }
    }
}
