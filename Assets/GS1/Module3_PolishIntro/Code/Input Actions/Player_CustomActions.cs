using UnityEngine;
using UnityEngine.InputSystem; // Input 1. - Required for Input system
using UnityEngine.Events;

public enum CurrentInputDirection {
    Left, Up, Right, Down, None
}

// This just maps the Input to a bunch of custom events you can use if you want. 
// Could extend this to support other buttons but the default ones are kind of nonsensical . 
public class Player_CustomActions : MonoBehaviour
{
    // Fires on the frame it changes to this direction, or ends. 
    public UnityEvent MoveEvent_Up = new UnityEvent();
    public UnityEvent MoveEvent_Right = new UnityEvent();
    public UnityEvent MoveEvent_Down = new UnityEvent();
    public UnityEvent MoveEvent_Left = new UnityEvent();
    public UnityEvent MoveEvent_NoInput = new UnityEvent();

    // Space by default
    public UnityEvent ButtonEvent_JumpPressed = new UnityEvent();
    public UnityEvent ButtonEvent_JumpReleased = new UnityEvent();

    // Enter by default
    public UnityEvent ButtonEvent_AttackPressed = new UnityEvent();
    public UnityEvent ButtonEvent_AttackReleased = new UnityEvent();

    // Input 2. - Make a variable to hold a reference to the input asset. (basically)
    InputSystem_Actions actions; // This is the default.

    // Just for visualizatio/demonstration right now anyways
    // Should be private but public is useful for debugging rn. It'll get overwritten anyways. 
    public Vector2 input_MoveInput;
//    public Vector2 input_LookInput; // Currently not using keyboard in put, but supports right stick. If you wanted a true dual stick experience, yo might want to update the mappings. 

    public bool input_JumpButton = false;
    public bool input_AttackButton = false;

    public float joystickDeadzone = 0.01f; // Actual settings in input, this is jump my threshold for changing input values. 

    // Can probably be replaced by the enum

    private CurrentInputDirection currentInputDirection = CurrentInputDirection.None;// check if we changed directions since last frame, which triggers different events. 

    public bool DEBUG_MODE = false;

    // This is ugly, but will get called before the update loop so should work ok even for input held. 
    private void HandleMove(InputAction.CallbackContext context) {
        input_MoveInput = context.ReadValue<Vector2>();

        CurrentInputDirection nextInputDirection;

        // Is it firing at all? 
        if (input_MoveInput.magnitude > joystickDeadzone) { // i know sqaure mag is faster but who cares rn. You can improve the optimization by caching the joystick deadzones square root if you really feel like it. 

            // Two things to check. 
            // Start Firing in dominant direction (for input changing)
            if (Mathf.Abs(input_MoveInput.x ) > Mathf.Abs(input_MoveInput.y)) {
                // X is larger (absolute value)
                if(input_MoveInput.x > 0) {
                    nextInputDirection = CurrentInputDirection.Right;
                } else {
                    nextInputDirection = CurrentInputDirection.Left;
                }
            } else {
                // Y is larger. 
                if(input_MoveInput.y > 0) {
                    nextInputDirection = CurrentInputDirection.Up;
                } else {
                    nextInputDirection = CurrentInputDirection.Down;
                }
            }
        } else {
            // No significant movement input. 
            nextInputDirection = CurrentInputDirection.None;
        }

        if (DEBUG_MODE) {
            Debug.Log("Current Move Direction: " + input_MoveInput + " for state: " + nextInputDirection.ToString());
        }
   

        // Call all events and check if we are changing directions. 
        // This will be some spaghetti just because each direction needs its own event call. 
        // Did we change direction? 
        if (nextInputDirection == currentInputDirection) {
            // continue firing could go here if we need them. 
        } else {
            // Input changed.  
            if (nextInputDirection == CurrentInputDirection.Left) {
                MoveEvent_Left.Invoke();
            } else if (nextInputDirection == CurrentInputDirection.Right) {
                MoveEvent_Right.Invoke();
            } else if (nextInputDirection == CurrentInputDirection.Up) {
                MoveEvent_Up.Invoke();
            } else if (nextInputDirection == CurrentInputDirection.Down) {
                MoveEvent_Down.Invoke();
            } else if (nextInputDirection == CurrentInputDirection.None) {
                MoveEvent_NoInput.Invoke();
            } else {
                Debug.LogError("Shouldn't reach this point");
            }

            currentInputDirection = nextInputDirection;
        }
    }
/*
    private void HandleLook(InputAction.CallbackContext context) {
        input_LookInput = context.ReadValue<Vector2>();
    }
*/
    private void HandleJumpPressed(InputAction.CallbackContext context) {
        input_JumpButton = true;
        ButtonEvent_JumpPressed.Invoke();
    }

    private void HandleJumpReleased(InputAction.CallbackContext context) {
        input_JumpButton = false;
        ButtonEvent_JumpReleased.Invoke();
    }

    // Attack Buttons (Enter)
    private void HandleAttackPressed(InputAction.CallbackContext context) {
        input_AttackButton = true;
        ButtonEvent_AttackPressed.Invoke();
    }

    private void HandleAttackReleased(InputAction.CallbackContext context) {
        input_AttackButton = false;
        ButtonEvent_AttackReleased.Invoke();
    }

    private void Awake() {
        actions = new InputSystem_Actions();
    }

    // Input 4. Enable and disable actions when entering and leaving scene. 
    void OnEnable() {
        actions.Enable();    // enable actions. 
        // Subscribe input events to the Handler Methods. 
        actions.Player.Move.performed += HandleMove;
        actions.Player.Move.canceled += HandleMove; // Need to detect when we let go of the joystick as well. 
        /*
        // Common Look actions (second joystick etc) - Right now no keyboard support so might want to remap arrow keys if you use this. 
        actions.Player.Look.performed += HandleLook;
        actions.Player.Look.canceled += HandleLook;
        */
        // --- Button Presses ---
        // Jump (Default to space)
        actions.Player.Jump.started += HandleJumpPressed;
        actions.Player.Jump.canceled += HandleJumpReleased;
        // Attack (Default to enter)
        actions.Player.Attack.started += HandleAttackPressed;
        actions.Player.Attack.canceled += HandleAttackReleased;

    }

    void OnDisable() {
        actions.Disable(); //disable when leaving the scene. 
        // De-Subscribe input events to the Handler Methods. 
        actions.Player.Move.performed -= HandleMove;
        actions.Player.Move.canceled -= HandleMove; // Need to detect when we let go of the joystick as well. 
        /*
        // Look action (second Stick)
        actions.Player.Look.performed -= HandleLook;
        actions.Player.Look.canceled -= HandleLook;
        */
        // Jump Button 
        actions.Player.Jump.started -= HandleJumpPressed;
        actions.Player.Jump.canceled -= HandleJumpReleased;
        // Attack
        actions.Player.Attack.started -= HandleAttackPressed;
        actions.Player.Attack.canceled -= HandleAttackReleased;

    }
}
