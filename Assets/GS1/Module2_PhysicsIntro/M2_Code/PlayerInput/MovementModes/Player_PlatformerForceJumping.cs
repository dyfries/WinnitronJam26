using UnityEngine;
// Input 1. - Required for Input system
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
// This Input System just listens for the Jump button and applies a Jump. 
// Includes Raycasting for the ground. 
// Combine with Player_WorldForceMovement script (with Y force = 0) for a normal jumping platformer. 

public class Player_PlatformerForceJumping : MonoBehaviour
{
    [Header("Asset Settings")]
    public float jumpForce = 1f;
    public float jumpingCooldown = .2f; // how long after jumping can we try to jump agian.

    [Header("Physics Settings")]
    public LayerMask groundLayerMask;
    public float groundCheckDistance = .1f;


    // I also like ContactFilters for more options - See docs for details 

    [Header("Cached Values")]
    // shouldn't be public usually but I want the input to be visible for debugging right now. 
    public bool requestToJump = false;
//    public bool allowDoubleJump = false;
    private bool isGrounded = false;
    private bool onCooldown = false;
    private float currentCountdownTimer = 0f;
 //   private bool usedDoubleJump = false; 


    // Physics Rigidbody Reference. Hide it for now since we are enforcing it with RequireComponent
    private Rigidbody2D rigid2d;

    // --- Note - This is all setup for Input system, skip down to the Update loop for the rest ---
    // Input 2. - Make a variable to hold a reference to the input asset. (basically)
    InputSystem_Actions actions; // This is the default. Needs the C# Code to be generated to work. This seems like the cleanest solution to me code wise and avoids strings etc. 

    public bool DEBUG_MODE = false; // draw the raycasts. 

    public bool IsGrounded() {  return isGrounded; }
    

    private void HandleJump(InputAction.CallbackContext context) {
        // Added grounded and cooldown check as the input was cueing up in the air, then jumping again on landing. 
        if(isGrounded && !onCooldown) {
            requestToJump = true; // don't need to read the value here (in fact reading the value will crash).
        }

        if (DEBUG_MODE) {
            Debug.Log("Registering Input Jump Command");
        }
    }

    // --- Fixed Update - Where Physics Updates happen (instead of Update) ---
    private void FixedUpdate() {

        // Update cooldown state
        if (onCooldown) {
            currentCountdownTimer -= Time.fixedDeltaTime;
            if (currentCountdownTimer <= 0) {
                // If our timer is complete, reset cooldown. 
                onCooldown = false;
            }
        }

        // Update grounded state
        // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Physics2D.Raycast.html
        // Note this uses the transform location, so your sprite should have it's pivot point at the bottom of the transform, and your collider should be offset . 
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayerMask);

        // Apply results of the raycast hit. 
        if (hit) {
            isGrounded = true;
        } else {
            isGrounded = false;
        }

        if (DEBUG_MODE) {
            // Probably shouldnt be in physics update for performance but fine for now. 
            if (hit) {
                Debug.DrawRay(transform.position, Vector2.down * groundCheckDistance, Color.green);
            } else {
                Debug.DrawRay(transform.position, Vector2.down * groundCheckDistance, Color.red);
            }
            
        }

        // Jumping
        if (requestToJump) {
            // Check state for in air
            if (isGrounded && !onCooldown) {
                ApplyJump();
            }
        }
    }

    private void ApplyJump() {
        // Add the actual Force, use impulse to add all force on a single frame. 
        rigid2d.AddForceY(jumpForce, ForceMode2D.Impulse);

        // Reset state vars. 
        requestToJump = false; // reset the flag from input
        // Start cooldown timer (to keep from being able to hit the button twice in a row quickly. 
        onCooldown = true;
        currentCountdownTimer = jumpingCooldown;
    }

    // --- Input System Setup ---
    // Input 3. Get a reference in Awake
    void Awake() {
        // Get Rigidbody Reference. Since we hid it as private we'll need to grab it here. 
        rigid2d = GetComponent<Rigidbody2D>();
        actions = new InputSystem_Actions(); // Not an object, so we are creating a new wrapper. 
    }

    // Input 4. Enable and disable actions when entering and leaving scene. 
    void OnEnable() {
        actions.Enable();    // enable actions. 
        // Subscribe input events to the Handler Methods. 
        actions.Player.Jump.performed += HandleJump;
    }

    void OnDisable() {
        actions.Disable(); //disable when leaving the scene. 
        // De-Subscribe input events to the Handler Methods. 
        actions.Player.Jump.performed -= HandleJump;
    }

}
