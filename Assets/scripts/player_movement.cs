using UnityEngine;

public class player_movement : MonoBehaviour
{
    public float movespeed = 5f; //speed player moves should be changeable by items

    //allows linking of componet to scripts
    public Rigidbody2D rb; 
    public Animator animator;
    public BoxCollider2D bc;
    private RaycastHit2D hitx;
    private RaycastHit2D hity;
    private Vector2 movement;

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
        //make sure we can move in x direction by casting a box there first if the box is null allows movement
        hitx = Physics2D.BoxCast(transform.position, bc.size, 0, new Vector2(movement.x,0), movement.x*Time.fixedDeltaTime, LayerMask.GetMask("blocking","actor"));
        //make sure we can move in y direction by casting a box there first if the box is null allows movement
        hity = Physics2D.BoxCast(transform.position, bc.size, 0, new Vector2(0,movement.y), movement.y*Time.fixedDeltaTime, LayerMask.GetMask("blocking","actor"));
        if (hity.collider==null && hitx.collider==null)
        {
            //movment happens
            rb.MovePosition(rb.position + movement * movespeed * Time.fixedDeltaTime);
        }

    }
}