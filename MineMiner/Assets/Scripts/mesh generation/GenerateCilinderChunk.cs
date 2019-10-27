using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCilinderChunk : MonoBehaviour
{
    [Range(5, 100)] public int chunkLength = 30;
    [Range(3, 15)] public int chunkWidth = 9;
    [Range(1, 10)] public int chunkCount = 4;

    [Range(0, 5)] public float digDepth = 1;

    [SerializeField] private debugChunk chunk;

    private void OnValidate() {
        Initialise();
    }

    private void Initialise() {
        chunk.chunkSize = new Vector2Int(chunkWidth,chunkLength);
        chunk.generateChunk();

        GenerateCilinderChunkMesh(chunk.walls,0);
    }

    void GenerateCilinderChunkMesh(bool[,] walls, int chunkIndex) {
        int quadCount = chunkWidth * chunkLength;
        float radius = (chunkCount * chunkLength)/(2*Mathf.PI);

        Vector3[] vertices = new Vector3[quadCount * 4];
        Vector2[] uvs = new Vector2[quadCount * 4];
        int[] triangles = new int[quadCount * 6];

        int triIndex = 0;
        int vertIndex = 0;

        Vector2[] circlePosses = possesOnCircle(chunkLength + 1,radius, chunkIndex);
        Vector2[] dugCirclePosses = possesOnCircle(chunkLength + 1, radius - digDepth, chunkIndex);

        for (int y = 0; y <  chunkLength; y++) {
            for (int x = 0; x < chunkWidth; x++) {
                if (walls[x,y]) {
                    vertices[vertIndex + 0] = new Vector3(x,        circlePosses[y].y,      circlePosses[y].x);
                    vertices[vertIndex + 1] = new Vector3(x,        circlePosses[y + 1].y,  circlePosses[y + 1].x);
                    vertices[vertIndex + 2] = new Vector3(x + 1,    circlePosses[y + 1].y,  circlePosses[y + 1].x);
                    vertices[vertIndex + 3] = new Vector3(x + 1,    circlePosses[y].y,      circlePosses[y].x);
                }
                else {
                    vertices[vertIndex + 0] = new Vector3(x,        dugCirclePosses[y].y,       dugCirclePosses[y].x);
                    vertices[vertIndex + 1] = new Vector3(x,        dugCirclePosses[y + 1].y,   dugCirclePosses[y + 1].x);
                    vertices[vertIndex + 2] = new Vector3(x + 1,    dugCirclePosses[y + 1].y,   dugCirclePosses[y + 1].x);
                    vertices[vertIndex + 3] = new Vector3(x + 1,    dugCirclePosses[y].y,       dugCirclePosses[y].x);
                }

                triangles[triIndex + 0] = vertIndex + 2;
                triangles[triIndex + 1] = vertIndex + 1;
                triangles[triIndex + 2] = vertIndex + 0;
                triangles[triIndex + 3] = vertIndex + 3;
                triangles[triIndex + 4] = vertIndex + 2;
                triangles[triIndex + 5] = vertIndex + 0;

                vertIndex += 4;
                triIndex += 6;
            }
        }

        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        //mesh.RecalculateNormals();
        mesh.normals = vertices;
    }

    Vector2[] possesOnCircle(int length, float radius, int chunkIndex) {
        Vector2[] posses = new Vector2[length];
        for (int y = 0; y < length; y++) {
            float dir = ((2 * Mathf.PI) / (chunkCount * chunkLength )) * y;
            posses[y] = new Vector2(Mathf.Cos(dir) * radius, Mathf.Sin(dir) * radius);
        }
        return posses;
    }

    Vector2 posOnCircle(int y, float radius) {
        float dir = ((2 * Mathf.PI)/(chunkCount * chunkLength)) * y;
        return new Vector2(Mathf.Cos(dir) * radius, Mathf.Sin(dir) * radius);
    }
}