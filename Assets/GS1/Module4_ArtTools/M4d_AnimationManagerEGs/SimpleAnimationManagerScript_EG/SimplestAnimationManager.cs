using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class SimplestAnimationController : MonoBehaviour
{
    // Needs a reference to the AnimationController/Animator
    [Header("Reference to the Animator")]
    public Animator animator;

    [Header("Cached Input - Should be private. I just made them public so you could observe the input. ")]
    // Old input system
    // public float hInput = 0;
    // public float vInput = 0;
    public Vector2 moveInput; // new input
    public bool isButtonDownThisFrame = false;

    // Input 2. - Make a variable to hold a reference to the input asset. (basically)
    private InputSystem_Actions actions; // This is the default. Needs the C# Code to be generated to work. This seems like the cleanest solution to me code wise and avoids strings etc. 


    // Start is called before the first frame update
    void Start()
    {
        // grab a reference, if we don't have one already
        if(animator == null) {
            animator = GetComponent<Animator>();
        }

        if (animator == null) {
            Debug.LogError("Expecting an AnimationController but none found. This should theoretically never be called because of the RequireComponent");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Get the input - OLD STYLE
       // hInput = Input.GetAxis("Horizontal");
       // vInput = Input.GetAxis("Vertical");

        // The string must match exactly the String in the Animator. 
        animator.SetFloat("hInput", moveInput.x);
        animator.SetFloat("vInput", moveInput.y);

        // Button Input - One Press (not as useful with Bools, but good for Triggers)
        // if (Input.GetButtonDown("Jump")) { OLD
        if (isButtonDownThisFrame) { 
            animator.SetBool("isButtonDownThisFrame", true); // remember to change this back to false otherwise. 
            animator.SetTrigger("isButtonDown_Trigger"); // Doesn't need the bool as it only turns true. 
            isButtonDownThisFrame = false; // Reset this
        } else {
            animator.SetBool("isButtonDownThisFrame", false);
        }
    }

    // New Imput System
    // --- Methods the Input System will call ---
    // Using Callbacks with the new Input System. OnMove will get called when the movement input gets changed.
    // This includes when it is released, and set back to (0,0). 
    private void HandleMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();
    }

    private void HandleJump(InputAction.CallbackContext context) {
        Debug.Log("Handle Jump");
        if (context.performed) {
            isButtonDownThisFrame = true;
        }

        /* If you want to test or debug this yourself. 
        if (context.started) Debug.Log("STARTED fired");
        if (context.performed) Debug.Log("PERFORMED fired");
        if (context.canceled) Debug.Log("CANCELED fired");
        */
    }

    // --- Input System Setup ---
    // Input 3. Get a reference in Awake
    void Awake() {
        // Get Rigidbody Reference. Since we hid it as private we'll need to grab it here. 
        actions = new InputSystem_Actions(); // Not an object, so we are creating a new wrapper. 
    }

    // Input 4. Enable and disable actions when entering and leaving scene. 
    void OnEnable() {
        actions.Enable();    // enable actions. 
        // Subscribe input events to the Handler Methods. 
        actions.Player.Move.performed += HandleMove;
        actions.Player.Move.canceled += HandleMove; // Need to detect when we let go of the joystick as well. 
        // jump
        actions.Player.Jump.performed += HandleJump; 
    }

    void OnDisable() {
        actions.Disable(); //disable when leaving the scene. 
        // De-Subscribe input events to the Handler Methods. 
        actions.Player.Move.performed -= HandleMove;
        actions.Player.Move.canceled -= HandleMove; // Need to detect when we let go of the joystick as well. 
        // Jumping
        actions.Player.Jump.performed -= HandleJump;
    }

}
