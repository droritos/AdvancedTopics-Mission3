using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceshipMovement : MonoBehaviour, IPausable
{   
    [SerializeField] float shipSpeed = 10f;
    [SerializeField] float maxTiltAngle = 35f;
    [SerializeField] float tiltSpeed = 12f;

    public bool isAlive = true;
    public GameObject collisionVfxPrefab;
    private float _knockbackRecoveryTime;
    private Rigidbody2D _rb;
    private bool _isPaused;

    public void SetPaused(bool isPaused)
    {
        _isPaused = isPaused;
        if (_rb != null) _rb.simulated = !isPaused;
    }

    private void Start()
    {
        if (GameEventManager.Instance != null)
            GameEventManager.Instance.OnGamePaused += SetPaused;
    }

    private void OnDestroy()
    {
        if (GameEventManager.Instance != null)
            GameEventManager.Instance.OnGamePaused -= SetPaused;
    }

    private void OnEnable()
    {
        if (_rb == null) _rb = GetComponent<Rigidbody2D>();
        if (_rb != null) _rb.constraints = RigidbodyConstraints2D.None; // Unfreeze Z rotation so tilt juice works
        GameEventManager.OnRequestPlayerTransform += GetPlayerTransform;
    }

    private void OnDisable()
    {
        GameEventManager.OnRequestPlayerTransform -= GetPlayerTransform;
    }

    private Transform GetPlayerTransform()
    {
        return transform;
    }

    void Update()
    {
        if (_isPaused) return;
        ShipMovement();
    }

    void ShipMovement()
    {
        if (Time.time < _knockbackRecoveryTime) return;
        
        // Snappy stop: kill lingering momentum after the stun finishes
        if (_rb != null && _rb.linearVelocity.sqrMagnitude > 0.1f)
        {
            _rb.linearVelocity = Vector2.zero;
        }
        if (_rb != null) _rb.angularVelocity = 0f; // Prevent physics collisions from spinning the ship wildly
        float horizontalInput = Input.GetAxis("Horizontal"); // Note: Getting acces to the Horizontal values 
        float verticalInput = Input.GetAxis("Vertical"); // Note: Getting acces to the Vertical values
        
        // Move in Space.World so our visual rotation doesn't ruin our physical direction!
        transform.Translate(new Vector2(horizontalInput * Time.deltaTime * shipSpeed, verticalInput * Time.deltaTime * shipSpeed), Space.World);
    }

    private float _currentTilt = 0f;

    void LateUpdate()
    {
        if (_isPaused) return;
        if (Time.time < _knockbackRecoveryTime) return;

        // Feedback: Tilt ship based on horizontal movement
        // We do this in LateUpdate to forcibly override any Animator keyframes that might be locking the rotation!
        float horizontalInput = Input.GetAxis("Horizontal");
        float targetZRotation = -horizontalInput * maxTiltAngle;
        
        _currentTilt = Mathf.Lerp(_currentTilt, targetZRotation, Time.deltaTime * tiltSpeed);
        transform.rotation = Quaternion.Euler(0, 0, _currentTilt);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Died Enemy"))
        {
            if (GameEventManager.Instance != null)
            {
                GameEventManager.Instance.TriggerImpactOccurred();
            }
            Vector2 bounceDirection = (transform.position - collision.transform.position).normalized;
            if (bounceDirection == Vector2.zero) bounceDirection = Random.insideUnitCircle.normalized;
            
            if (_rb != null)
            {
                _rb.linearVelocity = Vector2.zero;
                _rb.AddForce(bounceDirection * 5f, ForceMode2D.Impulse); // Reduced force
                _knockbackRecoveryTime = Time.time + 0.15f; // Quicker stun

                if (collisionVfxPrefab == null)
                {
#if UNITY_EDITOR
                    collisionVfxPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefab/CollisionVFX.prefab");
#endif
                }
                if (collisionVfxPrefab != null)
                {
                    PoolManager.Instance.SpawnFromPool(collisionVfxPrefab.name, collisionVfxPrefab, collision.contacts[0].point, Quaternion.identity);
                }
            }
        }
    }
}
