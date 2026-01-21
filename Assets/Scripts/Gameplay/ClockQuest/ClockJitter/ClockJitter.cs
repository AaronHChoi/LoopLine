using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ClockJitter : MonoBehaviour
{
    [Range(0f, 0.001f)] public float jitterAmount = 0f;
    [Range(0f, 0.01f)] public float jitterSpeed = 0f;

    Mesh mesh;
    Vector3[] baseVerts, workingVerts;

    static readonly Vector3 noiseDir = new Vector3(12.9898f, 78.233f, 37.719f);

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
        if (jitterAmount <= 0f || jitterSpeed <= 0f) return; // skip work if no jitter or speed

        float t = Time.time * (jitterSpeed * 0.1f);
        float amt = jitterAmount * 0.3f;

        for (int i = 0; i < baseVerts.Length; i++)
        {
            Vector3 v = baseVerts[i];
            float hash = Mathf.Sin(Vector3.Dot(v, noiseDir) + t) * 43758.5453f;

            float sinH = Mathf.Sin(hash);
            float cosH = Mathf.Cos(hash);

            workingVerts[i] = new Vector3(
                v.x + sinH * amt * 0.3f,
                v.y + cosH * amt * 0.2f,
                v.z + Mathf.Sin(hash * 0.4f) * amt
            );
        }

        mesh.vertices = workingVerts;
        mesh.RecalculateNormals();
    }
}