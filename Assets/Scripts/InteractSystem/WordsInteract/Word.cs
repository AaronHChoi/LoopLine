using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class Word : MonoBehaviour, IWord /*IInteract*/
{
    [SerializeField] private bool isCorrectWord;
    [SerializeField] private TextMeshPro tmpro;
    public string word;
    public int numerofWord; 

    [Header("LookAt")]
    private PlayerController playerController;
    [SerializeField] private int range = 2;
    Vector3 _direction;
    private void Awake()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        tmpro = GetComponentInChildren<TextMeshPro>();
    }
    private void Start()
    {
        if (playerController == null)
        {
            Debug.LogError("PlayerController not found in the scene.");
        }
        if (tmpro == null)
        {
            Debug.LogError("TextMeshPro component not found in children.");
        }
    }
    public void Interacted()
    {
        if (isCorrectWord)
        {
            tmpro.color = Color.green;
            GameManager.Instance.CorrectWord101 = true;
        }
        else
        {
            SceneManager.LoadScene("Train");
        }
    }
    private void Update()
    {
        tmpro.text = word + " " + "[" + numerofWord.ToString()+ "]";
        if (Vector3.Distance(transform.position, playerController.transform.position) <= range)
        {
            _direction = playerController.transform.position - transform.position;
            _direction.y = 0;

            transform.rotation = Quaternion.LookRotation(_direction);
        }
    }
}