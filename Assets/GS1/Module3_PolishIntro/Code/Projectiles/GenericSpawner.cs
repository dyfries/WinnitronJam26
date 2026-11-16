using UnityEngine;
using UnityEngine.Events;

public class GenericSpawner : MonoBehaviour
{
    // Object Spawned Event
    public UnityEvent ObjectSpawned = new UnityEvent();

    [Header("SpawnOptions")]
    public GameObject prefabToSpawn;
    
    public void SpawnObject() {
        Instantiate(prefabToSpawn, transform.position, transform.rotation);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
