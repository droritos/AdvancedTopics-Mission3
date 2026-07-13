using UnityEngine;

public class ReturnVfxToPool : MonoBehaviour
{
    public float delay = 1f;

    private void OnEnable()
    {
        Invoke(nameof(Return), delay);
    }

    private void Return()
    {
        PoolManager.Instance.ReturnToPool(gameObject.name, gameObject);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
