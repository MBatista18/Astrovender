using UnityEngine;

public class CameraScale : MonoBehaviour
{
    [SerializeField] Camera cameraObj;


    private void LateUpdate()
    {
        float newCameraScale = 12f / ((float) cameraObj.scaledPixelWidth / (float)cameraObj.scaledPixelHeight);

        cameraObj.orthographicSize = newCameraScale;
    }
}
