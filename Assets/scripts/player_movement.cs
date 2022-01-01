using UnityEngine;

public class player_movement : MonoBehaviour
{
    
    public float movespeed = 5f; //speed player moves should be changeable by items

    public Rigidbody2D rb; //allows linking to rigid body of the player for physics
    public Animator animator;
    
    Vector2 movement;

    // Update is called once per frame
    void Update()
    {
        //inputs
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;
        //set animator variables
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    void FixedUpdate()
    {
        //movment
        rb.MovePosition(rb.position + movement * movespeed * Time.fixedDeltaTime);
    }
}
