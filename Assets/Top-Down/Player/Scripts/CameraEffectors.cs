using UnityEngine;

public class CameraEffectors : MonoBehaviour
{
    [Header("Camera Shake")]
    [SerializeField] float cameraShakeStrength;
    [SerializeField] float cameraShakeMovementCap;

    float cameraShake;
    public void SetCameraShake(float duration)
    {
        cameraShake = duration;

        Debug.Log("Shake = " + cameraShake);
    }

    private void Update()
    {
        if (cameraShake > 0) { cameraShake -= Time.deltaTime; }
    }

    private void LateUpdate()
    {
        Debug.Log("Shake value current: " + cameraShake);

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
