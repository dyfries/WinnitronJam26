using UnityEngine;
using UnityEngine.Events;

// Just players a sprite animation from an array. 
// Be Careful there are lots of ways this will break if the settings aren't set up well. 
public class SimpleSpriteAnimator : MonoBehaviour {

    public UnityEvent OnCompletedPlaying = new UnityEvent();

    [Header("Component References")]
    // The animator to play it on. 
    public SpriteRenderer spriteRenderer;

    [Header("Put Frames Here - No Empty Arrays or Null entries")]
    // The Sprite frames to use (cannot be empty or have null entries). 
    public Sprite[] frames;

    [Header("Frame rate - Cannot be 0")]
    
    public float framesPerSecond = 10f;

    // Timers
    private float timer;
    private int index;

    [Header("Started controls")]
    public bool isPlaying = true;
    public bool loopPlayer = true;

    void Update() {
        if (isPlaying) {
            UpdateTimer();
        }
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
        // Errors - if the array is empty or contains null entries it will crash. 

        if(index == frames.Length - 1) {
            if (!loopPlayer) {
                isPlaying = false;
            }
            OnCompletedPlaying.Invoke();
        }

        index = (index + 1) % frames.Length; // Loops back to 0
        spriteRenderer.sprite = frames[index];

    }

    // Methods for event access
    public void SetIsPlaying(bool isPlaying) {
        // This is required otherwise our local var will be set
        // rather then our class var and it won't work and you will be very confused why. 
        this.isPlaying = isPlaying; 
    }
}
