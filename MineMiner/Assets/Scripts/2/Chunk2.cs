using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Chunk2 : MonoBehaviour
{
    public Vector2Int chunkSize;
    public int chunkIndex;
    public int chunkCount = 4;

    [Range(0, 30)] public byte randomBombPercent = 10;
    [SerializeField] private Sprite[] tileNumbers;
    [Space(10)]
    [SerializeField] private string seed;
    [SerializeField] private bool useRandomSeed;
    [Space(10)]
    [Range(0, 100), SerializeField] public byte randomFillPercent = 40;
    [SerializeField] private byte smoothingPasses = 5;

    public bool[,] walls;
    public bool[,] bombs;
    public int[,] values;

    private GenerateTiledCilinderChunk2 meshGen;


    private void Start()
    {
        generateChunk();
    }

    public void generateChunk()
    {
        //generate walls
        walls = new bool[chunkSize.x, chunkSize.y];
        bombs = new bool[chunkSize.x, chunkSize.y];
        values = new int[chunkSize.x, chunkSize.y];

        randomFillMap();
        for (int i = 0; i < smoothingPasses; i++)
        {
            smoothMap();
        }

        generateBombs();
        calculateSurroundingBombCount();

        generateChunkObjects();
    }


    void randomFillMap()
    {
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }
        System.Random Random = new System.Random(seed.GetHashCode());

        for (int x = 0; x < chunkSize.x; x++) {
            for (int y = 0; y < chunkSize.y; y++) {
                if (false/*x == 0 || x == chunkSize.x - 1 /*|| y == 0 || y == chunkSize.y -1*/) {
                    walls[x, y] = true;
                } else if (y < 3 || y > chunkSize.y - 3) {
                    walls[x, y] = false;
                } else {
                    walls[x, y] = (Random.Next(0, 100) < randomFillPercent) ? true : false;
                }
            }
        }
    }

    void smoothMap()
    {
        for (int x = 0; x < chunkSize.x; x++) {
            for (int y = 0; y < chunkSize.y; y++) {
                int neighborWallTiles = GetSurroundingWallCount(x, y);

                if (neighborWallTiles > 5) {
                    walls[x, y] = true;
                } else if (neighborWallTiles < 4) {
                    walls[x, y] = false;
                }
            }
        }
    }

    void generateBombs() {
        if (useRandomSeed) {
            seed = Time.time.ToString();
        }
        System.Random Random = new System.Random(seed.GetHashCode());

        for (int x = 0; x < chunkSize.x; x++) {
            for (int y = 0; y < chunkSize.y; y++) {
                if (walls[x,y]) {
                    bombs[x, y] = (Random.Next(0, 100) < randomBombPercent) ? true : false;
                }
            }
        }
    }

    void calculateSurroundingBombCount() {
        for (int x = 0; x < chunkSize.x; x++) {
            for (int y = 0; y < chunkSize.y; y++) {
                values[x,y] = GetSurroundingBombCount(x, y);
            }
        }
    }

    void generateChunkObjects() {
        meshGen = gameObject.GetComponent<GenerateTiledCilinderChunk2>();
        meshGen.chunk = this;
        meshGen.chunkLength = chunkSize.y;
        meshGen.chunkWidth = chunkSize.x;
        meshGen.chunkCount = chunkCount;
        meshGen.chunkIndex = chunkIndex;
        
        meshGen.Initialise();
    }


    public void mineTile(int x, int y) {
        if (!bombs[x,y]) {
            walls[x, y] = false;
            meshGen.GenerateTileMesh(walls, y);
        }
        else {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


    int GetTileObjectIndex(int x, int y) {
        return (x * chunkSize.y + y);
    }

    int GetSurroundingWallCount(int gridX, int gridY) {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++) {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) {
                if (neighbourX >= 0 && neighbourX < chunkSize.x && neighbourY >= 0 && neighbourY < chunkSize.y) {
                    if (neighbourX != gridX || neighbourY != gridY) {
                        wallCount += (walls[neighbourX, neighbourY]) ? 1 : 0;
                    }
                }
                else {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }

    int GetSurroundingBombCount(int gridX, int gridY) {
        int bombCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++) {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++) {
                if (neighbourX >= 0 && neighbourX < chunkSize.x && neighbourY >= 0 && neighbourY < chunkSize.y) {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        bombCount += (bombs[neighbourX, neighbourY]) ? 1 : 0;
                    }
                }
            }
        }
        return bombCount;
    }
}