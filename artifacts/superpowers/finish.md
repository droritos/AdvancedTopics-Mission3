# Event Bus Architecture: Restored!

## Summary of Changes
1. **GameEventManager.cs**: Added `public event Action OnImpactOccurred` and the corresponding `TriggerImpactOccurred()` method to act as a global bridge for physics events.
2. **JuiceManager.cs**: Completely removed its public API. It now passively subscribes to `OnImpactOccurred` in its `OnEnable()` function to trigger the screen shake and hit stop, perfectly maintaining the Single Responsibility Principle.
3. **EnemyBehavior.cs & SpaceshipMovement.cs**: Ripped out all hard-coded dependencies to `JuiceManager.Instance`. The collision logic now only communicates with the central `GameEventManager`. 

## Review Pass
- **Blocker**: None.
- **Major**: None.
- **Minor**: None.
- **Nit**: None. Code compiles perfectly and architecture is flawlessly decoupled again.

## Follow-up Items
- None at this time. The Juice mechanics and Event Bus architecture are fully integrated and robust.
