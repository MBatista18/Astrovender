using UnityEngine;

public class LockedDoorKey : MonoBehaviour
{
    [System.Serializable]
    public enum KeyColor
    {
        Red,
        Green,
        Blue
    }
    public KeyColor keyColor;

    private void Start()
    {
        DungeonDatObj a = GameManager.Instance.currentdataObj.dungeons[UnityEngine.SceneManagement.SceneManager.GetActiveScene().name];

        if (keyColor == KeyColor.Red) 
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 0.3647059f, 0.3647059f,1);
            if (!a.hasRedKey) { return; }
        }
        else if (keyColor == KeyColor.Blue)
        {
            GetComponent<SpriteRenderer>().color = new Color(0.3647059f, 0.3647059f, 1, 1);
            if (!a.hasBlueKey) { return; }
        }
        else if (keyColor == KeyColor.Green)
        {
            GetComponent<SpriteRenderer>().color = new Color(0.3647059f, 1, 0.3647059f, 1);
            if (!a.hasGreenKey) { return; }
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            DungeonDatObj dataObj;
            string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            if (GameManager.Instance.currentdataObj.dungeons.TryGetValue(currentSceneName, out dataObj))
            {
                GameManager.Instance.currentdataObj.dungeons.Remove(currentSceneName);

                switch (keyColor) 
                {
                    case KeyColor.Red:
                        dataObj.hasRedKey = true;
                        break;
                    case KeyColor.Green:
                        dataObj.hasGreenKey = true;
                        break;
                    case KeyColor.Blue:
                        dataObj.hasBlueKey = true;
                        break;
                }

                GameManager.Instance.currentdataObj.dungeons.Add(currentSceneName, dataObj);
            }

            Destroy(gameObject);
        }
    }
}
