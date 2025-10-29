using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ClockJitter : MonoBehaviour
{
    [Range(0f, 0.001f)] public float jitterAmount = 0f;
    [Range(0f, 0.01f)] public float jitterSpeed = 0f;

    Mesh mesh;
    Vector3[] baseVerts, workingVerts;

    void Start()
    {
        mesh = Instantiate(GetComponent<MeshFilter>().sharedMesh);
        mesh.MarkDynamic();
        GetComponent<MeshFilter>().mesh = mesh;

        baseVerts = mesh.vertices;
        workingVerts = new Vector3[baseVerts.Length];
    }

    void Update()
    {
        float t = Time.time * (jitterSpeed * 0.1f); // slower motion
        for (int i = 0; i < baseVerts.Length; i++)
        {
            Vector3 v = baseVerts[i];
            float hash = Mathf.Sin(Vector3.Dot(v, new Vector3(12.9898f, 78.233f, 37.719f)) + t) * 43758.5453f;
            Vector3 offset = new Vector3(
                Mathf.Sin(hash * 0.3f) * jitterAmount * 0.3f,
                Mathf.Cos(hash * 0.2f) * jitterAmount * 0.3f,
                Mathf.Sin(hash * 0.4f) * jitterAmount * 0.3f
            );
            workingVerts[i] = v + offset;
        }

        mesh.vertices = workingVerts;
        mesh.RecalculateNormals();
    }

}