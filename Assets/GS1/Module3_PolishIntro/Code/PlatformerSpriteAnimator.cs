using UnityEngine;

// A script to manually play the animations for a Platformer character. 

// This will be replaced with an Animation Controller later, but for now it'll work just fine. 
// Be Careful there are lots of ways this will break if the settings aren't set up well. Every array needs to have at least one element, and no spaces can be null. 

// Hopefully will make you appreciate why we use the Animator and Clips later on, this is bit of a pain. 
// Conceptually, this is basically what the Animation system will do later on though. 

// Enums are basically an integer with a wrapper around it but useful for situations like this where we only want to be in one state at a time. 
public enum PlatformerState {
    Idle,Run,Jump,Fall,Hit 
}
public class PlatformerSpriteAnimator : MonoBehaviour {
    [Header("Component References")]
    // The animator to play it on. 
    public SpriteRenderer spriteRenderer;

    [Header("Put Frames Here - No Empty Arrays or Null entries")]
    // The Sprite frames to use (cannot be empty or have null entries). 
    public Sprite[] idleFrames;
    public Sprite[] runFrames;
    public Sprite[] jumpFrames;
    public Sprite[] fallFrames;
    public Sprite[] hitFrames;

    private Sprite[] currentFrames; // we will cache a reference to the array to make counting easier. 

    // Store our current state. 
    public PlatformerState currentState = PlatformerState.Idle; 

    [Header("Frame rate - Cannot be 0")]
    
    public float framesPerSecond = 10f;

    // Timers
    private float timer;
    private int index;

    [Header("Started controls")]
    public bool isPlaying = true;
    public bool loopAnimation = true;

    // --- Public Methods ---
    // You can call these from events or other code. 

    // Methods for event access
    public void SetIsPlaying(bool isPlaying) {
        // This is required otherwise our local var will be set
        // rather then our class var and it won't work and you will be very confused why. 
        this.isPlaying = isPlaying;
    }

    // Set current state. 
    public void SetCurrentStateToIdle() {
        SetCurrentState(PlatformerState.Idle);
    }

    public void SetCurrentStateToRun() {
        SetCurrentState(PlatformerState.Run);
    }

    public void SetCurrentStateToJump() {
        SetCurrentState(PlatformerState.Jump);
    }

    public void SetCurrentStateToFall() {
        SetCurrentState(PlatformerState.Fall);
    }

    public void SetCurrentStateToHit() {
        SetCurrentState(PlatformerState.Hit);
    }


    // Annoyingly, can't ve called by events, so I'll create access methods for each type. 
    public void SetCurrentState(PlatformerState state) {
        // ignore changing to the same state we are in currently. 
        if(currentState != state) {
            currentState = state;
            currentFrames = GetArrayByState();
            index = 0;
        }
    }

    // --- Private Access Methods ---

    void Update() {
        if (isPlaying) {
            UpdateTimer();
        }
    }

    private void Start() {
        // Get first reference for currentFrames
        currentFrames = GetArrayByState();
    }

    // Update and check the timer. 
    private void UpdateTimer() {
        timer += Time.deltaTime;
        if (timer >= (1f / framesPerSecond)) { /// If framesperSecond == 0 this will crash hard. Not very efficient either to divide every frame. 
            timer = 0f;

            UpdateAnimation();
        }
    }

    private void UpdateAnimation() {

        index = (index + 1) % currentFrames.Length; // Loops back to 0
        spriteRenderer.sprite = currentFrames[index];

    }

    // This is pretty ugly code but more useful to have it in one location. 
    
    private Sprite[] GetArrayByState() {

        Sprite[] tempArray = null;

        if (currentState == PlatformerState.Idle) {
            tempArray = idleFrames;
        } else if (currentState == PlatformerState.Run) {
            tempArray = runFrames;
        } else if (currentState == PlatformerState.Jump) {
            tempArray = jumpFrames;
        } else if (currentState == PlatformerState.Fall) {
            tempArray = fallFrames;
        } else if (currentState == PlatformerState.Hit) {
            tempArray = hitFrames;
        } else {
            Debug.LogError("Illegal state found");
        }

        // using temp, I can do some error checking 
        // All of these will make the editor crash, so I'm throwing and error first to let you know what the problem was. 
        if (tempArray == null) {
            Debug.LogError("Trying to Grab State: " + currentState.ToString() + " But the array is null");
        } else {
            if (tempArray.Length < 1) {
                Debug.LogError("Trying to Grab State: " + currentState.ToString() + " But the array is Empty");
            } else {
                for (int i = 0; i < tempArray.Length; i++) {
                    if (tempArray[i] == null) {
                        Debug.LogError("Trying to Grab State: " + currentState.ToString() + " But the array has a null entry");
                    }
                }
            }
        }

        return tempArray; 
    }

}
