using UnityEngine;

public class SlidingWall : MonoBehaviour
{
    [SerializeField] float movementDistance;
    [SerializeField] float movementSpeed;

    float y;
    float x;

    [SerializeField] bool isMovingVertical;

    private void Start()
    {
        x = transform.position.x;
        y = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = isMovingVertical ? new Vector3(
                x, 
                y + (Mathf.Sin(Time.time * movementSpeed) * (movementDistance)+ (movementDistance))):
            new Vector3(
                x + (Mathf.Sin(Time.time * movementSpeed) * (movementDistance) + (movementDistance)), 
                y);
    }
}
