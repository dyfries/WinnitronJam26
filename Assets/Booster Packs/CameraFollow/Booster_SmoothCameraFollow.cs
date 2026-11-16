using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum CameraFollowMode {
    Locked, // No smoothing
    Lerp,  // Uses Lerp
    SmoothDamp // Uses smoothdamp 
}

// A dynamic camera follow with boundaries. 
public class Booster_SmoothCameraFollow : MonoBehaviour
{
    [Header("Boundary Settings")]
    public bool useFixedBoundary = true;
    // needs to be set wider for last room
    [Header("The minimum and Maximum X Axis position")]
    public float xAxisMiniumumBoundary = 0;
    public float xAxisMaximumBoundary = 10f;

    [Header("The minimum and Maximum Y Axis position")]
    public float yAxisMiniumumBoundary = 0;
    public float yAxisMaximumBoundary = 10f;

    [Header("Assign a follow target here")]
    public Transform followTarget;

    [Header(" --- Settings --- ")]
    // Note the setings are for one mode or the others. Make sure you have the right mode selected. 
    public CameraFollowMode followMode = CameraFollowMode.Locked;

    // Follow the target if this is true. 
    public bool activatedFollow = true;

    [Header("Lerp Speed (For Lerp Mode Only)")]
    [Header("Note: A Lerp value of 1 is the default. 0 it won't move.")]
    // Lerp Documentation : https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Vector2.Lerp.html
    public float lerpSpeed = 1f;

    [Header("SmoothDamp Speed (For Smoothdamp Mode Only)")]
    // See Unity Docs for full info
    // https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Vector2.SmoothDamp.html
    private Vector2 smoothDampVelocity; // This will be passed by reference in to the SmoothDamp method
    // aka it will be updated by the method, you shouldn't set it directly. 

    [Header("Approximate time to reach the target. Smaller is faster.")]
    [Range(0f,10f)]
    public float smoothTime = 1f; // Approximately the time it will take to reach the target. A smaller value will reach the target faster.

    [Header("Optionally allows you to clamp the maximum speed.")]
    public float smoothMaxTime = Mathf.Infinity; 


    [Header("Display Debugging Visualization")]
    public bool DEBUG_MODE = false;

    // Camera Z Offset. For 2D games should always be -10 (usually)
    private float zOffset = -10f;
    private Vector3 targetPosition; // Caching this so we don't create 200 vectors every frame

    // Start is called before the first frame update
    void Start()
    {
        if(followTarget == null) {
            Debug.LogError("Requires a target reference to work! Disabling follow for now. ");
            activatedFollow = false;
        }

    }
    /*
    public void SetXMaxBounds(float x){
        maxCamBounds.x = x;
    }

    public void SetYMaxBounds(float y) {
        maxCamBounds.y = y;
    }*/

    // Update is called once per frame
    void FixedUpdate()
    {
        if (activatedFollow) {

            if (DEBUG_MODE) {
                Vector2 path = followTarget.position - transform.position;
                Debug.DrawRay(transform.position, path, Color.yellow); // for debugging; 
            }

            if (followMode == CameraFollowMode.Locked) {

                targetPosition = new Vector3(followTarget.position.x, followTarget.position.y, zOffset);
                targetPosition = CheckBounds(targetPosition); // this will check the boundary (if its set) and update the position to be locked in the range provided. 
                transform.position = targetPosition;

            } else if (followMode == CameraFollowMode.Lerp) {

                float lerpValue = Time.deltaTime * lerpSpeed;
                lerpValue = Mathf.Clamp01(lerpValue); // note 0 won't work and 1 will be the same as following directly. 

                Vector2 lerpPath = Vector2.Lerp(transform.position, followTarget.position, Time.deltaTime * lerpSpeed);

                targetPosition = new Vector3(lerpPath.x, lerpPath.y, zOffset);
                transform.position = CheckBounds(targetPosition);

                // Still need to do this .
            } else if (followMode == CameraFollowMode.SmoothDamp) {

                // The Ref here means we are passing the variable by reference, which means we are passing the pointer, rather thne the value. 
                // SmoothDamp method will acutally update the value of it, so 
                Vector2 smoothDamp = Vector2.SmoothDamp(transform.position, followTarget.position, ref smoothDampVelocity, smoothTime, smoothMaxTime);

                if (DEBUG_MODE) { Debug.DrawRay(transform.position, smoothDampVelocity, Color.burlywood); }

                targetPosition = new Vector3(smoothDamp.x, smoothDamp.y, zOffset);
                transform.position = CheckBounds(targetPosition);
            }
            
        }
    }

    // Boundary check forces the value to stay within the boundary guideslines. Note I'm not checking that Min < Max but it always should be. 
    public Vector3 CheckBounds(Vector3 targetPosition) {

        if (useFixedBoundary) {

            // I'm not checking that the min is < then the max here, so I'll check them both separately. 
            if(targetPosition.x < xAxisMiniumumBoundary) {
                // Debugging before we override the value. 
                if (DEBUG_MODE) { Debug.Log("Hitting the Left Boundary " + xAxisMiniumumBoundary + " at position : " + targetPosition.x); }
                targetPosition.x = xAxisMiniumumBoundary;
                
            }

            if (targetPosition.x > xAxisMaximumBoundary) {
                // Debugging. Needs to be before it is updated. 
                if (DEBUG_MODE) { Debug.Log("Hitting the Right Boundary " + xAxisMaximumBoundary + " at position : " + targetPosition.x); }
                // Right boundary. 
                targetPosition.x = xAxisMaximumBoundary;
            }

            // Bottom (Min Y)
            if (targetPosition.y < yAxisMiniumumBoundary) {
                // Debugging before we override the value. 
                if (DEBUG_MODE) { Debug.Log("Hitting the Bottom Boundary " + yAxisMiniumumBoundary + " at position : " + targetPosition.x); }
                targetPosition.y = yAxisMiniumumBoundary;

            }

            if (targetPosition.y > yAxisMaximumBoundary) {
                // Debugging. Needs to be before it is updated. 
                if (DEBUG_MODE) { Debug.Log("Hitting the Top Boundary " + yAxisMaximumBoundary + " at position : " + targetPosition.y); }
                // Top
                targetPosition.y = yAxisMaximumBoundary;
            }

        }
        return targetPosition;
    }
}
