using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileSpawner : MonoBehaviour {

    public UnityEvent OnSpawnEvent = new UnityEvent();

    // I only want to spawn projectiles that have a rigidbody here. 
    public Rigidbody2D prefabReference;

    public Vector2 fireForce = new Vector2(0, 1f);
    public float fireTorque = 0f;

    public bool DEBUG_MODE = false;

    public void Fire()
    {
        // Error checking: 
        if (prefabReference == null) {
            Debug.LogWarning("Can't Spawn Prefab from " + gameObject.name + " as it has no prefab to spawn referenced");
            return; // quit method early. I don't love this technique but it has its uses. 
        }


        Rigidbody2D rigid = Instantiate<Rigidbody2D>(prefabReference, transform.position, transform.rotation, transform);
       

            // Some alternatives I didn't use: 
            // ok but this mode of force is for applying a force over time, continiously
            // rigid.AddRelativeForce(fireForce);
            // This is the same as the above. The above line has a default parameter for ForceMode.
            // The default ForceMode is Force, which means add the force over time (force/deltaTime per frame)
            // rigid.AddRelativeForce(fireForce, ForceMode2D.Force;

        // Use this ForceMode to apply the entire force this frame. 
        rigid.AddRelativeForce(fireForce, ForceMode2D.Impulse);
        rigid.AddTorque(fireTorque, ForceMode2D.Impulse);

        // On Spawn Event
        OnSpawnEvent.Invoke();

        if (DEBUG_MODE){
            Debug.Log("Spawned a Projectile: " + prefabReference.gameObject.name);
        }

    }
}
