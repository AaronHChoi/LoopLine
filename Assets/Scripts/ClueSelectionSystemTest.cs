using System.Collections.Generic;
using UnityEngine;

public class ClueSelectionSystemTest : MonoBehaviour
{
    [SerializeField] List<GameObject> clues;
    [SerializeField] GameObject conclusion;
    [SerializeField] Color targetColor = Color.green;
    [SerializeField] Material material;
    [SerializeField] Material mainMaterial;
    public void CheckForCorrectClues()
    {
        if (clues.Count < 5) { return; }

        bool allCorrect = IsClueCorrect(2) && IsClueCorrect(4) && IsClueCorrect(5);

        conclusion.SetActive(allCorrect);

        if (allCorrect)
        {
            mainMaterial.CopyPropertiesFromMaterial(material);
        }
    }

    private bool IsClueCorrect(int index)
    {
        Renderer renderer = clues[index].GetComponent<Renderer>();

        if (renderer == null) { return false; }

        return renderer.material.color == targetColor;
    }
}