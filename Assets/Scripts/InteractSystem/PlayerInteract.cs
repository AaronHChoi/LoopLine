using NUnit.Framework;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerInteract : MonoBehaviour, IDependencyInjectable
{
    private CinemachineCamera rayCastPoint;
    [SerializeField] private float raycastDistance = 2f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private LineOfSight lineOfSight;
    [SerializeField] private RawImage circleImage;
    [SerializeField] private float maxAlpha = 1f;
    private Transform currentTarget;

    private void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        rayCastPoint = provider.CinemachineCamera;
    }
    void Update()
    {
        Debug.DrawRay(rayCastPoint.transform.position, rayCastPoint.transform.forward * raycastDistance, Color.red);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            IInteract interactableObject = GetInteractableObject();
            if (interactableObject != null)
            {
                interactableObject.Interact();
            }
        }

        if (lineOfSight == null || circleImage == null) return;

        currentTarget = GetMostCenteredInteractable();

        if (currentTarget == null)
        {
            SetAlpha(0f);
            return;
        }

        Vector3 dirToTarget = currentTarget.position - lineOfSight.transform.position;
        float angleToTarget = Vector3.Angle(lineOfSight.transform.forward, dirToTarget);

        float maxAngle = lineOfSight.angle / 2;

        if (angleToTarget <= maxAngle)
        {
            
            float alpha = Mathf.Lerp(maxAlpha, 0f, angleToTarget / maxAngle);
            SetAlpha(alpha);
        }
        else
        {
            SetAlpha(0f); 
        }
    }

    public IInteract GetInteractableObject()
    {
        //List<IInteract> InteractableList = new List<IInteract>();

        Ray ray = new Ray(rayCastPoint.transform.position, rayCastPoint.transform.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, raycastDistance, interactableLayer);

        IInteract interactableObject = null;

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.TryGetComponent(out IInteract interactable))
            {
                interactableObject = interactable;
                //interactableLayer = hit.collider.gameObject.layer;
            }
        }

        return interactableObject;
    }

    private Transform GetMostCenteredInteractable()
    {
        float closestAngle = float.MaxValue;
        Transform mostCentered = null;

        foreach (var visible in lineOfSight.GetObjectsInSight())
        {
            if (visible.TryGetComponent<IInteract>(out _))
            {
                Vector3 dir = visible.transform.position - lineOfSight.transform.position;
                float angle = Vector3.Angle(lineOfSight.transform.forward, dir);

                if (angle < closestAngle)
                {
                    closestAngle = angle;
                    mostCentered = visible.transform;
                }
            }
        }

        return mostCentered;
    }

    void SetAlpha(float alpha)
    {
        Color c = circleImage.color;
        c.a = alpha;
        circleImage.color = c;
    }
}
