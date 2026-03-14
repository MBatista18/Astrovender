using UnityEngine;

public class TriggerOffsetCamera : MonoBehaviour
{
    [SerializeField] Vector2 offset;

    CameraEffectors cameraEffectors;

    private void Start()
    {
        cameraEffectors = AssetCall.instance.cameraEffectors;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
           cameraEffectors?.SetOffset(offset);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            cameraEffectors?.SetOffset(Vector2.zero);
        }
    }
}
