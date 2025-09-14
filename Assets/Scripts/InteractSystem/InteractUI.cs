using DependencyInjection;
using TMPro;
using UnityEngine;
//using static Unity.Cinemachine.InputAxisControllerBase<T>;

public class InteractUI : MonoBehaviour
{
    [SerializeField] private GameObject containerInteractUI;
    [SerializeField] private TextMeshProUGUI interactText;
    private bool interactionLocked = false;

    private IPlayerInteract playerInteract;
    private IDialogueManager dialogueManager;
    private void Awake()
    {
        playerInteract = InterfaceDependencyInjector.Instance.Resolve<IPlayerInteract>();
        dialogueManager = InterfaceDependencyInjector.Instance.Resolve<IDialogueManager>();
    }
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
        dialogueManager.OnDialogueStarted += LockInteraction;
        dialogueManager.OnDialogueEnded += UnlockInteraction;
    }
    private void OnDisable()
    {
        dialogueManager.OnDialogueStarted -= LockInteraction;
        dialogueManager.OnDialogueEnded -= UnlockInteraction;
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
