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
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();

        int vertIndex = 0;

        for (int x = 0; x < chunkWidth; x++) {
            if (walls[x,y]) {
                vertices.Add(new Vector3(x,         circlePosses[y].y,      circlePosses[y].x));
                vertices.Add(new Vector3(x,         circlePosses[y + 1].y,  circlePosses[y + 1].x));
                vertices.Add(new Vector3(x + 1,     circlePosses[y + 1].y,  circlePosses[y + 1].x));
                vertices.Add(new Vector3(x + 1,     circlePosses[y].y,      circlePosses[y].x));
            }
            else {
                vertices.Add(new Vector3(x,         dugCirclePosses[y].y,       dugCirclePosses[y].x));
                vertices.Add(new Vector3(x,         dugCirclePosses[y + 1].y,   dugCirclePosses[y + 1].x));
                vertices.Add(new Vector3(x + 1,     dugCirclePosses[y + 1].y,   dugCirclePosses[y + 1].x));
                vertices.Add(new Vector3(x + 1,     dugCirclePosses[y].y,       dugCirclePosses[y].x));
            }

            triangles.Add(vertIndex + 2);
            triangles.Add(vertIndex + 1);
            triangles.Add(vertIndex + 0);
            triangles.Add(vertIndex + 3);
            triangles.Add(vertIndex + 2);
            triangles.Add(vertIndex + 0);

            vertIndex += 4;

            if (!walls[x,y]) {
                if (y < chunkLength -1) {
                    if (walls[x,y + 1]) {
                        vertices.Add(new Vector3(x,         dugCirclePosses[y + 1].y,   dugCirclePosses[y + 1].x));
                        vertices.Add(new Vector3(x,         circlePosses[y + 1].y,      circlePosses[y + 1].x));
                        vertices.Add(new Vector3(x + 1,     circlePosses[y + 1].y,      circlePosses[y + 1].x));
                        vertices.Add(new Vector3(x + 1,     dugCirclePosses[y + 1].y,   dugCirclePosses[y + 1].x));

                        triangles.Add(vertIndex + 2);
                        triangles.Add(vertIndex + 1);
                        triangles.Add(vertIndex + 0);
                        triangles.Add(vertIndex + 3);
                        triangles.Add(vertIndex + 2);
                        triangles.Add(vertIndex + 0);

                        vertIndex += 4;
                    }
                }
                if (y > 0) {
                    if (walls[x,y - 1]) {
                        vertices.Add(new Vector3(x,         dugCirclePosses[y].y,   dugCirclePosses[y].x));
                        vertices.Add(new Vector3(x,         circlePosses[y].y,      circlePosses[y].x));
                        vertices.Add(new Vector3(x + 1,     circlePosses[y].y,      circlePosses[y].x));
                        vertices.Add(new Vector3(x + 1,     dugCirclePosses[y].y,   dugCirclePosses[y].x));

                        triangles.Add(vertIndex + 0);
                        triangles.Add(vertIndex + 1);
                        triangles.Add(vertIndex + 2);
                        triangles.Add(vertIndex + 0);
                        triangles.Add(vertIndex + 2);
                        triangles.Add(vertIndex + 3);

                        vertIndex += 4;
                    }
                }
                if (x < chunkWidth - 1) {
                    if (walls[x + 1,y]) {
                        vertices.Add(new Vector3(x + 1,     dugCirclePosses[y + 1].y,   dugCirclePosses[y + 1].x));
                        vertices.Add(new Vector3(x + 1,     circlePosses[y + 1].y,      circlePosses[y + 1].x));
                        vertices.Add(new Vector3(x + 1,     circlePosses[y].y,          circlePosses[y].x));
                        vertices.Add(new Vector3(x + 1,     dugCirclePosses[y].y,       dugCirclePosses[y].x));

                        triangles.Add(vertIndex + 2);
                        triangles.Add(vertIndex + 1);
                        triangles.Add(vertIndex + 0);
                        triangles.Add(vertIndex + 3);
                        triangles.Add(vertIndex + 2);
                        triangles.Add(vertIndex + 0);

                        vertIndex += 4;
                    }
                }
                else {
                    vertices.Add(new Vector3(x + 1,     dugCirclePosses[y + 1].y,   dugCirclePosses[y + 1].x));
                    vertices.Add(new Vector3(x + 1,     circlePosses[y + 1].y,      circlePosses[y + 1].x));
                    vertices.Add(new Vector3(x + 1,     circlePosses[y].y,          circlePosses[y].x));
                    vertices.Add(new Vector3(x + 1,     dugCirclePosses[y].y,       dugCirclePosses[y].x));

                    triangles.Add(vertIndex + 2);
                    triangles.Add(vertIndex + 1);
                    triangles.Add(vertIndex + 0);
                    triangles.Add(vertIndex + 3);
                    triangles.Add(vertIndex + 2);
                    triangles.Add(vertIndex + 0);

                    vertIndex += 4;
                }
                if (x > 0) {
                    if (walls[x - 1,y]) {
                        vertices.Add(new Vector3(x,     dugCirclePosses[y + 1].y,   dugCirclePosses[y + 1].x));
                        vertices.Add(new Vector3(x,     circlePosses[y + 1].y,      circlePosses[y + 1].x));
                        vertices.Add(new Vector3(x,     circlePosses[y].y,          circlePosses[y].x));
                        vertices.Add(new Vector3(x,     dugCirclePosses[y].y,       dugCirclePosses[y].x));

                        triangles.Add(vertIndex + 0);
                        triangles.Add(vertIndex + 1);
                        triangles.Add(vertIndex + 2);
                        triangles.Add(vertIndex + 0);
                        triangles.Add(vertIndex + 2);
                        triangles.Add(vertIndex + 3);

                        vertIndex += 4;
                    }
                }
                else {
                    vertices.Add(new Vector3(x,     dugCirclePosses[y + 1].y,   dugCirclePosses[y + 1].x));
                    vertices.Add(new Vector3(x,     circlePosses[y + 1].y,      circlePosses[y + 1].x));
                    vertices.Add(new Vector3(x,     circlePosses[y].y,          circlePosses[y].x));
                    vertices.Add(new Vector3(x,     dugCirclePosses[y].y,       dugCirclePosses[y].x));

                    triangles.Add(vertIndex + 0);
                    triangles.Add(vertIndex + 1);
                    triangles.Add(vertIndex + 2);
                    triangles.Add(vertIndex + 0);
                    triangles.Add(vertIndex + 2);
                    triangles.Add(vertIndex + 3);

                    vertIndex += 4;
                }
            }
        }

        meshes[y].vertices = vertices.ToArray();
        meshes[y].triangles = triangles.ToArray();
        //meshes[y].normals = vertices.ToArray();
        meshes[y].RecalculateNormals();
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