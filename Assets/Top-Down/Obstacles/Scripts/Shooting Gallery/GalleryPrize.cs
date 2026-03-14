using UnityEngine;

public class GalleryPrize : MonoBehaviour
{
    float positionY;

    private void Start()
    {
        positionY = transform.position.y;
        if (transform.childCount >= 1) { transform.GetChild(0).transform.localScale = Vector3.one * .6f; }
    }

    [SerializeField] float fallSpeed = 2f; 
    bool falling = false;
    public void SetToFall() { transform.SetParent(null, true); falling = true;
        if (transform.childCount >= 1) { transform.GetChild(0).transform.localScale = Vector3.one; } }

    private void Update()
    {
        if (falling) 
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - (fallSpeed * Time.deltaTime), transform.position.z);
        }
        
        if (transform.position.y < positionY) { falling = false; transform.position = new Vector3(transform.position.x, positionY, transform.position.z); }
    }
}
