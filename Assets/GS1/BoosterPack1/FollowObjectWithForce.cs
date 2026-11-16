using UnityEngine;

public class FollowObjectWithForce : MonoBehaviour
{
    public Transform target;

    public float forceAmount = 1f;

    [Header("Needs A Rigidbody")]
    public Rigidbody2D rigid;

    [Header("Just for info don't adjust these")]
    Vector2 targetVector;
    Vector2 normalizedTargetVector;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(target == null)
        {
            Debug.LogWarning("FollowObjectWithForce Needs a follow target");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        targetVector = target.position - transform.position;
        normalizedTargetVector = targetVector.normalized;

        // Add force in direction
        rigid.AddForce(normalizedTargetVector * forceAmount);

    }
}
