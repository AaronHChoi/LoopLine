using TMPro;
using UnityEngine;

public class InteractUI : MonoBehaviour
{
    [SerializeField] private GameObject containerInteractUI;
    [SerializeField] PlayerInteract playerInteract;
    [SerializeField] private TextMeshProUGUI interactText;

    void Update()
    {
        if(playerInteract.GetInteractableObject() != null)
        {
            Show(playerInteract.GetInteractableObject());
        }
        else
        {
            Hide();
        }
    }

    private void Show(IInteract interactable)
    {
        containerInteractUI.SetActive(true);
        interactText.text = interactable.GetInteractText();
    }

    private void Hide()
    {
        containerInteractUI.SetActive(false);
    }
}
