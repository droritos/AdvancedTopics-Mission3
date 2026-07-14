# Unity Component Caching Rule

When writing Unity scripts, always use the `[SerializeField]` and `OnValidate()` pattern to cache components instead of using `GetComponent()` in `Awake()`, `Start()`, or `Update()`. This significantly improves performance by caching the reference in the editor.

## Pattern Example
```csharp
using UnityEngine;

public class ExampleScript : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private Rigidbody2D _rb;

    private void OnValidate()
    {
        if (_collider == null) _collider = GetComponent<Collider2D>();
        if (_rb == null) _rb = GetComponent<Rigidbody2D>();
    }
}
```

## Why?
- Avoids expensive runtime `GetComponent` calls.
- Automatically wires up references when scripts are added in the Inspector.
- Explicitly exposes dependencies in the Inspector for designers.
