using UnityEngine;

public abstract class BaseBullet : MonoBehaviour, IPausable
{
    protected Rigidbody2D _rb;
    protected bool _isPaused;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        if (GameEventManager.Instance != null)
            GameEventManager.Instance.OnGamePaused += SetPaused;
    }

    protected virtual void OnDestroy()
    {
        if (GameEventManager.Instance != null)
            GameEventManager.Instance.OnGamePaused -= SetPaused;
    }

    public virtual void SetPaused(bool isPaused)
    {
        _isPaused = isPaused;
        if (_rb != null)
        {
            _rb.simulated = !isPaused;
        }
    }
}
