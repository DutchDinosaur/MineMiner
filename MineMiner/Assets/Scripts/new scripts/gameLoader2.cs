using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameLoader2 : MonoBehaviour
{
    [SerializeField] private Vector2Int chunkSize;
    [SerializeField] private int chunkCount;

    [SerializeField] private GameObject playerObject;
    private Player2 player;

    [SerializeField] private GameObject chunkPrefab;

    [SerializeField] private chunkData[] chunksToLoad;

    private int currentChunk;
    private List<Chunk2> chunkList;

    [System.Serializable]
    public class chunkData {
        public int ChunkWidth = 3;
        [Range(0, 30)] public byte randomBombPercent = 10;
        [Range(0, 100)] public byte randomFillPercent = 40;
    }

    private void Start() {
        player = GameObject.Instantiate(playerObject, transform.position, Quaternion.identity).GetComponent<Player2>();
        GetComponent<SwipeDetection2>().player = player;
        GetComponent<CameraController2>().player = player.transform;

        chunkList = new List<Chunk2>();

        if (chunksToLoad.Length > 3) {
            chunkList.Add(loadChunk(0));
            chunkList.Add(loadChunk(1));
            chunkList.Add(loadChunk(2));

            player.currentChunk = chunkList[0];
            player.nextChunk = chunkList[1];

            currentChunk = 2;
        }
        else {
            Debug.LogError("not enough chunksToLoad");
        }
    }

    private void Update() {
        if (player.chunkPos.y > chunkSize.y/2 && player.currentChunk == chunkList[chunkList.Count -3]) {
            chunkList.Add(loadChunk(currentChunk += 1));
            player.nextChunk = chunkList[chunkList.Count - 3];
            DespawnChunks();
        }
    }

    Chunk2 loadChunk(int i) {
        if (i < chunksToLoad.Length) {
            Chunk2 chunk = GameObject.Instantiate(chunkPrefab, transform.position, Quaternion.identity).GetComponent<Chunk2>();
            chunk.chunkSize = new Vector2Int(chunksToLoad[i].ChunkWidth, chunkSize.y);
            chunk.randomBombPercent = chunksToLoad[i].randomBombPercent;
            chunk.randomFillPercent = chunksToLoad[i].randomFillPercent;
            chunk.gameObject.GetComponent<GenerateTiledCilinderChunk2>().chunkIndex = i;
            return chunk;
        }
        else {
            Chunk2 chunk = GameObject.Instantiate(chunkPrefab, transform.position, Quaternion.identity).GetComponent<Chunk2>();
            chunk.chunkSize = new Vector2Int(chunksToLoad[chunksToLoad.Length -1].ChunkWidth, chunkSize.y);
            chunk.randomBombPercent = chunksToLoad[chunksToLoad.Length -1].randomBombPercent;
            chunk.randomFillPercent = chunksToLoad[chunksToLoad.Length -1].randomFillPercent;
            chunk.gameObject.GetComponent<GenerateTiledCilinderChunk2>().chunkIndex = i;
            return chunk;
        }
    }

    void DespawnChunks() {
        if (chunkList.Count > 6) {
            Destroy(chunkList[0].gameObject);
            chunkList.RemoveAt(0);
        }
    }
}