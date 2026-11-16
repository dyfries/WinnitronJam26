using UnityEngine;
using UnityEngine.Events;

// Just useful if you are setting up the same event sequences many time. 
// Call the Call Event method to call the event. Thats all. useful if you have many listeners. 
public class Twiddle_EventCaller : MonoBehaviour
{
    public UnityEvent CustomUnityEvent = new UnityEvent();

    public bool callEventOnStart = false;

    public void CallEvent() {
        CustomUnityEvent.Invoke();
    }

    public void Start() {
        if (callEventOnStart) {
            CallEvent();
        }
    }
}
