using UnityEngine;
using DG.Tweening;

public class SquashAndStretch : MonoBehaviour
{
    [SerializeField] private Vector3 punchScale = new Vector3(-0.2f, 0.2f, 0f);
    [SerializeField] private float duration = 0.25f;
    
    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    public void Squash()
    {
        // Kills any currently running tweens on this transform to prevent glitching
        transform.DOKill();
        
        // Reset scale instantly before punching
        transform.localScale = originalScale;
        
        // DOPunchScale creates a highly elastic, bouncy squash and stretch effect natively!
        // Parameters: Punch Vector, Duration, Vibrato (bounces), Elasticity
        transform.DOPunchScale(punchScale, duration, 10, 1f);
    }
}
