using UnityEngine;

public class CameraEffectors : MonoBehaviour
{
    [Header("Camera Shake")]
    [SerializeField] float cameraShakeStrength;
    [SerializeField] float cameraShakeMovementCap;

    [Header("Camera Offset")]
    [SerializeField] float offsetSpeed;
    Vector2 offset;
    public void SetOffset(Vector2 value) { offset = value; }

    float cameraShake;
    public void SetCameraShake(float duration)
    {
        cameraShake = duration;
    }

    private void Update()
    {
        if (cameraShake > 0) { cameraShake -= Time.deltaTime; }

        transform.localPosition = Vector3.MoveTowards(transform.localPosition, offset, offsetSpeed * Time.deltaTime);
    }

    private void LateUpdate()
    {
        if (cameraShake > 0)
        {
            transform.localPosition
                = new Vector3(
                    Mathf.Sin(Time.time * cameraShakeStrength) * Mathf.Clamp(cameraShake, 0, cameraShakeMovementCap),
                    transform.localPosition.y,
                    transform.localPosition.z
                );
        }
    }
}
