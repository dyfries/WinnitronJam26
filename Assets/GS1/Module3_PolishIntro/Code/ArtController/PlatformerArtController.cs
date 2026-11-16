using UnityEngine;
using UnityEngine.Events;

// This script monitors the movement and jumping script, and communicates it to Animation. 
// It's often good practice to make the Art Controller a seperate element from the movement. 
// This setup is a little confusing because of the movement and jumping being seperate though. 
// Doesn't handle being hit yet. 
public class PlatformerArtController : MonoBehaviour
{
    // Jump Event
    public UnityEvent JumpStartedEvent = new UnityEvent();
    public UnityEvent StartedFallingEvent = new UnityEvent();
    public UnityEvent IdleEvent = new UnityEvent();
    public UnityEvent RunEvent = new UnityEvent();
    public UnityEvent HitEvent = new UnityEvent();

    // Requires at least one of the Jump or movement script. 
    [Header("Component References")]
    public Player_WorldForceMovement worldMovement;
    public Player_PlatformerForceJumping jumpingMovement;
    public Rigidbody2D rigid2D;
    public SpriteRenderer spriteRenderer;

    // A few settings
    [Header("Settings")]
    public bool flipSpriteOnLeftMovement = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // If we don't have these yet, grab them. 
        if(worldMovement == null) {
            worldMovement = GetComponentInParent<Player_WorldForceMovement>();
            if(worldMovement == null) { Debug.LogError("Platformer Art Controller Relies on the Movement Controller"); }
        }

        if (jumpingMovement == null) {
            jumpingMovement = GetComponentInParent<Player_PlatformerForceJumping>();
            if (jumpingMovement == null) { Debug.LogError("Platformer Art Controller Relies on the Player_PlatformerForceJumping"); }
        }

        if (rigid2D == null) {
            rigid2D = GetComponentInParent<Rigidbody2D>();
            if (rigid2D == null) { Debug.LogError("Platformer Art Controller Relies on the Rigidbody2D"); }
        }
    }

    // Update is called once per frame
    void Update() {
        if (jumpingMovement.IsGrounded()) {
            // Apply Animation based on Movement
            if(Mathf.Abs(worldMovement.GetCurrentInput().x) > .1f) {
                RunEvent.Invoke();
            } else {
                IdleEvent.Invoke();
            }

        } else {
            // In the Air
            // Rising if Velocity is > 0
            if (rigid2D.linearVelocityY > 0) {
                JumpStartedEvent.Invoke();
            } else {
                StartedFallingEvent.Invoke();
            }
        }

        if (flipSpriteOnLeftMovement) {
            // Also want to flip the direction of the sprite if we are going left
            // I gave it a bit of a buffer so it doesn't flip near 0 when we stop. 
            if (worldMovement.GetCurrentInput().x > 0.1f) {
                spriteRenderer.flipX = false;
            } else if (worldMovement.GetCurrentInput().x < -0.1f) {
                spriteRenderer.flipX = true;
            }
        }

    }
}
