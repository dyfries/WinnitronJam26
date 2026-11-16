using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimpleAddTorque : MonoBehaviour
{
    // comments
    public ForceMode2D forceMode = ForceMode2D.Force;

    public Rigidbody2D rigid;

    public float torqueToApply = 1f;

    public bool applyTorqueOnStart = true;
    public bool currentlyApplyingTorque = false;

    // Start is called before the first frame update
    void Start()
    {
        if (applyTorqueOnStart) {

            ApplyTorque();
        }
    }

    public void ApplyTorque() {

        // Depending on the forcemode, we will either apply the Torque once (impulse) or over time. 
        if (forceMode == ForceMode2D.Force) {
            // We will apply this in the Fixed Update Loop as Torque is applied over time. 
            currentlyApplyingTorque = true;

            // If we have an impulse it will get added all at once. 
        } else if(forceMode == ForceMode2D.Impulse){

            rigid.AddTorque(torqueToApply, ForceMode2D.Impulse); // Apply all the force at once

        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentlyApplyingTorque) {
            if (forceMode == ForceMode2D.Force) {
                rigid.AddTorque(torqueToApply, ForceMode2D.Force); // Apply all the force at once
            }
        }
    }

    // Helper Methods for using with Events
    public void StopApplyingTorque() {
        currentlyApplyingTorque = false;
    }

    public void ChangeTorqueApplied(float newTorqueToApply) {
        torqueToApply = newTorqueToApply; 
    }

}
