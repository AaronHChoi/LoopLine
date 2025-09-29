using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MarkerBoard : MonoBehaviour
{
    [SerializeField] protected GameObject photo;
    [SerializeField] protected float delayBetween = 1f;

    protected virtual void Awake() { }
    protected IEnumerator ChangeMaterialsInSequence(List<GameObject> objects, List<Material> firstSlotMaterials, Material secondSlotMaterial)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            yield return new WaitForSeconds(delayBetween);

            Renderer rend = objects[i].GetComponent<Renderer>();
            if (rend != null)
            {
                var mats = rend.materials;
                if (mats.Length > 0 && i < firstSlotMaterials.Count)
                    mats[0] = firstSlotMaterials[i];

                if (mats.Length > 1)
                    mats[1] = secondSlotMaterial;

                rend.materials = mats;
            }

            if (i == 0 && photo != null)
                photo.SetActive(true);
        }
    }

    protected IEnumerator ChangeMaterialInSequence(List<GameObject> objects, Material newMaterial, int materialIndex)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            yield return new WaitForSeconds(delayBetween);

            Renderer rend = objects[i].GetComponent<Renderer>();
            if (rend != null && materialIndex >= 0 && materialIndex < rend.materials.Length)
            {
                var mats = rend.materials;
                mats[materialIndex] = newMaterial;
                rend.materials = mats;
            }

            if (i == 0 && photo != null)
                photo.SetActive(true);
        }
    }
    protected IEnumerator ActivateObjectsInSequence(List<GameObject> objects)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            yield return new WaitForSeconds(delayBetween);
            if (objects[i] != null) objects[i].SetActive(true);
        }
    }
}