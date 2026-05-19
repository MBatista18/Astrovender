using UnityEngine;

public class BossBaseSM : EnemySM
{
    BossMusicPlayer musicPlayer;

    public override void OnEnableFunctions()
    {
        base.OnEnableFunctions();

        musicPlayer = FindFirstObjectByType<BossMusicPlayer>();
        musicPlayer?.StartBossTheme();
    }

    public override void OnDisableFunctions()
    {
        base.OnDisableFunctions();

        musicPlayer?.EndBossTheme();
    }

    public override StateBase DeathState()
    {
        DungeonDatObj dataObj;
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        if (GameManager.Instance.currentdataObj.dungeons.TryGetValue(currentSceneName, out dataObj))
        {
            GameManager.Instance.currentdataObj.dungeons.Remove(currentSceneName);

            dataObj.defeatedBoss = true;
            GameManager.Instance.currentdataObj.dungeons.Add(currentSceneName, dataObj);
        }

        return base.DeathState();
    }
}
