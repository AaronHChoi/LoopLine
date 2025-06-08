using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class LineOfSight : MonoBehaviour, ILineOfSight
{
    public float range;
    [Range(1, 360)]
    public float angle;
    public LayerMask maskObs;
    [SerializeField] private GameObject rotationToCopy;

    public bool CheckRange(Transform target)
    {
        return CheckRange(target, range);
    }
    public bool CheckRange(Transform target, float range)
    {
        float distance = Vector3.Distance(target.position, Origin);
        return distance <= range;
    }
    public bool CheckAngle(Transform target)
    {
        return CheckAngle(target, angle);
    }
    public bool CheckAngle(Transform target, float angle)
    {
        Vector3 dirToTarget = target.position - Origin;
        float angleToTarget = Vector3.Angle(Forward, dirToTarget);
        return angleToTarget <= angle / 2;
    }
    public bool CheckView(Transform target)
    {
        return CheckView(target, maskObs);
    }
    public bool CheckView(Transform target, LayerMask maskObs)
    {
        Vector3 dirToTarget = target.position - Origin;
        float distance = dirToTarget.magnitude;
        return !Physics.Raycast(Origin, dirToTarget, distance, maskObs);
    }

    private void Update()
    {
        if (rotationToCopy != null)
        {
            gameObject.transform.rotation = rotationToCopy.transform.rotation;
        }
    }

    public List<GameObject> GetObjectsInSight()
    {
        List<GameObject> visibleObjects = new List<GameObject>();

        Collider[] colliders = Physics.OverlapSphere(transform.position, range, maskObs);

        foreach (Collider collider in colliders)
        {
            Vector3 dirToTarget = (collider.transform.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(Forward, dirToTarget);

            if (angleToTarget <= angle / 2f)
            {
                float distanceToTarget = Vector3.Distance(transform.position, collider.transform.position);
                if (distanceToTarget <= range)
                {
                    if (!Physics.Raycast(transform.position, dirToTarget, distanceToTarget, maskObs))
                    {
                        visibleObjects.Add(collider.gameObject);
                    }
                }
            }
        }
        return visibleObjects;
    }
    Vector3 Origin => transform.position;
    Vector3 Forward => transform.forward;   
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Origin, range);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Origin, Quaternion.Euler(0, angle / 2, 0) * Forward * range);
        Gizmos.DrawRay(Origin, Quaternion.Euler(0, -(angle / 2), 0) * Forward * range);
    }
}
