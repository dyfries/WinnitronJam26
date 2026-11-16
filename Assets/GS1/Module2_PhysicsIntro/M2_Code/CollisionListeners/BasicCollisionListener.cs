using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/*
 * Introduction to listening to collisions
 * Uses Events to activate when collided with. 
 */ 
public class BasicCollisionListener : MonoBehaviour
{
    // Custom Events to add functionality. 
    public UnityEvent OnCollisionEnter2D_Event = new UnityEvent();
    public UnityEvent OnCollisionExit2D_Event = new UnityEvent();
    public UnityEvent OnCollisionStay2D_Event = new UnityEvent();


    // --- Additional Reading and reference ---
    // 2D Collider Docs
    // https://docs.unity3d.com/ScriptReference/Collider2D.html

    // Collision Object returned from Collision (passed as parameter)
    // https://docs.unity3d.com/ScriptReference/Collision2D.html

    // Collision Event
    // https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionEnter2D.html

    public bool DEBUG_MODE = false; // If True print debug messages. 


    void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollisionEnter2D_Event.Invoke(); // Calls Custom Collision Event

        if (DEBUG_MODE)
            Debug.Log(collision.otherCollider.gameObject.name + " OnCollisionEnter2D_Event event called from collision with " + collision.collider.gameObject.name);
    }

    // Fired once on the frame the collision ends
    private void OnCollisionExit2D(Collision2D collision) {
        OnCollisionExit2D_Event.Invoke();

        if (DEBUG_MODE)
            Debug.Log(collision.otherCollider.gameObject.name + " OnCollisionExit2D event called from collision with " + collision.collider.gameObject.name);
    }

    // Fired every frame that the collision occurs, usually overkill but I included it here for completeness. . 
    void OnCollisionStay2D(Collision2D collision) {
        OnCollisionStay2D_Event.Invoke();


        if (DEBUG_MODE)
            Debug.Log(collision.otherCollider.gameObject.name + " OnCollisionStay2D event called from collision with " + collision.collider.gameObject.name);

    }
}
