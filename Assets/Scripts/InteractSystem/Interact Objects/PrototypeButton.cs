using UnityEngine;
using UnityEngine.SceneManagement;

public class PrototypeButton : MonoBehaviour, IInteract
{
    [SerializeField] private string interactText = "Volver al tren";

    public string GetInteractText()
    {
        return interactText;
    }

    public void Interact()
    {
        SceneManager.LoadScene("Main");
    }
}
