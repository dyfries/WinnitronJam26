using UnityEngine;
using UnityEngine.Events;

// Used for simple counting tasks. 
public class Twiddle_Counter : MonoBehaviour
{
    public UnityEvent OnReachedTargetValue = new UnityEvent();
    public UnityEvent OnGreaterThenTargetValue = new UnityEvent();
    public UnityEvent OnLessThenTargetValue = new UnityEvent();
    public UnityEvent CountChanged = new UnityEvent();

    public int startingCountValue = 0;
    public int targetValue = 5;

    private int currentCountValue = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentCountValue = startingCountValue;
    }

    // Add a negative number to subtract
    public void AddValue(int value) {
        currentCountValue += value;

        // Check for events
        if (currentCountValue > targetValue) {
            OnGreaterThenTargetValue.Invoke();
        }

        if (currentCountValue < startingCountValue) {
            OnLessThenTargetValue.Invoke();
        }

        if(currentCountValue == targetValue) {
            OnReachedTargetValue.Invoke();
        }
    }
}
