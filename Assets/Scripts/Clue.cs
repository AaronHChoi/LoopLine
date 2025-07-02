using UnityEngine;

public class Clue : MonoBehaviour, IInteract
{
    [SerializeField] Material material;
    Color originalColor;
    int currentColorIndex = 0;

    Color[] colorSequence;
    ClueSelectionSystemTest test;
    private void Awake()
    {
        test = FindFirstObjectByType<ClueSelectionSystemTest>();
        if (material != null)
        {
            originalColor = material.color;
            colorSequence = new Color[]
            {
                originalColor,
                Color.green,
                Color.red
            };
        }
    }
    public string GetInteractText()
    {
        throw new System.NotImplementedException();
    }

    public void Interact()
    {
        currentColorIndex = (currentColorIndex +1) % colorSequence.Length;
        material.color = colorSequence[currentColorIndex];
        test.CheckForCorrectClues();
    }
}
