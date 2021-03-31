using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class LightMesh : MonoBehaviour
{
    public float lightRange;
    public Material lightMaterial;
    public int resolution = 30;

    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    public void Awake()
    {

        gameObject.AddComponent<MeshRenderer>();
        gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
    }

    private void Update()
    {
        Vector3[] vertices = PointsToTris(CalculateVertexPoints(resolution));

        if (vertices != null)
        {
            for (int i = 0; i < vertices.Length; i++)
            {

                Vector2 pos = Vector2.zero;
                if (i > 0)
                {
                    Debug.DrawLine(transform.localToWorldMatrix.MultiplyPoint(vertices[i]), transform.localToWorldMatrix.MultiplyPoint(vertices[i - 1]), Color.yellow);
                }
                else
                {
                    Debug.DrawLine(transform.localToWorldMatrix.MultiplyPoint(vertices[i]), transform.localToWorldMatrix.MultiplyPoint(vertices[vertices.Length - 1]), Color.yellow);
                }
            }
            GenerateTri(vertices);
        }
    }

    public Vector3[] PointsToTris(Vector3[] verts)
    {
        if(verts!=null)
        {
            List<Vector3> vertsToTris = new List<Vector3>();
            for (int i = 0; i < verts.Length; i++)
            {
                if (i == verts.Length - 1)
                {
                    // start with center vertex
                    vertsToTris.Add(Vector3.zero);
                    // goto vertex ahead
                    vertsToTris.Add(verts[i]);
                    // goto next vertex
                    vertsToTris.Add(verts[0]);
                    // go back to center vertex
                    //vertsToTris.Add(Vector3.zero);
                }
                else
                {
                    // start with center vertex
                    vertsToTris.Add(Vector3.zero);
                    // goto vertex ahead
                    vertsToTris.Add(verts[i]);
                    // goto next vertex
                    vertsToTris.Add(verts[i + 1]);
                    // go back to center vertex
                    //vertsToTris.Add(Vector3.zero);
                }
            }
            return vertsToTris.ToArray();
        }
       return null;
    }

    public Vector3[] CalculateVertexPoints(int res)
    {
        if (resolution <= 2)
            return null;

        Vector3[] verts = new Vector3[res];
        float[] dists = new float[res];


        for (int i = 0; i < resolution; i++)
        {
            float progress = ((float)i) / resolution;
            float angle = progress * Mathf.PI * 2;
            Vector3 origin = transform.position;
            Vector3 direction = transform.rotation * new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle));
            Ray ray = new Ray(origin, direction);

            Vector3 LocalHitPoint;
            if (Physics.Raycast(ray, out RaycastHit hit, lightRange))
            {
                dists[i] = hit.distance;
                verts[i] = ray.direction * hit.distance;

            }
            else
            {
                LocalHitPoint = transform.localToWorldMatrix.MultiplyPoint(ray.direction * lightRange);
                dists[i] = float.MaxValue;
                verts[i] = ray.direction * lightRange;

            }
        }

        return verts;
    }

    public void GenerateTri(Vector3[] verts)
    {
        GetComponent<Renderer>().sharedMaterial = lightMaterial;

        //meshFilter = gameObject.GetComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        mesh.vertices = verts;

        List<int> tris = new List<int>();
        for (int i = 0; i < verts.Length; i++)
        {
            tris.Add(i);
        }

        mesh.triangles = tris.ToArray();
        meshFilter.mesh = mesh;
    }
}