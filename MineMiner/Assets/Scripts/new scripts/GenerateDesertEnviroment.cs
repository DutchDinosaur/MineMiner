﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateDesertEnviroment : MonoBehaviour
{
    private GenerateTiledCilinderChunk2 chunkGen;

    [SerializeField] float cliffHeight;
    [SerializeField] float wallSlope;
    [SerializeField] int SidesWidth;

    [SerializeField] private GameObject[] desertObjects;
    [SerializeField] private int ObjectCount;

    void Start() {
        chunkGen = GetComponent<GenerateTiledCilinderChunk2>();
        GenerateEnviroment();
    }

    void GenerateEnviroment() {
        float radius = (chunkGen.chunkCount * chunkGen.chunkLength) / (2 * Mathf.PI);
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();

        int vertIndex = 0;

        Vector2[] circlePosses = possesOnCircle(chunkGen.chunkLength + 1, radius);
        Vector2[] cliffCirclePosses = possesOnCircle(chunkGen.chunkLength + 1, radius + cliffHeight);

        for (int y = 0; y < chunkGen.chunkLength; y++) {
            for (int x = 0; x < SidesWidth; x++) {
                if (x == 0) {
                    vertices.Add(new Vector3(x, circlePosses[y + 1].y, circlePosses[y + 1].x));
                    vertices.Add(new Vector3(x - wallSlope, cliffCirclePosses[y + 1].y, cliffCirclePosses[y + 1].x));
                    vertices.Add(new Vector3(x - wallSlope, cliffCirclePosses[y].y, cliffCirclePosses[y].x));
                    vertices.Add(new Vector3(x, circlePosses[y].y, circlePosses[y].x));
                    AddQuadUVs(new Vector2(0, 0.6667f), new Vector2(0.3333f, 1));
                    AddTriangles(true);

                    vertIndex += 4;
                }
                else {
                    vertices.Add(new Vector3(-x - wallSlope, cliffCirclePosses[y + 1].y, cliffCirclePosses[y + 1].x));
                    vertices.Add(new Vector3(-x + 1 - wallSlope, cliffCirclePosses[y + 1].y, cliffCirclePosses[y + 1].x));
                    vertices.Add(new Vector3(-x + 1 - wallSlope, cliffCirclePosses[y].y, cliffCirclePosses[y].x));
                    vertices.Add(new Vector3(-x - wallSlope, cliffCirclePosses[y].y, cliffCirclePosses[y].x));
                    AddQuadUVs(new Vector2(0, 0.6667f), new Vector2(0.3333f, 1));
                    AddTriangles(false);

                    vertIndex += 4;
                }
            }

            for (int x = 0; x > -SidesWidth; x--) {
                if (x == 0) {
                    vertices.Add(new Vector3(chunkGen.chunkWidth, circlePosses[y + 1].y, circlePosses[y + 1].x));
                    vertices.Add(new Vector3(chunkGen.chunkWidth + wallSlope, cliffCirclePosses[y + 1].y, cliffCirclePosses[y + 1].x));
                    vertices.Add(new Vector3(chunkGen.chunkWidth + wallSlope, cliffCirclePosses[y].y, cliffCirclePosses[y].x));
                    vertices.Add(new Vector3(chunkGen.chunkWidth, circlePosses[y].y, circlePosses[y].x));
                    AddQuadUVs(new Vector2(0, 0.6667f), new Vector2(0.3333f, 1));
                    AddTriangles(false);

                    vertIndex += 4;
                }
                else {
                    vertices.Add(new Vector3(-x + chunkGen.chunkWidth -1 + wallSlope, cliffCirclePosses[y + 1].y, cliffCirclePosses[y + 1].x));
                    vertices.Add(new Vector3(-x + chunkGen.chunkWidth + wallSlope, cliffCirclePosses[y + 1].y, cliffCirclePosses[y + 1].x));
                    vertices.Add(new Vector3(-x + chunkGen.chunkWidth + wallSlope,  cliffCirclePosses[y].y, cliffCirclePosses[y].x));
                    vertices.Add(new Vector3(-x + chunkGen.chunkWidth - 1 + wallSlope, cliffCirclePosses[y].y, cliffCirclePosses[y].x));
                    AddQuadUVs(new Vector2(0, 0.6667f), new Vector2(0.3333f, 1));
                    AddTriangles(false);

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

        void AddQuadUVs(Vector2 BottomLeftCorner, Vector2 TopRightCorner) {
            uvs.Add(new Vector2(BottomLeftCorner.x, BottomLeftCorner.y));
            uvs.Add(new Vector2(BottomLeftCorner.x, TopRightCorner.y));
            uvs.Add(new Vector2(TopRightCorner.x, TopRightCorner.y));
            uvs.Add(new Vector2(TopRightCorner.x, BottomLeftCorner.y));
        }

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
        //mesh.normals = vertices.ToArray();

        for (int i = 0; i < ObjectCount; i++) {
            int y = Random.Range(0, chunkGen.chunkLength);

            Vector2 p = cliffCirclePosses[y];
            int x = Random.Range(0, SidesWidth * 2);
            if (x > SidesWidth) {
                x += chunkGen.chunkWidth - SidesWidth +1;
            }
            else if (x <= SidesWidth)
            {
                x = -x;
            }
            Vector3 pos = new Vector3(x -1,p.y,p.x);

            spawnDesertObjects(DirOnCircle(y), pos);
        }
    }

    Vector2[] possesOnCircle(int length, float radius) {
        Vector2[] posses = new Vector2[length];
        for (int y = 0; y < length; y++) {
            float deg = ((2 * Mathf.PI) / (chunkGen.chunkCount * chunkGen.chunkLength));
            float dir = (deg * y) + (deg * chunkGen.chunkLength * chunkGen.chunkIndex);
            posses[y] = new Vector2(Mathf.Cos(dir) * radius, Mathf.Sin(dir) * radius);
        }
        return posses;
    }

    Quaternion DirOnCircle(int y) {
        float deg = ((2 * Mathf.PI) / (chunkGen.chunkCount * chunkGen.chunkLength));
        float dir = (deg * y) + (deg * chunkGen.chunkLength * chunkGen.chunkIndex);
        return Quaternion.Euler(-dir * 57.3f, 0, 0); ;
    }

    void spawnDesertObjects(Quaternion rotation, Vector3 pos) {
        int randomObject = Random.Range(0, desertObjects.Length);
        GameObject.Instantiate(desertObjects[randomObject], pos, rotation, transform);
    }
}