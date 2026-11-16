using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

// Call a random event. You MUST set this up in the editor. 
public class Twiddle_RandomEvent : MonoBehaviour
{
    // Actually can't believe this will work, but the inspector will create the objects when you add them so it *should* be ok. 
    public UnityEvent[] eventsToCall;

    public void Start() {
       
    }

    public void CallRandom() {
        if(eventsToCall != null && eventsToCall.Length > 0) {
            int randomIndex = Random.Range(0, eventsToCall.Length);

            // Checks the specific event is not null, shouldn't be since it was set in the editor. 
            if (eventsToCall[randomIndex] != null) {
                // call the random event. 
                eventsToCall[randomIndex].Invoke();
            } else {
                Debug.LogWarning("The Specific Event you tried to call was null at index =  " + randomIndex);
            }

        } else {
            Debug.LogWarning("No Events to Call");
        }

    }
}
