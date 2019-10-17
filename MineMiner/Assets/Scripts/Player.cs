using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //[SerializeField] private Text score;
    private int scoreint;

    [SerializeField] public Chunk currentChunk;
    public Vector2Int posInCurrentChunk;

    private void Start()
    {
        posInCurrentChunk = new Vector2Int(currentChunk.chunkSize.x/2, 1);
    }

    public void Move(int dir) {
        switch (dir) {
            case 0: //right
                if (transform.position.x < currentChunk.chunkSize.x/2) {
                    if (currentChunk.walls[posInCurrentChunk.x + 1, posInCurrentChunk.y]) {
                        currentChunk.mineTile(posInCurrentChunk.x +1, posInCurrentChunk.y);
                    }
                    else {
                        posInCurrentChunk += Vector2Int.right;
                    }
                }
                break;
            case 1: //left
                if (transform.position.x > -currentChunk.chunkSize.x/2) {
                    if (currentChunk.walls[posInCurrentChunk.x -1, posInCurrentChunk.y])
                    {
                        currentChunk.mineTile(posInCurrentChunk.x -1, posInCurrentChunk.y);
                    }
                    else
                    {
                        posInCurrentChunk += Vector2Int.left;
                    }
                }
                break;
            case 2: //up
                if (transform.position.y > -currentChunk.chunkSize.y) {
                    if (currentChunk.walls[posInCurrentChunk.x, posInCurrentChunk.y + 1]) {
                        currentChunk.mineTile(posInCurrentChunk.x, posInCurrentChunk.y + 1);
                    }
                    else {
                        posInCurrentChunk += Vector2Int.up;
                    }
                }
                break;
            case 3: //down
                if (transform.position.y > 0) {
                    if (currentChunk.walls[posInCurrentChunk.x, posInCurrentChunk.y - 1]) {
                        currentChunk.mineTile(posInCurrentChunk.x, posInCurrentChunk.y - 1);
                    }
                    else {
                        posInCurrentChunk += Vector2Int.down;
                    }
                }
                break;
        }
        transform.position = new Vector3(posInCurrentChunk.x - currentChunk.chunkSize.x / 2, posInCurrentChunk.y + currentChunk.transform.position.y,-1);
        //if (scoreint < posInCurrentChunk.y) {
        //    scoreint = posInCurrentChunk.y;
        //}
        //score.text = scoreint.ToString();
    }
}