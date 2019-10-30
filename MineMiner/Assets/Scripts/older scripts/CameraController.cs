using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform camera;
    public Transform player;
    [SerializeField] float speedMult;
    
    void Update() {
        if (player.position.y > camera.position.y) {
            float speed = Vector2.Distance(new Vector2(player.position.x, player.position.y),new Vector2 (camera.position.x, camera.position.y)) * speedMult;
            camera.position = new Vector3 (0, camera.position.y + speed,-10);
        }
    }
}