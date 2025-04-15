using UnityEngine;

public class Prototype : MonoBehaviour, IInteract
{
    [SerializeField] private string interactText = "Interact with me!";

    public string GetInteractText()
    {
        return interactText;
    }

    public void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }

}
