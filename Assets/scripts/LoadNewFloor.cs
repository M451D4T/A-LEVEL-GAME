using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewFloor : MonoBehaviour
{
    //reloads scene when player touches the object
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Debug.Log(SceneManager.GetActiveScene().name +"  new floor loaded");
        }
    }
}
