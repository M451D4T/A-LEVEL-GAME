using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movespeed = 30f; //speed player moves should be changeable by items

    //linking of component to script
    public Rigidbody2D rb; 
    public Animator animator;
    public BoxCollider2D bc;
    //variables
    private RaycastHit2D hitx;
    private RaycastHit2D hity;
    private Vector2 movement;
    private bool isdamaged;
    private bool isidle;
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
        animator.SetBool("hit", isdamaged);
        animator.SetBool("idle", isidle);    
    }

    void FixedUpdate()
    {
        //make sure we can move in x direction by casting a box there first if the box is null allows movement
        hitx = Physics2D.BoxCast(transform.position, bc.size, 0, new Vector2(movement.x,0), movement.x*Time.fixedDeltaTime, LayerMask.GetMask("blocking","actor"));
        //make sure we can move in y direction by casting a box there first if the box is null allows movement
        hity = Physics2D.BoxCast(transform.position, bc.size, 0, new Vector2(0,movement.y), movement.y*Time.fixedDeltaTime, LayerMask.GetMask("blocking","actor"));
        if (hity.collider==null && hitx.collider==null)
        {
            //movment happens
            rb.MovePosition(rb.position + movement * movespeed * Time.fixedDeltaTime);
        }

        //check if player should be idle
         if (movement.x == 0 && movement.y == 0)
         {
             isidle = true;
         }
         else
         {
             isidle = false;
         }
    }
        
    
}