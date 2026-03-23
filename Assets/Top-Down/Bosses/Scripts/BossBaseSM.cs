using UnityEngine;

public class BossBaseSM : EnemySM
{
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
