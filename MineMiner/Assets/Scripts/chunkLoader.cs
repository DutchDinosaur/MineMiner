using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chunkLoader : MonoBehaviour
{
    [SerializeField] private Vector2Int chunkSize;
    [SerializeField] private Player player;

    [SerializeField] private GameObject chunkPrefab;

    [SerializeField] private chunkData[] chunksToLoad;

    private int chunksLoaded;

    [System.Serializable]
    public class chunkData
    {
        public int ChunkWidth = 3;
        [Range(0, 30)] public byte randomBombPercent = 10;
        [Range(0, 100)] public byte randomFillPercent = 40;
    }

    private void Start()
    {
        if (chunksToLoad.Length > 0) {
            player.currentChunk = loadChunk(0);

            loadChunk(1);
            loadChunk(2);
            loadChunk(3);
            loadChunk(4);
        }
    }

    Chunk loadChunk(int i)
    {
        Chunk chunk = GameObject.Instantiate(chunkPrefab, transform.position + Vector3.up * chunkSize.y * i, Quaternion.identity, transform).GetComponent<Chunk>();
        chunk.chunkSize = new Vector2Int(chunksToLoad[i].ChunkWidth, chunkSize.y);
        chunk.randomBombPercent = chunksToLoad[i].randomBombPercent;
        chunk.randomFillPercent = chunksToLoad[i].randomFillPercent;
        chunk.generateChunk();
        return chunk;
    }
}