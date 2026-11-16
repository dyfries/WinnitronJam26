using UnityEngine;

public class PlayerPrototype1 : MonoBehaviour
{
    public Animator anim;

    // Animation Strings
    public string animMovingString = "IsMoving";

    Vector2 inputMove;
    public bool inputAttack;

    public void Update()
    {
        inputMove = new Vector2( Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        inputAttack = Input.GetButton("Jump");

        if( inputMove.magnitude > 0.1f)
        {
            anim.SetBool("IsMoving", true);
        }
        else
        {
            anim.SetBool(animMovingString, false);
        }

        anim.SetBool("IsAttacking", inputAttack);
        
    }
}
