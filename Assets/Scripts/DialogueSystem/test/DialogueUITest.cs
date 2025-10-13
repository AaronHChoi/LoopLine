using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUITest : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button continueButton;

    private void Start()
    {
        dialoguePanel.SetActive(false);

        continueButton.onClick.AddListener(() => DialogueManagerTest.Instance.DisplayNextLine());
    }

    public void ShowDialogueUI()
    {
        dialoguePanel.SetActive(true);
    }

    public void HideDialogueUI()
    {
        dialoguePanel.SetActive(false);
    }

    public void DisplayDialogueLine( string dialogue)
    {
        dialogueText.text = dialogue;
    }
}