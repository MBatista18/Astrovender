using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(ObjectID))]
public class DungeonTile : MonoBehaviour
{
    ObjectID objectID;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(15, 10));
    }

    bool roomHasBeenEntered = false;
    public bool GetIfRoomHasBeenEntered() { return roomHasBeenEntered; }

    bool playerIsInRoom = false;
    public bool GetIfPlayerIsInRoom() { return playerIsInRoom; }

    bool isStartingRoom = false;
    public bool GetIsStartingRoom() { return isStartingRoom; }

    Image image;
    public void SetImage(Image _image)
    {
        image = _image;
    }

    GameObject[] roomObjects;

    private void Awake()
    {
        objectID = GetComponent<ObjectID>();
    }

    private void Start()
    {

        isStartingRoom = Physics2D.BoxCast(transform.position, new Vector2(15, 10), 0f, Vector2.zero, 0f, LayerMask.GetMask("Player"));

        //Debug.Log(Physics2D.BoxCast(transform.position, new Vector2(15, 10), 0f, Vector2.zero, 0f, LayerMask.GetMask("Player")));

        RaycastHit2D[] rayAll = Physics2D.BoxCastAll(transform.position, new Vector2(15, 10), 0f, Vector2.zero, 0f, LayerMask.GetMask("Enemy", "Destructible", "Obstacle"));

        if (rayAll.Length <= 0) { roomObjects = new GameObject[0]; return; }

        roomObjects = new GameObject[rayAll.Length];

        for (int i = 0; i < rayAll.Length; i++)
        {
            roomObjects[i] = rayAll[i].collider.gameObject;
            roomObjects[i].SetActive(false);
        }

        if (GameManager.Instance.currentdataObj.saveENVGameWorld.Contains(objectID.GetID()))
        {
            roomHasBeenEntered = true;
        }
    }

    private void Update()
    {
        if (!image) { return; }

        if (playerIsInRoom) { image.sprite = AssetCall.instance.mapGetSprites?.GetSpriteBoxPlayer(); return; }
        if (isStartingRoom) { image.sprite = AssetCall.instance.mapGetSprites?.GetSpriteBoxEntrance(); return; }
        if (roomHasBeenEntered) { image.sprite = AssetCall.instance.mapGetSprites?.GetSpriteBoxOpen(); return; }
        image.sprite = AssetCall.instance.mapGetSprites?.GetSpriteBoxClosed();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            SetActiveArray(true);

            if (!roomHasBeenEntered)
            {
                roomHasBeenEntered = true;

                GameManager.Instance.currentdataObj.saveENVGameWorld.Add(objectID.GetID());
            }

            playerIsInRoom = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            SetActiveArray(false);

            playerIsInRoom = false;
        }
    }

    void SetActiveArray(bool setactive)
    {
        if (roomObjects.Length <= 0) { return; }

        for (int i = 0; i < roomObjects.Length; i++)
        {
            if (roomObjects[i] == null) { continue; }

            roomObjects[i].SetActive(setactive);
        }
    }
}
