using UnityEngine;
using System.Collections;
using DG.Tweening;

public class JuiceManager : MonoBehaviour
{
    public static JuiceManager Instance;

    [Header("Juice Toggles")]
    public bool enableCameraShake = true;
    public bool enableHitStop = true;
    public bool enableFlipbook = true;

    [Header("Camera Shake Settings")]
    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.3f;

    private Vector3 originalCameraPos;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
            
        if (Camera.main != null)
        {
            originalCameraPos = Camera.main.transform.localPosition;
        }
    }

    private void Start()
    {
        if (Camera.main != null)
            originalCameraPos = Camera.main.transform.localPosition;

        if (GameEventManager.Instance != null)
        {
            GameEventManager.Instance.OnImpactOccurred += TriggerImpact;
        }
    }

    private void OnDestroy()
    {
        if (GameEventManager.Instance != null)
        {
            GameEventManager.Instance.OnImpactOccurred -= TriggerImpact;
        }
    }

    private void TriggerImpact()
    {
        if (enableCameraShake && Camera.main != null)
        {
            Camera.main.transform.DOComplete();
            Camera.main.transform.DOShakePosition(shakeDuration, shakeMagnitude, 30, 90f, false, true);
        }

        if (enableHitStop && Time.timeScale > 0.1f) // Prevent overlapping hit stops
        {
            StartCoroutine(HitStopRoutine());
        }
    }

    private IEnumerator HitStopRoutine()
    {
        Time.timeScale = 0.05f;
        yield return new WaitForSecondsRealtime(0.05f); // Freeze for 50 milliseconds
        Time.timeScale = 1f;
    }
}
