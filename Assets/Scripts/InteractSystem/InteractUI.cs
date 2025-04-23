using TMPro;
using UnityEngine;

public class InteractUI : MonoBehaviour
{
    [SerializeField] private GameObject containerInteractUI;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private TextMeshProUGUI interactText;
    private bool interactionLocked = false;
    void Update()
    {
        if (interactionLocked)
        {
            Hide();
            return;
        }
        var interactable = playerInteract.GetInteractableObject();

        if(interactable != null)
        {
            Show(interactable);
        }
        else
        {
            Hide();
        }
    }
    private void OnEnable()
    {
        DialogueManager.OnDialogueStarted += LockInteraction;
        DialogueManager.OnDialogueEnded += UnlockInteraction;
    }
    private void OnDisable()
    {
        DialogueManager.OnDialogueStarted -= LockInteraction;
        DialogueManager.OnDialogueEnded -= UnlockInteraction;
    }
    private void Show(IInteract interactable)
    {
        containerInteractUI.SetActive(true);
        interactText.text = interactable.GetInteractText();
    }

    private void Hide()
    {
        //Debug.Log("Ocultando Interact UI por evento");
        containerInteractUI.SetActive(false);
    }
    private void LockInteraction()
    {
        interactionLocked = true;
        Hide();
    }
    private void UnlockInteraction()
    {
        interactionLocked = false;
    }
}
