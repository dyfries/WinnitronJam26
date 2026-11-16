using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ForceOrientation {
    WorldSpace,
    RelativeSpace
}

public class SimpleAddForce : MonoBehaviour
{
    // comments
    public ForceMode2D forceMode = ForceMode2D.Force;
    public ForceOrientation orientation = ForceOrientation.WorldSpace;

    public Rigidbody2D rigid;

    public Vector2 forceToApply = Vector2.up; // same as new Vector2(0,1f);

    public bool applyForceOnStart = true;
    public bool currentlyApplyingForce = false;

    // Start is called before the first frame update
    void Start()
    {
        if (applyForceOnStart) {
            
            ApplyForce();
        }
    }

    // Helper Methods for using with Events
    public void StopApplyingForce() {
        currentlyApplyingForce = false;
    }

    // Simple Wrapper methods for use with Events. 
    public void ChangeForceAppliedX(float x) {
        forceToApply.x = x;
    }
    public void ChangeForceAppliedY(float y) {
        forceToApply.y = y;
    }

    // This won't show up in Events so I'll have to add individual X and Y versions. 
    public void ChangeForceApplied(Vector2 newForceToApply) {
        forceToApply = newForceToApply;
    }

    public void ApplyForce() {

        // Depending on the forcemode, we will either apply the force once (impulse) or over time. 
        if (forceMode == ForceMode2D.Force) {
            // We will apply this in the Fixed Update Loop as Force is applied over time. 
            currentlyApplyingForce = true;

            // If we have an impulse it will get added all at once. 
        } else if(forceMode == ForceMode2D.Impulse){

            if (orientation == ForceOrientation.WorldSpace) {
                rigid.AddForce(forceToApply, ForceMode2D.Impulse); // Apply all the force at once
            } else if(orientation == ForceOrientation.RelativeSpace){
                // We are in relative space if not in World Space
                rigid.AddRelativeForce(forceToApply, ForceMode2D.Impulse);
            } 
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentlyApplyingForce) {
            if (forceMode == ForceMode2D.Force) {
                if (orientation == ForceOrientation.WorldSpace) {

                    rigid.AddForce(forceToApply, ForceMode2D.Force); // Apply all the force at once

                } else if (orientation == ForceOrientation.RelativeSpace) {

                    // We are in relative space if not in World Space
                    // Note: Because teh ForceMode2D.Force is the default value of the method, we could 
                    // just have used rigid.AddRelativeForce(forceToApply); // here but I wanted to make it
                    // Explicit so you were aware what was going on. 
                    rigid.AddRelativeForce(forceToApply, ForceMode2D.Force);
                }
            }
        }
    }



}
