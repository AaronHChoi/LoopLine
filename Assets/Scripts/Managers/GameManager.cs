using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] public float LoopTime { get; private set; } = 360f;

    [SerializeField] private string nextScene;

    public bool changeLoopTime = false; 

    private DevelopmentManager developmentManager;
    private float iniatialLoopTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        developmentManager = FindAnyObjectByType<DevelopmentManager>();
        iniatialLoopTime = LoopTime;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (changeLoopTime && LoopTime >= 5f)
        {
            LoopTime = 5f;
        }
        if (SceneManager.GetActiveScene().name == "Main")
        {
            
            LoopTime -= Time.deltaTime;
            if (Input.GetKey(KeyCode.F))
            {
                LoopTime -= Time.deltaTime * 8f;
            }
            if (LoopTime <= 0)
            {
                LoopTime = 360f;
                LoadNextScene(nextScene);
            }
        }
    }

    private void LoadNextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
