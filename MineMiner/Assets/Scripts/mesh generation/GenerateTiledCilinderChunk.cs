using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTiledCilinderChunk : MonoBehaviour
{
    [Range(5, 100)] public int chunkLength = 30;
    [Range(3, 15)] public int chunkWidth = 9;
    [Range(1, 10)] public int chunkCount = 4;

    [Range(0, 5)] public float digDepth = 1;

    [SerializeField] private debugChunk chunk;
    [SerializeField] private GameObject chunkTileObject;

    [HideInInspector] int chunkIndex; // controls where chunk is generated on cilinder

    float radius;
    Mesh[] meshes;
    Vector2[] circlePosses;
    Vector2[] dugCirclePosses;

    private void Start()
    {
        Initialise();
    }

    private void Initialise() {
        meshes = new Mesh[chunkLength];
        for (int i = 0; i < chunkLength; i++) {
            meshes[i] = GameObject.Instantiate(chunkTileObject,transform.position,Quaternion.identity,transform).GetComponent<MeshFilter>().mesh;
        }

        radius = (chunkCount * chunkLength) / (2 * Mathf.PI);
        circlePosses = possesOnCircle(chunkLength + 1, radius, chunkIndex);
        dugCirclePosses = possesOnCircle(chunkLength + 1, radius - digDepth, chunkIndex);

        chunk.chunkSize = new Vector2Int(chunkWidth, chunkLength);
        chunk.generateChunk();

        for (int i = 0; i < chunkLength; i++) {
            GenerateTileMesh(chunk.walls,i);
        }
    }

    public void GenerateTileMesh(bool[,] walls, int y) {
        int quadCount = chunkWidth;

        Vector3[] vertices = new Vector3[quadCount * 4];
        Vector2[] uvs = new Vector2[quadCount * 4];
        int[] triangles = new int[quadCount * 6];

        int triIndex = 0;
        int vertIndex = 0;

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

        meshes[y].vertices = vertices;
        meshes[y].triangles = triangles;
        meshes[y].normals = vertices;
        //meshes[y].RecalculateNormals();
    }

    Vector2[] possesOnCircle(int length, float radius, int chunkIndex) {
        Vector2[] posses = new Vector2[length];
        for (int y = 0; y < length; y++) {
            float dir = ((2 * Mathf.PI) / (chunkCount * chunkLength * (chunkIndex + 1))) * y;
            posses[y] = new Vector2(Mathf.Cos(dir) * radius, Mathf.Sin(dir) * radius);
        }
        return posses;
    }
}