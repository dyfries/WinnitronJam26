using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicTriggerListener : MonoBehaviour
{
    // Custom Events to add functionality. 
    public UnityEvent OnTriggerEnter2D_Event = new UnityEvent();
    public UnityEvent OnTriggerExit2D_Event = new UnityEvent();
    public UnityEvent OnTriggerStay2D_Event = new UnityEvent();

    // --- Additional Reading and reference ---
    // 2D Collider Docs - Triggers are still colliders. 
    // https://docs.unity3d.com/ScriptReference/Collider2D.html

    // OnTriggerEnter
    // https://docs.unity3d.com/ScriptReference/Collider.OnTriggerEnter.html

    public bool DEBUG_MODE = false; // If True print debug messages. 

    // Similar to Collision listener but for Triggers. 
    // Note: This is the other Collider2D object, rather then a Collision object. (So we don't have as much collision data available). 
    void OnTriggerEnter2D(Collider2D collision){
        OnTriggerEnter2D_Event.Invoke();

        if ( DEBUG_MODE)
            Debug.Log(this.gameObject.name + " OnTriggerEnter2D event called from collision with " + collision.gameObject.name);
    }

    void OnTriggerExit2D(Collider2D collision) {
        OnTriggerExit2D_Event.Invoke();

        if (DEBUG_MODE)
            Debug.Log(this.gameObject.name + " OnTriggerExit2D event called from collision with " + collision.gameObject.name);
    }

    void OnTriggerStay2D(Collider2D collision) {
        OnTriggerStay2D_Event.Invoke();

        if (DEBUG_MODE)
            Debug.Log(this.gameObject.name + " OnTriggerStay2D event called from collision with " + collision.gameObject.name);
    }
}
