using UnityEngine;
// Input 1. - Required for Input system
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
/*
 * In this Demo we will look at using the AddRelativeForce method and Add Torque method to 
 * make a player character that will move relative to its own orientation, rather then the world orientation. 
 * This is typical of games such as Asteroids, or racing/driving/flying games. 
 */
public class Player_RelativeForceMovement : MonoBehaviour
{
    [Header("Asset Settings - Movement Force")]
    public float forwardMoveForce = 2f; // Move faster forward.  
    public float reverseMoveForce = 1f;
    public float rotationTorque = 1f; // Turning

    [Header("Cached Values")]
    // shouldn't be public usually but I want the input to be visible for debugging right now. 
    public Vector2 moveInput;

    // --- Note - This is all setup for Input system, skip down to the Update loop for the rest ---
    // Input 2. - Make a variable to hold a reference to the input asset. (basically)
    InputSystem_Actions actions; // This is the default. Needs the C# Code to be generated to work. This seems like the cleanest solution to me code wise and avoids strings etc. 

    // Physics Rigidbody Reference. Hide it for now since we are enforcing it with RequireComponent
    private Rigidbody2D rigid2d;

    // --- Fixed Update - Where Physics Updates happen (instead of Update) ---
    private void FixedUpdate() {
        // --- Movement ---
        // The Add Relative Force uses the relative orientation of the physics object, so +Y means Forward 
        // Relative to the rotation of the object itself, rather then the world. 
        // We are ignoring Strafing movement for now  (but adding a value in the AddRelativeForce X value would strafe)
        // The if statement simply checks if we are going forwards or backwards. 
        if (moveInput.y > 0) {
            rigid2d.AddRelativeForceY(moveInput.y * forwardMoveForce);
        } else {
            rigid2d.AddRelativeForceY(moveInput.y * reverseMoveForce);
        }

        // --- Rotation --- 
        // Add Torque to rotate the object
        // In 2D, the only axis we can rotate on is Z (think of rotating AROUND that axis)
        // Note the xInput goes from (-1,+1)
        // the -1f multiplier is because by default we will rotate the wrong way. 
        rigid2d.AddTorque(moveInput.x * rotationTorque * -1f);
    }


    // --- Methods the Input System will call ---
    // Using Callbacks with the new Input System. OnMove will get called when the movement input gets changed.
    // This includes when it is released, and set back to (0,0). 
    private void HandleMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();
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
        actions.Player.Move.performed += HandleMove;
        actions.Player.Move.canceled += HandleMove; // Need to detect when we let go of the joystick as well. 
    }

    void OnDisable() {
        actions.Disable(); //disable when leaving the scene. 
        // De-Subscribe input events to the Handler Methods. 
        actions.Player.Move.performed -= HandleMove;
        actions.Player.Move.canceled -= HandleMove; // Need to detect when we let go of the joystick as well. 
    }

}
