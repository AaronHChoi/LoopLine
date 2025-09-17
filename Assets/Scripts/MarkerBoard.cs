using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerBoard : MonoBehaviour, IInteract
{
    public static MarkerBoard Instance;

    [SerializeField] GameObject Photo;

    [SerializeField] List<GameObject> clues;
    [SerializeField] List<GameObject> strings;
    [SerializeField] float delayBetween = 1f;
    [SerializeField] List<Material> material1;
    [SerializeField] Material material2;
    [SerializeField] Material material3;
    [SerializeField] List<GameObject> objectsToActivate;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            CheckTest();
        }
    }
    public string GetInteractText()
    {
        throw new System.NotImplementedException();
    }

    public void Interact()
    {
        CheckTest();
    }
    private void CheckTest()
    {
        if (GameManager.Instance.CameraGirlPhoto)
        {
            //Photo.SetActive(true);
            StartCoroutine(ChangeMaterialsInSequence(clues, delayBetween));
            StartCoroutine(ActivateObjectsInSequence(objectsToActivate, delayBetween));
            StartCoroutine(ChangeMaterialInSequence(strings, material3, 0, delayBetween));
        }
    }
    IEnumerator ChangeMaterialsInSequence(List<GameObject> objects, float delay)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            yield return new WaitForSeconds(delay);
            
            Renderer rend = objects[i].GetComponent<Renderer>();
            if (rend != null)
            {
                Material[] mats = rend.materials;
                if (mats.Length > 0 && i < material1.Count)
                    mats[0] = material1[i];

                mats[1] = material2;

                rend.materials = mats;
            }
            if (i == 0)
                Photo.SetActive(true);
        }
    }
    IEnumerator ChangeMaterialInSequence(List<GameObject> objects, Material newMaterial, int materialIndex, float delay)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            yield return new WaitForSeconds(delay);

            Renderer rend = objects[i].GetComponent<Renderer>();
            if (rend != null)
            {
                Material[] mats = rend.materials;
                if (materialIndex >= 0 && materialIndex < mats.Length)
                {
                    mats[materialIndex] = newMaterial;
                    rend.materials = mats;
                }
            }

            if (i == 0)
                Photo.SetActive(true);
        }
    }
    IEnumerator ActivateObjectsInSequence(List<GameObject> objects, float delay)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            yield return new WaitForSeconds(delay);
            if (objects[i] != null)
                objects[i].SetActive(true);
        }
    }
}
