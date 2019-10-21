using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameLoader : MonoBehaviour
{
    [SerializeField] private Vector2Int chunkSize;
    [SerializeField] private GameObject playerObject;
    private Player player;

    [SerializeField] private GameObject chunkPrefab;

    [SerializeField] private chunkData[] chunksToLoad;

    private int currentChunk;
    private Chunk lastLoadedChunk;

    [System.Serializable]
    public class chunkData {
        public int ChunkWidth = 3;
        [Range(0, 30)] public byte randomBombPercent = 10;
        [Range(0, 100)] public byte randomFillPercent = 40;
    }

    private void Start() {
        player = GameObject.Instantiate(playerObject,transform.position,Quaternion.identity).GetComponent<Player>();
        GetComponent<SwipeDetection>().player = player;
        GetComponent<CameraController>().player = player.transform;

        if (chunksToLoad.Length > 1) {
            player.currentChunk = loadChunk(0);
            lastLoadedChunk = loadChunk(1);
            currentChunk = 1;
            player.nextChunk = lastLoadedChunk;
        }
        else {
            Debug.LogError("not enough chunksToLoad");
        }
    }

    private void Update() {
        if (player.transform.position.y > player.currentChunk.transform.position.y + chunkSize.y/2 && player.currentChunk == lastLoadedChunk) {            
            lastLoadedChunk = loadChunk(currentChunk += 1);
            player.nextChunk = lastLoadedChunk;
            Debug.Log("aaaaaaaaaa");
        }
    }

    Chunk loadChunk(int i) {
        if (i <= chunksToLoad.Length) {
            Chunk chunk = GameObject.Instantiate(chunkPrefab, transform.position + Vector3.up * chunkSize.y * i, Quaternion.identity).GetComponent<Chunk>();
            chunk.chunkSize = new Vector2Int(chunksToLoad[i].ChunkWidth, chunkSize.y);
            chunk.randomBombPercent = chunksToLoad[i].randomBombPercent;
            chunk.randomFillPercent = chunksToLoad[i].randomFillPercent;
            chunk.generateChunk();
            return chunk;
        }
        else {
            Chunk Chunk = GameObject.Instantiate(chunkPrefab, transform.position + Vector3.up * chunkSize.y * (chunksToLoad.Length - 1), Quaternion.identity).GetComponent<Chunk>();
            Chunk.chunkSize = new Vector2Int(chunksToLoad[chunksToLoad.Length -1].ChunkWidth, chunkSize.y);
            Chunk.randomBombPercent = chunksToLoad[chunksToLoad.Length -1].randomBombPercent;
            Chunk.randomFillPercent = chunksToLoad[chunksToLoad.Length -1].randomFillPercent;
            Chunk.generateChunk();
            return Chunk;
        }
    }
}