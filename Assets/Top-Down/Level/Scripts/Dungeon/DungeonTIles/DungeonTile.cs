using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DungeonTile : MonoBehaviour
{
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

    private void Start()
    {

        isStartingRoom = Physics2D.BoxCast(transform.position, new Vector2(15, 10), 0f, Vector2.zero, 0f, LayerMask.GetMask("Player"));

        Debug.Log(Physics2D.BoxCast(transform.position, new Vector2(15, 10), 0f, Vector2.zero, 0f, LayerMask.GetMask("Player")));

        RaycastHit2D[] rayAll = Physics2D.BoxCastAll(transform.position, new Vector2(15, 10), 0f, Vector2.zero, 0f, LayerMask.GetMask("Enemy", "Destructible"));

        if (rayAll.Length <= 0) { roomObjects = new GameObject[0]; return; }

        roomObjects = new GameObject[rayAll.Length];

        for (int i = 0; i < rayAll.Length; i++)
        {
            roomObjects[i] = rayAll[i].collider.gameObject;
            roomObjects[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (!image) { return; }

        if (playerIsInRoom) { image.color = Color.red; return; }
        if (isStartingRoom) { image.color = Color.blue; return; }
        if (roomHasBeenEntered) { image.color = Color.white; return; }
        image.color = Color.black;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            SetActiveArray(true);

            roomHasBeenEntered = true;

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
