using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ParallaxLayer
{
    public string name;
    public float moveSpeed = 5f;
    public List<Transform> objects = new List<Transform>();
    public float spacing;
}
public class ParallaxManager3D : MonoBehaviour
{
    [SerializeField] private float limitZ = -706f;
    [SerializeField] private List<ParallaxLayer> layers = new List<ParallaxLayer>();
    
    //[SerializeField] private Color limitColor = Color.red;
    //[SerializeField] private Color teleportColor = Color.green;

    private void Start()
    {
        foreach (var layer in layers)
        {
            if (layer.objects.Count < 2) continue;

            layer.objects.Sort((a, b) => b.position.z.CompareTo(a.position.z));

            float total = 0f;
            for (int i = 0; i < layer.objects.Count - 1; i++)
            {
                total += Mathf.Abs(layer.objects[i].position.z - layer.objects[i + 1].position.z);
            }

            layer.spacing = total / (layer.objects.Count - 1);
        }
    }
    void LateUpdate()
    {
        foreach (var layer in layers)
        {
            if (layer.objects.Count == 0) continue;

            foreach (var obj in layer.objects)
            {
                if (obj == null) continue;
                obj.Translate(Vector3.back * layer.moveSpeed * Time.deltaTime, Space.World);
            }

            foreach (var obj in layer.objects)
            {
                if (obj == null) continue;

                if (obj.position.z <= limitZ)
                {
                    Transform first = GetFrontMostObject(layer.objects);

                    Vector3 newPos = obj.position;
                    newPos.z = first.position.z + layer.spacing;
                    obj.position = newPos;
                }
            }
        }
    }
    private Transform GetFrontMostObject(List<Transform> objs)
    {
        Transform front = objs[0];
        foreach (var o in objs)
        {
            if (o.position.z > front.position.z)
                front = o;
        }
        return front;
    }
//    private void OnDrawGizmos()
//    {
//        Gizmos.color = limitColor;
//        Vector3 limitStart = new Vector3(-1000, gizmoHeight, limitZ);
//        Vector3 limitEnd = new Vector3(1000, gizmoHeight, limitZ);
//        Gizmos.DrawLine(limitStart, limitEnd);

//#if UNITY_EDITOR
//        UnityEditor.Handles.Label(new Vector3(0, gizmoHeight + 2, limitZ), "Límite Z");
//#endif
//    }
}