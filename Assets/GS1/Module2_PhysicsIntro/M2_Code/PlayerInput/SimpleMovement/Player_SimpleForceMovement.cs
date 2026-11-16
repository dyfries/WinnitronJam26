using UnityEngine;
// Input 1. - Required for Input system
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
// This Player system just includes Left and Right forces, with a Jump button to add an impulse force. 
// Note this is very simple and doesn't ground check the jump or anything else like that. 
public class Player_SimpleForceMovement : MonoBehaviour
{
    [Header("Asset Settings")]
    public float moveForce = 1f; // Custom variable to provide speed. I gave it a reasonable value of 1 to start.
                                 // Pro Tip: make your variables work by default (if I left the 1 out, it would be 0 and would not move). 
    public float jumpForce = 1f; 

    [Header("Cached Values")]
    // shouldn't be public usually but I want the input to be visible for debugging right now. 
    public Vector2 moveInput;
    public bool requestToJump = false;

    // --- Note - This is all setup for Input system, skip down to the Update loop for the rest ---
    // Input 2. - Make a variable to hold a reference to the input asset. (basically)
    InputSystem_Actions actions; // This is the default. Needs the C# Code to be generated to work. This seems like the cleanest solution to me code wise and avoids strings etc. 

    // Physics Rigidbody Reference. Hide it for now since we are enforcing it with RequireComponent
    private Rigidbody2D rigid2d;


    // --- Methods the Input System will call ---
    // Using Callbacks with the new Input System. OnMove will get called when the movement input gets changed.
    // This includes when it is released, and set back to (0,0). 
    private void HandleMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();       
    }

    private void HandleJump(InputAction.CallbackContext context) {
        requestToJump = true; // don't need to read the value here (in fact reading the value will crash). 
    }

    // --- Fixed Update - Where Physics Updates happen (instead of Update) ---
    private void FixedUpdate() {
        // movement
        rigid2d.AddForceX(moveInput.x * moveForce);

        // Jumping
        if (requestToJump) {
            rigid2d.AddForceY(jumpForce, ForceMode2D.Impulse);
            requestToJump = false; // reset the flag. 
        }
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
        actions.Player.Jump.performed += HandleJump;
    }

    void OnDisable() {
        actions.Disable(); //disable when leaving the scene. 
        // De-Subscribe input events to the Handler Methods. 
        actions.Player.Move.performed -= HandleMove;
        actions.Player.Move.canceled -= HandleMove; // Need to detect when we let go of the joystick as well. 
        actions.Player.Jump.performed -= HandleJump;
    }

}
