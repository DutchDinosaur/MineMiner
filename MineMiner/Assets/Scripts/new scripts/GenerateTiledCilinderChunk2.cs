using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTiledCilinderChunk2 : MonoBehaviour
{
    public int chunkLength = 30;
    public int chunkWidth = 9;
    public int chunkCount = 4;

    public float digDepth = 1;

    private Chunk2 chunk;
    [SerializeField] private GameObject chunkTileObject;

    public int chunkIndex;

    [HideInInspector]
    float radius;
    Mesh[] meshes;
    Vector2[] circlePosses;
    Vector2[] dugCirclePosses;

    private void Start()
    {
        Initialise();
    }

    public void Initialise() {
        chunk = gameObject.GetComponent<Chunk2>();

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
                setUvs(2);

                //AddQuadUVs(new Vector2(0, 0.6667f), new Vector2(0.3333f, 1));
            }
            else {
                vertices.Add(new Vector3(x,         dugCirclePosses[y].y,       dugCirclePosses[y].x));
                vertices.Add(new Vector3(x,         dugCirclePosses[y + 1].y,   dugCirclePosses[y + 1].x));
                vertices.Add(new Vector3(x + 1,     dugCirclePosses[y + 1].y,   dugCirclePosses[y + 1].x));
                vertices.Add(new Vector3(x + 1,     dugCirclePosses[y].y,       dugCirclePosses[y].x));
                if (chunk.exploded[x,y]) {
                    setUvs(6);
                }
                else if (chunk.values[x,y] == 0) {
                    setUvs(0);
                }
                else if (chunk.values[x,y] == 1) {
                    setUvs(4);
                }
                else if (chunk.values[x, y] > 1) {
                    setUvs(7);
                }

                //AddQuadUVs(new Vector2(0,0), new Vector2(0.3333f,0.3333f));
            }

            AddTriangles(false);
            vertIndex += 4;

            if (!walls[x,y]) {
                if (y < chunkLength -1) {
                    if (walls[x,y + 1]) {
                        vertices.Add(new Vector3(x,         dugCirclePosses[y + 1].y,   dugCirclePosses[y + 1].x));
                        vertices.Add(new Vector3(x,         circlePosses[y + 1].y,      circlePosses[y + 1].x));
                        vertices.Add(new Vector3(x + 1,     circlePosses[y + 1].y,      circlePosses[y + 1].x));
                        vertices.Add(new Vector3(x + 1,     dugCirclePosses[y + 1].y,   dugCirclePosses[y + 1].x));
                        setUvs(1);

                        //AddQuadUVs(new Vector2(0, 0.3334f), new Vector2(0.3333f, 0.6666f));
                        AddTriangles(false);
                        vertIndex += 4;
                    }
                }
                if (y > 0) {
                    if (walls[x,y - 1]) {
                        vertices.Add(new Vector3(x,         dugCirclePosses[y].y,   dugCirclePosses[y].x));
                        vertices.Add(new Vector3(x,         circlePosses[y].y,      circlePosses[y].x));
                        vertices.Add(new Vector3(x + 1,     circlePosses[y].y,      circlePosses[y].x));
                        vertices.Add(new Vector3(x + 1,     dugCirclePosses[y].y,   dugCirclePosses[y].x));
                        setUvs(1);

                        //AddQuadUVs(new Vector2(0, 0.3334f), new Vector2(0.3333f, 0.6666f));
                        AddTriangles(true);
                        vertIndex += 4;
                    }
                }
                if (x < chunkWidth - 1) {
                    if (walls[x + 1,y]) {
                        vertices.Add(new Vector3(x + 1,     dugCirclePosses[y + 1].y,   dugCirclePosses[y + 1].x));
                        vertices.Add(new Vector3(x + 1,     circlePosses[y + 1].y,      circlePosses[y + 1].x));
                        vertices.Add(new Vector3(x + 1,     circlePosses[y].y,          circlePosses[y].x));
                        vertices.Add(new Vector3(x + 1,     dugCirclePosses[y].y,       dugCirclePosses[y].x));
                        setUvs(1);

                        //AddQuadUVs(new Vector2(0, 0.3334f), new Vector2(0.3333f, 0.6666f));
                        AddTriangles(false);
                        vertIndex += 4;
                    }
                }
                else {
                    vertices.Add(new Vector3(x + 1,     dugCirclePosses[y + 1].y,   dugCirclePosses[y + 1].x));
                    vertices.Add(new Vector3(x + 1,     circlePosses[y + 1].y,      circlePosses[y + 1].x));
                    vertices.Add(new Vector3(x + 1,     circlePosses[y].y,          circlePosses[y].x));
                    vertices.Add(new Vector3(x + 1,     dugCirclePosses[y].y,       dugCirclePosses[y].x));
                    setUvs(1);

                    //AddQuadUVs(new Vector2(0, 0.3334f), new Vector2(0.3333f, 0.6666f));
                    AddTriangles(false);
                    vertIndex += 4;
                }
                if (x > 0) {
                    if (walls[x - 1,y]) {
                        vertices.Add(new Vector3(x,     dugCirclePosses[y + 1].y,   dugCirclePosses[y + 1].x));
                        vertices.Add(new Vector3(x,     circlePosses[y + 1].y,      circlePosses[y + 1].x));
                        vertices.Add(new Vector3(x,     circlePosses[y].y,          circlePosses[y].x));
                        vertices.Add(new Vector3(x,     dugCirclePosses[y].y,       dugCirclePosses[y].x));
                        setUvs(1);

                        //AddQuadUVs(new Vector2(0, 0.3334f), new Vector2(0.3333f, 0.6666f));
                        AddTriangles(true);
                        vertIndex += 4;
                    }
                }
                else {
                    vertices.Add(new Vector3(x,     dugCirclePosses[y + 1].y,   dugCirclePosses[y + 1].x));
                    vertices.Add(new Vector3(x,     circlePosses[y + 1].y,      circlePosses[y + 1].x));
                    vertices.Add(new Vector3(x,     circlePosses[y].y,          circlePosses[y].x));
                    vertices.Add(new Vector3(x,     dugCirclePosses[y].y,       dugCirclePosses[y].x));
                    setUvs(1);

                    //AddQuadUVs(new Vector2(0, 0.3334f), new Vector2(0.3333f, 0.6666f));
                    AddTriangles(true);
                    vertIndex += 4;
                }
            }
        }

        void AddTriangles(bool direction) {
            if (direction) {
                triangles.Add(vertIndex + 0);
                triangles.Add(vertIndex + 1);
                triangles.Add(vertIndex + 2);
                triangles.Add(vertIndex + 0);
                triangles.Add(vertIndex + 2);
                triangles.Add(vertIndex + 3);
            }
            else {
                triangles.Add(vertIndex + 2);
                triangles.Add(vertIndex + 1);
                triangles.Add(vertIndex + 0);
                triangles.Add(vertIndex + 3);
                triangles.Add(vertIndex + 2);
                triangles.Add(vertIndex + 0);
            }
        }

        void setUvs(int texture) {
            switch (texture) {
                case 0: {
                    AddQuadUVs(new Vector2(0,0),new Vector2(.3333f,.3333f));
                    break;
                }
                case 1: {
                    AddQuadUVs(new Vector2(0,.3333f),new Vector2(.3333f,.6666f));
                    break;
                }
                case 2: {
                    AddQuadUVs(new Vector2(0,.6666f),new Vector2(.3333f,1));
                    break;
                }
                case 3: {
                    AddQuadUVs(new Vector2(.3333f,.6666f),new Vector2(.6666f,1));
                    break;
                }
                case 4: {
                    AddQuadUVs(new Vector2(.3333f,.3333f),new Vector2(.6666f,.6666f));
                    break;
                }
                case 5: {
                    AddQuadUVs(new Vector2(.3333f,0),new Vector2(.6666f,.3333f));
                    break;
                }
                case 6: {
                    AddQuadUVs(new Vector2(.6666f,0),new Vector2(1,.3333f));
                    break;
                }
                case 7: {
                    AddQuadUVs(new Vector2(.6666f,.3333f),new Vector2(1,.6666f));
                    break;
                }
                case 8: {
                    AddQuadUVs(new Vector2(.6666f,.6666f),new Vector2(1,1));
                    break;
                }
            }
        }

        void AddQuadUVs(Vector2 BottomLeftCorner, Vector2 TopRightCorner) {
            uvs.Add(new Vector2(BottomLeftCorner.x, BottomLeftCorner.y));
            uvs.Add(new Vector2(BottomLeftCorner.x, TopRightCorner.y));
            uvs.Add(new Vector2(TopRightCorner.x, TopRightCorner.y));
            uvs.Add(new Vector2(TopRightCorner.x, BottomLeftCorner.y));
        }

        meshes[y].Clear();
        meshes[y].vertices = vertices.ToArray();
        meshes[y].triangles = triangles.ToArray();
        meshes[y].uv = uvs.ToArray();
        //meshes[y].normals = vertices.ToArray();
        meshes[y].RecalculateNormals();
    }

    Vector2[] possesOnCircle(int length, float radius, int chunkIndex) {
        Vector2[] posses = new Vector2[length];
        for (int y = 0; y < length; y++) {
            float deg = ((2 * Mathf.PI) / (chunkCount * chunkLength));
            float dir = (deg * y) + (deg * chunkLength * chunkIndex);
            posses[y] = new Vector2(Mathf.Cos(dir) * radius, Mathf.Sin(dir) * radius);
        }
        return posses;
    }
}