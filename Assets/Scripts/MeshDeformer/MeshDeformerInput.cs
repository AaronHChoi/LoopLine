using UnityEngine;

public class MeshDeformerInput : MonoBehaviour {
    [Header("Ray settings")]
    public bool useScreenRay = false;
    public Transform rayOrigin; // e.g., character head or camera pivot
    public float maxDistance = 5f;
    public LayerMask layerMask = ~0;

    [Header("Force")]
    public float force = 10f;
    public float forceOffset = 0.05f;

    void Update() {
        // example trigger: left mouse button or Fire1. Adjust to your input system.
        if (Input.GetMouseButton(0) || Input.GetButtonDown("Fire1")) {
            HandleInput();
        }
    }

    void HandleInput() {
        Ray ray;
        if (useScreenRay) {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        } else {
            if (rayOrigin == null) rayOrigin = transform;
            ray = new Ray(rayOrigin.position, rayOrigin.forward);
        }

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layerMask)) {
            var deformer = hit.collider.GetComponent<MeshDeformer>();
            if (deformer != null) {
                Vector3 point = hit.point + hit.normal * forceOffset;
                deformer.AddDeformingForce(point, force);
            }
            Debug.DrawLine(ray.origin, hit.point, Color.red, 1f);
        }
    }
}