using System;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour, ISaveable
{
    public static GameManager Instance;

    /*[HideInInspector] public int CurrentDay { get; private set; } = 0;
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
    [HideInInspector] public bool defeatedGunsBoss;*/

    public int collectedCoins;
    public int collectedGems;

    public DataObj currentdataObj = new DataObj("");
    public DataObj startingDataObj = new DataObj("");

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
    }

    void Start()
    {
        StartDay(); // this should be removed for the final build, otherwise everytime the player starts the game and this object is created, it'll start a new day
    }

    public void Progress(bool success)
    {
        if (success)
        {
            /*currentdataObj.coins += collectedGems;
            currentdataObj.coins += collectedCoins;

            if (PermaCollectedBombs == false) { PermaCollectedBombs = collectedBombs; }
            if (PermaCollectedGun == false) { PermaCollectedGun = collectedGun; }
            if (PermaCollectedShield == false) { PermaCollectedShield = collectedShield; }

            if (PermaDefeatedBombsBoss == false) { PermaDefeatedBombsBoss = defeatedBombsBoss; }
            if (PermaDefeatedGunsBoss == false) { PermaDefeatedGunsBoss = defeatedGunsBoss; }*/

            ModifyDataCoinCountBy(collectedCoins);
            ModifyDataGemCountBy(collectedGems);
        }
        else
        {
            startingDataObj.day = currentdataObj.day; // saves the day count but resets everything else before saving.
            currentdataObj = startingDataObj;

            Debug.Log("Day was failed. Progress lost");
        }

        progressSuccessful = success;

        CallSaveGame();
    }

    public void ModifyDataCoinCountBy(int val)
    {
        currentdataObj.coins += val;
        currentdataObj.coins = Mathf.Clamp(currentdataObj.coins, 0, 999999999);
    }

    public void ModifyDataGemCountBy(int val)
    {
        currentdataObj.gems += val;
        currentdataObj.gems = Mathf.Clamp(currentdataObj.gems, 0, 999999999);
    }

    public void CallSaveGame()
    {
        SaveManager.Instance.Save(this);
    }

    public void StartDay()
    {
        IncrementDay();
        //Debug.Log("A new day has started. Current Day: " + 1CurrentDay);

        /*collectedCoins = 0;
        collectedGems = 0;

        collectedBombs = PermaCollectedBombs;
        collectedGun = PermaCollectedGun;
        collectedShield = PermaCollectedShield;

        defeatedBombsBoss = PermaDefeatedBombsBoss;
        defeatedGunsBoss = PermaDefeatedGunsBoss;*/

        startingDataObj = currentdataObj;

        PlayerManager.ResetPlayerValues();
        PlayerManager.playerWorldSpawn = new Vector3(10, 23);
        //SaveManager.Instance.Save(this);
    }

    public void IncrementDay(int value = 1)
    {
        currentdataObj.day += value;
        collectedCoins = 0;
        collectedGems = 0;
    }

    public void IncrementCoins(int value = 1)
    {
        collectedCoins += value;
        Mathf.Clamp(collectedCoins, 0, 999999999);
        AssetCall.instance.HUDText.RefreshDailyValuesUI();
    }

    public void IncrementGems(int value = 1)
    {
        collectedGems += value;
        Mathf.Clamp(collectedGems, 0, 999999999);
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
            /*CurrentDay = data.day;
            CurrentCoins = data.coins;
            CurrentGems = data.gems;

            PermaCollectedShield = data.hasShield;
            PermaCollectedGun = data.hasGun;
            PermaCollectedBombs = data.hasBombs;

            PermaDefeatedBombsBoss = data.deafeatedBombsBoss;
            PermaDefeatedGunsBoss = data.deafeatedGunsBoss;*/

            currentdataObj = saveData.gameData.data;
            currentdataObj.coins = 0;
            currentdataObj.gems = 0;
        }
    }
}
