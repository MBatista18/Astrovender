using UnityEngine;

public class CameraGrid : MonoBehaviour
{
    public Transform target;
    public Vector2 size;
    public float speed;

    Vector2 cameraStartPos;

    [Tooltip("Causes the camera to ignore the grid")] [SerializeField] bool parentToPlayer;

    private void Start()
    {
        if (parentToPlayer) { 
            this.transform.parent = target;
            transform.position = target.position + new Vector3(0, 0, -10);
            return; }

        cameraStartPos = transform.position;
    }

    void Update()
    {
        if (parentToPlayer) { return; }

        Vector3 pos = new Vector3(
            Mathf.RoundToInt((target.position.x - cameraStartPos.x) / size.x) * size.x, 
            Mathf.RoundToInt((target.position.y - cameraStartPos.y) / size.y) * size.y, transform.position.z); 

        transform.position = Vector3.Lerp(transform.position, (Vector3) cameraStartPos + pos, speed * Time.deltaTime);
    }
}
 