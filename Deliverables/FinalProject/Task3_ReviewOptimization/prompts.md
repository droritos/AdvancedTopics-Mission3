# Mission 3: Prompts Log

Here are 15 significant prompts used during this mission, structured to meet the assignment requirements:

### 1. "review projcet and make it better"
*   **Why used:** To initiate the core refactoring task and get an AI-generated architectural assessment of the current codebase.
*   **Result:** The AI performed a full code review and generated a formal Implementation Plan targeting Singletons and GC Allocations.
*   **Use again?** Yes, it is an excellent starting point to let the AI build context.
*   **Improvement:** Be more specific upfront about what "better" means (e.g., "focus on performance and memory").

### 2. "also i must i update the project to unity 6. in additon a some sort of event system bus that i prefer working insteaf of depedencies and singletons"
*   **Why used:** To steer the AI's architecture plan toward the exact design pattern (Event Bus) required by the assignment and ensure Unity 6 compliance.
*   **Result:** The AI updated the plan to include Unity 6 API compliance (`FindAnyObjectByType`) and designed the `GameEventManager`.
*   **Use again?** Yes, forcing the AI to use specific patterns prevents spaghetti code.
*   **Improvement:** Provide a strict code example of the desired Event Bus syntax in the prompt.

### 3. "/superpowers-reload"
*   **Why used:** To trigger a specific slash command that reloads the agent's workflow rules from disk.
*   **Result:** Enabled the `/superpowers-execute-plan-parallel` workflow, granting the AI the ability to spawn subagents.
*   **Use again?** Yes, essential for workflow management.
*   **Improvement:** N/A, it's a static system command.

### 4. "yeah ok but also want to create like a generic poolmanager so we can pool what ever we want"
*   **Why used:** To reject the AI's initial hardcoded bullet pool and force it to create a scalable, generic pooling system.
*   **Result:** The AI created a dictionary-based `PoolManager.cs` capable of pooling both bullets and enemies.
*   **Use again?** Yes, demanding generic systems from AI saves hours of future refactoring.
*   **Improvement:** Ask the AI to also include auto-expand logic if the pool runs empty.

### 5. "/superpowers-execute-plan" -> followed by "u can PARALLEL"
*   **Why used:** To authorize the AI to begin coding and explicitly command it to use parallel subagents to save time.
*   **Result:** The AI deployed multiple autonomous subagents (`invoke_subagent`) to execute the Event Bus and Object Pooling tasks simultaneously.
*   **Use again?** Absolutely, parallel agent execution cuts task time in half.
*   **Improvement:** Monitor the subagents more closely, as parallel file edits can sometimes cause merge conflicts.

### 6. "also later on I want to also change the enemies movements ithey are like moving the same... add moe feedbacks when enmies hit, exploade I want to make like splash ink droplets"
*   **Why used:** To expand the scope of the project into Game Feel and visual feedback after the core refactor was completed.
*   **Result:** The AI paused, updated the plan, and generated new orbital movement math and ink splash particle spawning logic.
*   **Use again?** Yes, prompting for "juice" yields highly creative AI solutions.
*   **Improvement:** Break this into two prompts (one for movement, one for VFX) to keep the AI focused.

### 7. "the color of the particle need to be changed depoend on the Main color on the enemy"
*   **Why used:** To refine the VFX logic so the ink splashes match the dynamic tinting of the enemy sprites.
*   **Result:** The AI successfully wrote logic to extract `SpriteRenderer.color` and apply it to `ParticleSystem.main.startColor`.
*   **Use again?** Yes, specific aesthetic corrections work great with AI.
*   **Improvement:** Explicitly mention `MaterialPropertyBlock` if the color is driven by a shader rather than the base SpriteRenderer.

