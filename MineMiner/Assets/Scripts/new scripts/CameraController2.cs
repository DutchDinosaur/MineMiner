using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController2 : MonoBehaviour
{
    [SerializeField] private Transform camera;
    public Player2 player;

    [SerializeField]private float cameraDistance;

    void LateUpdate() {
        GenerateTiledCilinderChunk2 currentchunk = player.currentChunk.GetComponent<GenerateTiledCilinderChunk2>();

        float radius = (currentchunk.chunkCount * currentchunk.chunkLength) / (2 * Mathf.PI) * cameraDistance;

        float deg = ((2 * Mathf.PI) / (currentchunk.chunkCount * currentchunk.chunkLength));
        float dir = (deg * -player.chunkPos.y) + (deg * currentchunk.chunkLength * (-currentchunk.chunkIndex)) + 1.8f;
        camera.position = new Vector3(currentchunk.chunkWidth /2 + .5f, Mathf.Cos(dir) * radius, Mathf.Sin(dir) * radius);

        Vector3 rot = player.transform.rotation.eulerAngles;
        camera.rotation = player.transform.rotation;
        camera.Rotate(new Vector3(-40, 180, 0));
    }
}