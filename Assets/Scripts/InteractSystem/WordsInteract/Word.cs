using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class Word : MonoBehaviour, IWord, IInteract
{

    [SerializeField] private bool isCorrectWord;
    [SerializeField] private string interactText = "Interact";
    private TextMeshPro tmpro;
    public string word;

    [Header("LookAt")]
    private PlayerController playerController;
    [SerializeField] private int range = 2;
    Vector3 _direction;

    public string GetInteractText()
    {
        return interactText;
    }
    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("PlayerController not found in the scene.");
        }
        tmpro = GetComponentInChildren<TextMeshPro>();
        if (tmpro == null)
        {
            Debug.LogError("TextMeshPro component not found in children.");
        }

    }
    public void Interact()
    {
        if (isCorrectWord)
        {
            tmpro.color = Color.green;
        }
        else
        {
            SceneManager.LoadScene("Main");
        }
    }

    

    private void Update()
    {
        tmpro.text = word;
        if (Vector3.Distance(transform.position, playerController.transform.position) <= range)
        {

            _direction = playerController.transform.position - transform.position;
            _direction.y = 0;

            transform.rotation = Quaternion.LookRotation(_direction);
        }
    }
}
