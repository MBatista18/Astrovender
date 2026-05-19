using UnityEngine;

public class FinalDoor : MonoBehaviour
{
    [SerializeField] SpriteRenderer leftDoor;
    [SerializeField] Sprite leftDoor_withKey;
    [SerializeField] Sprite leftDoor_withoutKey;

    [SerializeField] SpriteRenderer rightDoor;
    [SerializeField] Sprite rightDoor_withKey;
    [SerializeField] Sprite rightDoor_withoutKey;

    bool isOpening;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        leftDoor.sprite = GameManager.Instance.currentdataObj.finalKeyLeft ? leftDoor_withKey : leftDoor_withoutKey;
        rightDoor.sprite = GameManager.Instance.currentdataObj.finalKeyRight ? rightDoor_withKey : rightDoor_withoutKey;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (isOpening) { return; }

            if (!GameManager.Instance.currentdataObj.finalKeyLeft || !GameManager.Instance.currentdataObj.finalKeyRight)
            {
                return;
            }

            isOpening = true;

            GetComponent<Animator>().Play("FinalDoorOpen");
        }
    }
}
