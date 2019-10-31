using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLimit : MonoBehaviour
{
    [SerializeField] private Transform DEATH;

    int yPos;
    public float speed;

    [SerializeField] private float Distance;

    public Player2 player;

    GenerateTiledCilinderChunk2 currentchunk;

    private void Start() {
        StartCoroutine(MoveForward());
    }

    void Update() {
        GenerateTiledCilinderChunk2 currentchunk = player.currentChunk.GetComponent<GenerateTiledCilinderChunk2>();
        int chunkcount = currentchunk.chunkCount;
        int chunklength = currentchunk.chunkLength;

        float radius = (chunkcount * chunklength) / (2 * Mathf.PI) * Distance;

        float deg = ((2 * Mathf.PI) / (chunkcount * chunklength));
        float dir = (-deg * yPos) + (deg * chunklength * 1.7f);

        DEATH.position = new Vector3(currentchunk.chunkWidth / 2 + .5f, Mathf.Cos(dir) * radius, Mathf.Sin(dir) * radius);

        DEATH.rotation = Quaternion.Euler(dir * 57.3f, 0, 0); ;
    }

    IEnumerator MoveForward() {
        while (true) {
            yPos += 1;
            GetComponent<GameManager>().DeathDist += 1;
            yield return new WaitForSeconds(1 / speed);
        }
    }
}