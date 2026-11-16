using UnityEngine;
// Input 1. - Required for Input system
using UnityEngine.InputSystem;

public class FirstInput_Translate : MonoBehaviour
{
    [Header("Asset Settings")]
    public float moveSpeed = 1f; // Custom variable to provide speed. I gave it a reasonable value of 1 to start.
                                 // Pro Tip: make your variables work by default (if I left the 1 out, it would be 0 and would not move). 


    // --- Note - This is all setup for Input system, skip down to the Update loop for the rest ---
    // Input 2. - Make a variable to hold a reference to the input asset. (basically)
    InputSystem_Actions actions; // This is the default. Needs the C# Code to be generated to work. If this is throwing errors, check the "Generate C# files" box on your InputSystem_Actions file. 

    // Input 3. Get a reference in Awake
    void Awake() {
        actions = new InputSystem_Actions(); // Not an object, so we are creating a new wrapper. 

    }
    // Input 4. Enable and disable actions when entering and leaving scene. 
    void OnEnable() {
        actions.Enable();    // enable actions. 
    }

    void OnDisable() {
        actions.Disable(); //disable when leaving the scene. 
    }

    // Update is called once per frame
    void Update()
    {
        // Most simple use of input. 
        // Input 5. Read the input. 
        Vector2 inputDirection = actions.Player.Move.ReadValue<Vector2>(); // Reading the directional Move input as a 2D Vector. 
        bool jumpButtonPressed = actions.Player.Jump.WasPressedThisFrame(); // Just for example, doesn't do anything yet. For buttons, there are a few options (WasPressed, WasPressedThisFrame, WasReleasedThisFrame, etc). 

        // Move the Transform. 
        Vector3 movement = inputDirection; // Looks weird to go from Vector2 to Vector 3 but unity can do the conversion. The Transform needs a 3D input. The Z value will be 0. 
        // transform.Translate(movement); // this would go too fast, so we multiply it by the frame time (1/ FPS) - This would move N units per frame. 
        movement = movement * moveSpeed; // adjustable speed
        movement = movement * Time.deltaTime; // converts from units per frame to units per second. 
        transform.Translate(movement); // Move n units per second now. 
    }
}
