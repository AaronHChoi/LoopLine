using UnityEngine;

public class Clue : MonoBehaviour, IInteract
{
    int currentColorIndex = -1;
    Color[] colorSequence = { Color.green, Color.red };
    Renderer rend;
    ClueSelectionSystemTest test;
    private void Awake()
    {
        test = FindFirstObjectByType<ClueSelectionSystemTest>();
        rend = GetComponent<Renderer>();
        if(rend != null)
        {
            rend.material = new Material(rend.sharedMaterial);
            rend.material.color = Color.cyan;
        }
    }
    public string GetInteractText()
    {
        throw new System.NotImplementedException();
    }

    public void Interact()
    {
        currentColorIndex = (currentColorIndex + 1) % colorSequence.Length;
        rend.material.color = colorSequence[currentColorIndex];

        test.CheckForCorrectClues();
    }
}