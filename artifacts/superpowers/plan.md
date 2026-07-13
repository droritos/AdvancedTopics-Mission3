# Doodle Aesthetic "Juice" Implementation

## Proposed Changes

### 1. Centralized Juice Manager
- **`JuiceManager.cs`**: A new centralized manager handling global screen effects. Includes toggles for Hit Stop, Camera Shake, Flipbook Effect, and Paper Overlay.

### 2. Camera Shake & Hit Stop (Impact Freezes)
- Modify `EnemyBehavior.cs` and `SpaceshipMovement.cs` to trigger `JuiceManager` on impacts.
- Adds `Time.timeScale` freezing and `Camera.main.transform.localPosition` perlin noise shaking.

### 3. Spider-Verse Flipbook Effect
- **`FlipbookEffect.cs`**: A script that visually throttles rendering framerate (e.g. 12 FPS) without slowing down the physics engine, creating a stop-motion look.

### 4. Squash and Stretch (Shooting)
- **`SquashAndStretch.cs`**: Coroutine that physically squashes and stretches the sprite scales upon shooting to add weight.

### 5. Raw Paper Overlay
- **`CreatePaperOverlay.cs`**: Editor script that generates a static UI Canvas overlay with a "Multiply" blend paper texture to simulate real hand-drawn paper.
