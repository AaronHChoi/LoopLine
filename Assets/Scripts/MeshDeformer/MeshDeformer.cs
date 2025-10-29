using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshDeformer : MonoBehaviour {
    Mesh deformingMesh;
    Vector3[] originalVertices, displacedVertices, vertexVelocities;
    float uniformScale = 1f;

    [Header("Spring physics")]
    public float springForce = 20f;
    public float damping = 5f;

    [Header("Collider")]
    public bool updateMeshCollider = false;
    MeshCollider meshCollider;

    void Start() {
        var mf = GetComponent<MeshFilter>();
        // instantiate to avoid changing shared mesh across clocks
        deformingMesh = Instantiate(mf.mesh);
        deformingMesh.MarkDynamic();
        mf.mesh = deformingMesh;

        originalVertices = deformingMesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
        vertexVelocities = new Vector3[originalVertices.Length];
        for (int i = 0; i < originalVertices.Length; i++)
            displacedVertices[i] = originalVertices[i];

        if (updateMeshCollider)
            meshCollider = GetComponent<MeshCollider>();
    }

    void Update() {
        uniformScale = transform.localScale.x;
        for (int i = 0; i < displacedVertices.Length; i++)
            UpdateVertex(i);

        deformingMesh.vertices = displacedVertices;
        deformingMesh.RecalculateNormals();

        if (updateMeshCollider && meshCollider != null)
            meshCollider.sharedMesh = deformingMesh;
    }

    void UpdateVertex(int i) {
        Vector3 velocity = vertexVelocities[i];

        Vector3 displacement = displacedVertices[i] - originalVertices[i];
        // compensate for scale when computing spring
        displacement *= uniformScale;
        velocity -= displacement * springForce * Time.deltaTime;

        // damping
        vertexVelocities[i] = velocity * (1f - damping * Time.deltaTime);
        displacedVertices[i] += vertexVelocities[i] * Time.deltaTime;
    }

    public void AddDeformingForce(Vector3 pointWorld, float force) {
        // convert to local space so transform/rotation are handled
        Vector3 point = transform.InverseTransformPoint(pointWorld);

        for (int i = 0; i < displacedVertices.Length; i++)
            AddForceToVertex(i, point, force);
    }

    void AddForceToVertex(int i, Vector3 point, float force) {
        Vector3 pointToVertex = displacedVertices[i] - point;
        // compensate for uniform scale before distance calc
        pointToVertex *= uniformScale;
        float attenuatedForce = force / (1f + pointToVertex.sqrMagnitude);
        float velocity = attenuatedForce * Time.deltaTime;
        vertexVelocities[i] += pointToVertex.normalized * velocity;
    }
}
