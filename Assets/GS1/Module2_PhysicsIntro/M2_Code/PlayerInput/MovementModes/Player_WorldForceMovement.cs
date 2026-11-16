using UnityEngine;
// Input 1. - Required for Input system
using UnityEngine.InputSystem;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
// This player just adds force in world space. 
// Use this for a top down game such as a JRPG Map view, Schmupp, or Isometric game. 
public class Player_WorldForceMovement : MonoBehaviour
{
    [Header("Asset Settings")]
    public Vector2 moveForce = new Vector2(1f, 1f); // Make a vector to handle isometric views.         

    [Header("Cached Values")]
    // shouldn't be public usually but I want the input to be visible for debugging right now. 
    public Vector2 moveInput;

    // --- Note - This is all setup for Input system, skip down to the Update loop for the rest ---
    // Input 2. - Make a variable to hold a reference to the input asset. (basically)
    InputSystem_Actions actions; // This is the default. Needs the C# Code to be generated to work. This seems like the cleanest solution to me code wise and avoids strings etc. 

    // Physics Rigidbody Reference. Hide it for now since we are enforcing it with RequireComponent
    private Rigidbody2D rigid2d;

    public Vector2 GetCurrentInput() {
        return moveInput;
    }

    // --- Methods the Input System will call ---
    // Using Callbacks with the new Input System. OnMove will get called when the movement input gets changed.
    // This includes when it is released, and set back to (0,0). 
    private void HandleMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();       
    }

    // --- Fixed Update - Where Physics Updates happen (instead of Update) ---
    private void FixedUpdate() {
        // Cache to do the multiplication. This is a pretty simple implimentation, but a good start. 
        Vector2 cachedVector = new Vector2(moveInput.x * moveForce.x, moveInput.y * moveForce.y);

        // The AddForce method will apply a force in a World Orientation, which means it will ignore the
        // rigidbody's own facing value, and a +Y value is always "Up" in the world.
        //
        // This is useful if you want to have a simple control scheme where the input directly 
        // relates to the movement in "World Space". Top down, Schmupps, Isometric games, or even side "brawlers"
        // would all use something like this. 
        //
        // Usually when using this, I will lock the rotation constraint, and
        // handle the art seperately later (to have a sprite face different directions etc)
        rigid2d.AddForce(cachedVector);
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
