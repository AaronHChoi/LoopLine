using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] public float LoopTime { get; private set; } = 360f;

    [SerializeField] private string nextScene;

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
        if (developmentManager.developmentMode == true && Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.T))
        {
            LoopTime = 5f;
        }
        if (SceneManager.GetActiveScene().name == "Main")
        {
            
            LoopTime -= Time.deltaTime;
            if (LoopTime <= 0)
            {
                LoopTime = 360f;
                LoadNextScene(nextScene);
            }
        }

        if (SceneManager.GetActiveScene().name == "ThinkingWorld")
        {
            if (developmentManager.developmentMode == true && Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.L)) 
            {
                SceneManager.LoadScene("Main");
            }
        }
    }

    private void LoadNextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
