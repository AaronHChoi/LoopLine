using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager3D : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;   
    [SerializeField] private float teleportZ = 254f;
    [SerializeField] private float limitZ = -706f;
    [SerializeField] private List<Transform> movingObjects = new List<Transform>();

    [SerializeField] private float gizmoHeight = 20f;  
    [SerializeField] private Color limitColor = Color.red;
    [SerializeField] private Color teleportColor = Color.green;
    void LateUpdate()
    {
        foreach (var obj in movingObjects)
        {
            if (obj == null) continue;

            obj.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);

            if (obj.position.z <= limitZ)
            {
                Vector3 newPos = obj.position;
                newPos.z = teleportZ;
                obj.position = newPos;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = limitColor;
        Vector3 limitStart = new Vector3(-1000, gizmoHeight, limitZ);
        Vector3 limitEnd = new Vector3(1000, gizmoHeight, limitZ);
        Gizmos.DrawLine(limitStart, limitEnd);

        Gizmos.color = teleportColor;
        Vector3 teleportStart = new Vector3(-1000, gizmoHeight, teleportZ);
        Vector3 teleportEnd = new Vector3(1000, gizmoHeight, teleportZ);
        Gizmos.DrawLine(teleportStart, teleportEnd);

#if UNITY_EDITOR
        UnityEditor.Handles.Label(new Vector3(0, gizmoHeight + 2, limitZ), "Límite Z");
        UnityEditor.Handles.Label(new Vector3(0, gizmoHeight + 2, teleportZ), "Teletransporte Z");
#endif
    }
}