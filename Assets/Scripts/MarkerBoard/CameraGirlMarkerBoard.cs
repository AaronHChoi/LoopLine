using System.Collections.Generic;
using UnityEngine;

public class CameraGirlMarkerBoard : MarkerBoard, IInteract
{
    public static CameraGirlMarkerBoard Instance;

    [SerializeField] List<GameObject> clues;
    [SerializeField] List<GameObject> strings;
    [SerializeField] List<Material> material1;
    [SerializeField] Material material2;
    [SerializeField] Material material3;
    [SerializeField] List<GameObject> objectsToActivate;

    protected override void Awake()
    {
        base.Awake();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public string GetInteractText()
    {
        throw new System.NotImplementedException();
    }
    public void Interact()
    {
        if (GameManager.Instance.CameraGirlPhoto)
        {
            StartCoroutine(ChangeMaterialsInSequence(clues, material1, material2));
            StartCoroutine(ActivateObjectsInSequence(objectsToActivate));
            StartCoroutine(ChangeMaterialInSequence(strings, material3, 0));
        }
    }
    public void DeactivateMakerBoard() => gameObject.SetActive(false);
    public void ActivateMakerBoard() => gameObject.SetActive(true);
}