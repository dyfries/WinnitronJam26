using UnityEngine;

public class M4_TriggerToAnimatorEG : MonoBehaviour
{
    // Store a reference to the animator
    private Animator anim; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Grab the reference. I'm using GetComponentInChildren because the Trigger/Collider is on the parent object of the Animator. 
        anim = GetComponentInChildren<Animator>();
    }

    // When the trigger is activated. 
    private void OnTriggerEnter2D(Collider2D collision) {
        // Watch for null references. 
        if (anim != null) {
            // Call the Trigger. This sets it to true. 
            anim.SetTrigger("Collected");
        }
    }

    // Just some other useful method calls for the Animator. 
    private void OtherAnimCalls() {
        anim.ResetTrigger("Collected"); // sets trigger to false. 
        anim.SetBool("BoolParameter", true); // sets bool to true (or false)
        anim.SetInteger("IntParameter", 123); // sets int to 123. 
        anim.SetFloat("FloatParameter", 1.0f); // same but for float
    }
}
