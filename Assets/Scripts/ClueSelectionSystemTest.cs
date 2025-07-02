using System.Collections.Generic;
using UnityEngine;

public class ClueSelectionSystemTest : MonoBehaviour
{
    [SerializeField] List<GameObject> clues;
    [SerializeField] GameObject conclusion;
    [SerializeField] Color targetColor = Color.green;

    public void CheckForCorrectClues()
    {
        if (clues.Count < 4)
        {
            Debug.LogWarning("No hay suficientes elementos en la lista 'clues'");
            return;
        }

        bool allCorrect = true;
        for (int i = 1; i <= 3; i++)
        {
            Renderer renderer = clues[i].GetComponent<Renderer>();
            if (renderer == null || renderer.material.color != targetColor)
            {
                allCorrect = false;
                break;
            }
        }

        conclusion.SetActive(allCorrect);
    }
}
