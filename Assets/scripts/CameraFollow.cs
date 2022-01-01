using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    void Update()
    {
        //Debug.Log(player.position);
        transform.position = player.position + offset;
        
    }
}
