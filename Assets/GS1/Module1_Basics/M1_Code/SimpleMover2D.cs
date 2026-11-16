
using UnityEngine;

// A simple script that moves and rotates the object its placed on. This one is setup for 2D movement and 1D rotation
public class SimpleMover2D : MonoBehaviour {

    [Header("Enable or disable movement and rotation")]
    public bool moving = true;
    public bool rotating = true;

    [Header("Movement in Units per Second")]
    public Vector2 movementSpeed = Vector2.zero;

    [Header("Rotation in degrees per second")]
    public float rotationSpeed = 0f;
	
	// Update is called once per frame
	void Update () {
        // Move at units per second
        if( moving){
            transform.Translate(movementSpeed * Time.deltaTime);
        }

        // rotate the transform by degrees per second
        if( rotating ){
            transform.Rotate(new Vector3(0,0,rotationSpeed * Time.deltaTime)); // Rotating on the Z axis (for 2D)
        }
    }

    // These can be called to explicitly set a

    // To use with events
    public void ToggleRotation(bool isRotating){
        rotating = isRotating;
    }
    // set from an event
    public void ToggleMovement(bool isMoving){
        moving = isMoving;
    }
    // Set speeds for events or scripts

    public void SetRotationSpeed(float newSpeed) {
        rotationSpeed = newSpeed;
    }
    // set from an event
    public void SetMovementSpeed(Vector2 newSpeed) {
        movementSpeed = newSpeed;
    }

}
