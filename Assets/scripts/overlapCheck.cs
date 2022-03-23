using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class overlapCheck : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("floor"))
        {
            Debug.Log(col.name);
            Destroy(col.gameObject);
        }
    }


}




    
