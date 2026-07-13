using UnityEngine;

public class FlipbookEffect : MonoBehaviour
{
    private void Start()
    {
        // The previous AI script disabled the camera, which completely broke Cinemachine!
        // We ensure it is re-enabled here to restore your vision.
        if (TryGetComponent<Camera>(out Camera cam))
        {
            cam.enabled = true;
        }
    }
}