### 8. "can u create an asset image of the Droplets collection white ones with outliine black that fit in the game other assets... than we will use the that for the particles"
*   **Why used:** To leverage the AI's multi-modal capabilities (image generation) to create actual game assets.
*   **Result:** The AI generated `InkDroplets.png` matching the game's doodle aesthetic.
*   **Use again?** Yes, generating placeholder or final 2D assets inside the IDE is incredibly fast.
*   **Improvement:** Provide a more detailed prompt for the image generation model (e.g., mentioning "transparent background, 2D flat vector style").

### 9. "piercing bullets are like not handled like the regular bullets they are getting flynig around after colliison with an enmy"
*   **Why used:** To report a specific logical bug with the piercing bullet implementation during QA.
*   **Result:** The AI investigated and successfully refactored `BulletPiercingCollision.cs` to utilize the new `IDamageable` interface.
*   **Use again?** Yes, describing the *behavioral symptom* is highly effective for AI debugging.
*   **Improvement:** Provide the exact file name if known to save the AI search time.

### 10. "also the boss bullets after i kill him the ones that are still alive are still moving and not getting pause"
*   **Why used:** To catch an edge case where dynamically spawned boss bullets weren't hooking into the new Event Bus pause system.
*   **Result:** The AI modified the Boss Bullets to inherit from the newly created `BaseBullet` class so they automatically receive pause events.
*   **Use again?** Yes, pointing out missing edge cases is a core human-in-the-loop requirement.
*   **Improvement:** None, perfect descriptive bug report.

### 11. "lets add new samll hack that skipp me to the upgrade station so i can test out the upgrades"
*   **Why used:** To ask the AI to build a temporary development/debug tool to accelerate human QA testing.
*   **Result:** The AI successfully created a temporary skip-level mechanic.
*   **Use again?** Absolutely. Having the AI build debug tools is a massive time-saver.
*   **Improvement:** Remind the AI to wrap the hack in `#if UNITY_EDITOR` tags so it doesn't ship in the final build.

### 12. "MissingComponentException: There is no 'Rigidbody2D' attached to the 'Boss_1Bullet 1(Clone)' game object..."
*   **Why used:** To feed a raw Unity console stack trace directly into the AI for rapid resolution.
*   **Result:** The AI instantly identified the missing physics component on the prefab and wrote a dynamic fallback script to attach it at runtime.
*   **Use again?** Yes, pasting stack traces is the fastest way to debug with AI.
*   **Improvement:** Also paste the line of code referenced in the stack trace if the AI's context window has shifted.

### 13. "the explosion bullet somtines work somtine not feels like if the enemy dies than i dont see the explosion"
*   **Why used:** To describe a visual bug where the physical hitboxes of dead enemies were blocking instantiated shrapnel.
*   **Result:** The AI correctly deduced that the `Collider2D` was remaining active during the death animation and disabled it on death.
*   **Use again?** Yes, translating "game feel" issues into technical bugs is a great AI use-case.
*   **Improvement:** Attach a screenshot of the Unity Physics Debugger if possible.

### 14. "enemies are still like summoned with the script disabled ! make them not moving"
*   **Why used:** To solve an issue where prefabs were accidentally saved with their behavior scripts unchecked in the inspector.
*   **Result:** The AI attempted several code workarounds (like forcing `this.enabled = true` in `Awake()`) before the user realized it was an Animator bug.
*   **Use again?** Yes, but with caution regarding Unity Editor specific states.
*   **Improvement:** Provide a screenshot of the Inspector panel sooner, as AI cannot "see" unticked boxes unless using MCP or Vision.

### 15. "also why getcompnent u cacn do serlizfielld of the cloidder2d and apply OnValidte()... learn this and add that to skills"
*   **Why used:** To actively teach the AI a specific Unity performance optimization technique and force it to remember it for the future.
*   **Result:** The AI learned the pattern, applied it to the codebase, and generated a persistent `.agents/AGENTS.md` global rule, successfully updating its permanent behavior.
*   **Use again?** Absolutely. Teaching the agent custom rules is the most powerful feature of an agentic workflow.
*   **Improvement:** N/A, this was a flawless execution of Agent teaching.
