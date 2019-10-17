using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] Transform player;
    [SerializeField] float speedMult;
    
    void Update()
    {
        if (player.position.y > transform.position.y) {
            float speed = Vector2.Distance(new Vector2(player.position.x, player.position.y),new Vector2 (transform.position.x,transform.position.y)) * speedMult;
            transform.position = new Vector3 (0,transform.position.y + speed,-10);
        }
    }
}