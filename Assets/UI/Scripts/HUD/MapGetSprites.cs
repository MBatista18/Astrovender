using UnityEngine;

public class MapGetSprites : MonoBehaviour
{

    [SerializeField] Sprite boxOpen, boxClosed, boxEntrance, boxPlayer;
    public Sprite GetSpriteBoxOpen() { return boxOpen; }
    public Sprite GetSpriteBoxClosed() { return boxClosed; }
    public Sprite GetSpriteBoxEntrance() { return boxEntrance; }
    public Sprite GetSpriteBoxPlayer() { return boxPlayer; }

}
