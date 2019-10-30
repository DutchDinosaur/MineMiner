using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player2 : MonoBehaviour
{
    //[SerializeField] private Text score;
    //private int scoreint;

    public Chunk2 currentChunk;
    public Chunk2 nextChunk;
    private Chunk2 lastChunk;

    public Vector2Int chunkPos;

    private void Start() {
        chunkPos = new Vector2Int(currentChunk.chunkSize.x/2, 0);
    }

    public void Move(int dir) {
        switch (dir) {
            case 0: //right
                if (chunkPos.x < currentChunk.chunkSize.x -1) {
                    if (currentChunk.walls[chunkPos.x + 1, chunkPos.y]) {
                        currentChunk.mineTile(chunkPos.x +1, chunkPos.y);
                    }
                    else {
                        chunkPos += Vector2Int.right;
                    }
                }
                break;
            case 1: //left
                if (chunkPos.x > 0) {
                    if (currentChunk.walls[chunkPos.x -1, chunkPos.y]) {
                        currentChunk.mineTile(chunkPos.x -1, chunkPos.y);
                    }
                    else {
                        chunkPos += Vector2Int.left;
                    }
                }
                break;
            case 2: //up
                if (chunkPos.y < currentChunk.chunkSize.y - 1) {
                    if (currentChunk.walls[chunkPos.x, chunkPos.y + 1]) {
                        currentChunk.mineTile(chunkPos.x, chunkPos.y + 1);
                    }
                    else {
                        chunkPos += Vector2Int.up;
                    }
                }
                else {
                    enterNextChunk();
                }
                break;
            case 3: //down
                if (chunkPos.y > 0) {
                    if (currentChunk.walls[chunkPos.x, chunkPos.y - 1]) {
                        currentChunk.mineTile(chunkPos.x, chunkPos.y - 1);
                    }
                    else {
                        chunkPos += Vector2Int.down;
                    }
                }
                else {
                    enterlastChunk();
                }
                break;
        }

        UpdatePosition();
        //score.text = doScore().ToString();
    }

    void UpdatePosition() {
        GenerateTiledCilinderChunk2 chunkMeshGen = currentChunk.GetComponent<GenerateTiledCilinderChunk2>();

        float deg = ((2 * Mathf.PI) / (chunkMeshGen.chunkCount * chunkMeshGen.chunkLength));
        float dir = (deg * (chunkPos.y + .5f)) + (deg * chunkMeshGen.chunkLength * chunkMeshGen.chunkIndex);

        float radius = (chunkMeshGen.chunkCount * currentChunk.chunkSize.y) / (2 * Mathf.PI) - .5f;

        transform.position = new Vector3(chunkPos.x + .5f, Mathf.Sin(dir) * radius, Mathf.Cos(dir) * radius);

        transform.rotation = Quaternion.Euler(-dir * 57.8f, 0,0);
    }

    //int doScore() {
    //    if (scoreint < transform.position.y)
    //    {
    //        scoreint = (int)transform.position.y;
    //    }
    //    return scoreint;
    //}

    void enterNextChunk() {
        int chunkSizeDifference = nextChunk.chunkSize.x - currentChunk.chunkSize.x;
        if (chunkSizeDifference > 0 || (chunkPos.x >= -chunkSizeDifference / 2 && chunkPos.x < -chunkSizeDifference / 2 + nextChunk.chunkSize.x)) {
            lastChunk = currentChunk;
            currentChunk = nextChunk;
            chunkPos.y = 0;
            chunkPos.x += (currentChunk.chunkSize.x - lastChunk.chunkSize.x) / 2;
        }
    }

    void enterlastChunk() {
        int chunkSizeDifference = lastChunk.chunkSize.x - currentChunk.chunkSize.x;
        if (chunkSizeDifference > 0 || (chunkPos.x >= -chunkSizeDifference /2 && chunkPos.x < -chunkSizeDifference/2 + lastChunk.chunkSize.x)) {
            currentChunk = lastChunk;
            chunkPos.y = currentChunk.chunkSize.y - 1;
            chunkPos.x += (currentChunk.chunkSize.x - nextChunk.chunkSize.x) / 2;
        }
    }
}