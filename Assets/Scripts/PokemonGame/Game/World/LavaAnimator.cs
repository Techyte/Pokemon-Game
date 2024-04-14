using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class LavaAnimator : MonoBehaviour
{
    private Mesh myMesh;
    private MeshFilter _meshFilter;

    [SerializeField] private Vector2 planeSize = new Vector2(1, 1);
    [SerializeField] private int planeResolution = 1;
    [SerializeField] private float heightMultiplier = 1;
    [SerializeField] private float cordMultiplier = 1;

    private List<Vector3> vertices;
    private List<int> triangles;

    private void Awake()
    {
        myMesh = new Mesh();
        _meshFilter = GetComponent<MeshFilter>();
        _meshFilter.mesh = myMesh;
    }

    private void Update()
    {
        planeResolution = Mathf.Clamp(planeResolution, 1, 50);
        
        GeneratePlane(planeSize, planeResolution);
        RippleSine(Time.timeSinceLevelLoad);
        AssignMesh();
    }

    private void GeneratePlane(Vector2 size, int resolution)
    {
        vertices = new List<Vector3>();
        float xPerStep = size.x / resolution;
        float yPerStep = size.y / resolution;
        for (int y = 0; y < resolution+1; y++)
        {
            for (int x = 0; x < resolution+1; x++)
            {
                vertices.Add(new Vector3(x * xPerStep, 0, y * yPerStep));
            }
        }

        triangles = new List<int>();
        for (int row = 0; row < resolution; row++)
        {
            for (int column = 0; column < resolution; column++)
            {
                int i = (row * resolution) + row + column;
                
                triangles.Add(i);
                triangles.Add(i+(resolution)+1);
                triangles.Add(i+(resolution)+2);
                
                triangles.Add(i);
                triangles.Add(i+resolution+2);
                triangles.Add(i+1);
            }
        }
    }

    private void AssignMesh()
    {
        myMesh.Clear();
        myMesh.vertices = vertices.ToArray();
        myMesh.triangles = triangles.ToArray();
    }

    private void LeftToRightSine(float time)
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            Vector3 vertex = vertices[i];
            vertex.y = Mathf.Sin(time + vertex.x * cordMultiplier) * heightMultiplier;
            vertices[i] = vertex;
        }
    }

    private void RippleSine(float time)
    {
        Vector3 origin = new Vector3(planeSize.x / 2, 0, planeSize.y / 2);

        for (int i = 0; i < vertices.Count; i++)
        {
            Vector3 vertex = vertices[i];
            float distanceFromCenter = (vertex - origin).magnitude;
            vertex.y = Mathf.Sin(time + distanceFromCenter * cordMultiplier) * heightMultiplier;
            vertices[i] = vertex;
        }
    }
}
