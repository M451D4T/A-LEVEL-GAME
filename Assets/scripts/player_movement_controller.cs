using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement_controller : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    
    private Vector3 movementChange;

    private  void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }
    
    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
  
        //reset movementchange
        movementChange = new Vector3(x,y,0);

        //swap sprite direction based on horizontal direction
        if (movementChange.x > 0)
        transform.localScale = Vector3.one;

        else if (movementChange.x < 0)
        transform.localScale = new Vector3(-1,1,1);
        
        // makeing movement
        transform.Translate(movementChange* Time.deltaTime);
    }


}
