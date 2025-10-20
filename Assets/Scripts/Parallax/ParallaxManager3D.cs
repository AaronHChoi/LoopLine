using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager3D : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float resetDistance = -50f;
    [SerializeField] private float forwardOffset = 100f;
    [SerializeField] private List<Transform> parallaxObjects = new List<Transform>();

    void Update()
    {
        foreach (var obj in parallaxObjects)
        {
            obj.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);

            if (obj.position.z < resetDistance)
            {
                Vector3 newPos = obj.position;
                newPos.z += forwardOffset;
                obj.position = newPos;
            }
        }
    }
}